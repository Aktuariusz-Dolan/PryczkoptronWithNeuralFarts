using System;
using System.Collections.Generic;
using System.Linq;

namespace MyNotSoLittlePryczkoptron
{
    public class KohonenLearning : Learn
    { 
    private List<Neuron> Neurons;
    private List<Point> Points;
    private List<Point> Errors; //x -> numer iteracji, y-> błąd

    private double MaxRange;
    private double MinRange;

    private double MaxSpeed;
    private double MinSpeed;

    private double MinimalPotential;

    private int EpochsCount = 100;

    private double LambdaParameter;

    private Neuron Winner;
    private List<Neuron> NeuronsToChange;

    private Func<Neuron, Neuron, double> ProximityFunctionType;

    public KohonenLearning(List<Neuron> ListOfNeurons, List<Point> ListOfPoints, Configuration Configuration)
    {
        this.Neurons = ListOfNeurons;
        this.Points = ListOfPoints;
        this.SetParameters(Configuration);
        this.Errors = new List<Point>();
        ProximityFunctionType = new ProximityFunction(Metric.Euclidean, Proximity.Rekt).CalculateProximity;
    }

    public void SetParameters(Configuration SourceConfiguration)
    {
        this.MaxRange = SourceConfiguration.GetMaxRange();
        this.MinRange = SourceConfiguration.GetMinRange();
        this.MaxSpeed = SourceConfiguration.GetMaxLearningRate();
        this.MinSpeed = SourceConfiguration.GetMinLearningRate();
        this.ProximityFunctionType = SourceConfiguration.GetFunction();
        this.MinimalPotential = SourceConfiguration.GetMinPotential();
    }
        
    public override void Train(int NumberOfIteration)
    {   Random Randomizer = new Random();
        ListShuffler Shuffler = new ListShuffler();
        
        Shuffler.Shuffle(Points, Randomizer);
        double CurrentError = 0;

        foreach(Point TargetPoint in Points)
        {
            this.SetTargetPoints(TargetPoint);

            LambdaParameter = MaxRange * Math.Pow(MinRange / MaxRange, NumberOfIteration / (double)EpochsCount);
            Winner = this.SeekForWinner();
            NeuronsToChange = this.GetNeuronsToChange();
            
            this.SetNeuronPotential(NumberOfIteration);

            CurrentError += Winner.CalculateDistanceFrom(TargetPoint);

            this.UpdateWeights(NumberOfIteration, TargetPoint);
        }
        foreach (Neuron Neuron in Neurons)
        {
            Neuron.UpdatePositions(Neuron.GetWeight(0), Neuron.GetWeight(1));
        }
        Errors.Add(new Point(NumberOfIteration, CurrentError / Points.Count()));
    }

    private void SetNeuronPotential(int CurrentIteration)
    {
        foreach(Neuron Neuron in Neurons)
        {
            double Potential;
            if (Neuron.Equals(Winner))
            {
                Potential = Neuron.GetPotential() - MinimalPotential;
            }
            else
            {
                Potential = Neuron.GetPotential() + 1 / (double)Neurons.Count();
            }
            Neuron.UpdatePotential(Potential);
        }
    }

    private Neuron SeekForWinner()
    {
        Neuron CurrentWinner = Neurons.First();
        double Distance = CurrentWinner.GetDistanceFromTarget();

        foreach (Neuron neuron in Neurons)
        {
            double WinnerDistance = neuron.GetDistanceFromTarget();
            if (WinnerDistance < Distance && neuron.GetPotential() >= MinimalPotential)
            {
                CurrentWinner = neuron;
                Distance = WinnerDistance;
            }
        }

        return CurrentWinner;
    }

    private List<Neuron> GetNeuronsToChange()
    {
        List<Neuron> NeuronsToUpdate = new List<Neuron>();

			foreach (Neuron Neuron in Neurons)
			{
				Point NeuronAsPoint = Neuron.GetAsPoint();
				if (Winner.CalculateDistanceFrom(NeuronAsPoint) < LambdaParameter)
				{
					NeuronsToUpdate.Add(Neuron);
				}
			}
        NeuronsToUpdate.Add(Winner);
        return NeuronsToUpdate;
    }

    private void UpdateWeights(int Iteration, Point TargetPoint)
    {
        for (int i = 0; i < NeuronsToChange.Count(); i++)
        {
            Neuron Current = NeuronsToChange[i];
            double[] WeightsOfNeuronToChange = NeuronsToChange[i].GetWeightsArray();
            WeightsOfNeuronToChange[0] = WeightsOfNeuronToChange[0] + SpeedFactor(Iteration) * ProximityFunctionType(Winner, Current) * (TargetPoint.XCoordinate - WeightsOfNeuronToChange[0]);
            WeightsOfNeuronToChange[1] = WeightsOfNeuronToChange[1] + SpeedFactor(Iteration) * ProximityFunctionType(Winner, Current) * (TargetPoint.YCoordinate - WeightsOfNeuronToChange[1]);
        }
    }

    private double SpeedFactor(int Iteration)
    {
        return MaxSpeed * Math.Pow(MinSpeed / MaxSpeed, Iteration / (double)EpochsCount);
    }

    private void SetTargetPoints(Point TargetPoint)
    {
        foreach(Neuron Neuron in Neurons)
        {
            Neuron.SetTargetPoint(TargetPoint);
        }
    }

    public List<Neuron> GetNeurons()
    {
        return Neurons;
    }

        public List<Point> ReturnNeuronsAsPoints()
        {
            List<Point> ReturnPoints = new List<Point>();
            foreach (Neuron Neuron in Neurons)
                ReturnPoints.Add(Neuron.GetAsPoint());
            return ReturnPoints;
        }

        public List<Point> GetErrors()
    {
        return Errors;
    }
    public double GetLast()
    {
        return Errors.Last().YCoordinate;
    }
    public void SetNeurons(List<Neuron> Neurons)
    {
        this.Neurons = Neurons;
    }

    public List<Point> GetPoints()
    {
        return Points;
    }

    public void SetPoints(List<Point> Points)
    {
        this.Points = Points;
    }
}
}
