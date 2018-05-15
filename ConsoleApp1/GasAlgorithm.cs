using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotSoLittlePryczkoptron
{
	public class NeuralGas : Learn
	{
	private ListShuffler Shuffler = new ListShuffler();
	private	Random Randomizer = new Random();

	private List<Neuron> Neurons;
	private List<Point> Points;
	private List<Point> Errors;

	private double MaxRange;
	private double MinRange;

	private double MaxSpeed;
	private double MinSpeed;

	private Neuron Winner;
	private NeuronGenerator NeuronGen;
	private int NumberOfEpochs;

	public Func<int, double> ProximityFunctionType;

		public NeuralGas(List<Neuron> ListOfNeurons, List<Point> ListOfPoints, Configuration Configuration, NeuronGenerator Generator)
		{
        this.Neurons = ListOfNeurons;
        this.Points = ListOfPoints;
        this.SetParams(Configuration);
        this.Errors = new List<Point>();
		ProximityFunctionType = new ProximityFunction(Metric.Euclidean, Proximity.Gauss).CalculateGasProximity;
		NeuronGen = new NeuronGenerator();
		}

	private void SetTargetPoint(Point Target)
	{
		foreach(Neuron Neuron in Neurons)
		{
			Neuron.SetTargetPoint(Target);
		}
	}
		private double SpeedFactor(int Iteration)
		{
			return MaxSpeed * Math.Pow(MinSpeed / MaxSpeed, (Double)Iteration / (Double)NumberOfEpochs);
		}

		public void SetParams(Configuration SourceConfiguration)
		{
			this.MaxRange = SourceConfiguration.GetMaxRange();
			this.MinRange = SourceConfiguration.GetMinRange();
			this.MaxSpeed = SourceConfiguration.GetMaxLearningRate();
			this.MinSpeed = SourceConfiguration.GetMinLearningRate();
			this.NumberOfEpochs = SourceConfiguration.GetAmountOfEpochs();
		}

		private void UpdateWeights(int Iteration, Point TargetPoint)
		{
			for (int i = 0; i<Neurons.Count(); i++)
			{
				double[] WeightsOfNeuronToChange = Neurons[i].GetWeightsArray();

				WeightsOfNeuronToChange[0] = WeightsOfNeuronToChange[0] + SpeedFactor(Iteration) * ProximityFunctionType(i) * (TargetPoint.XCoordinate - WeightsOfNeuronToChange[0]);
				WeightsOfNeuronToChange[1] = WeightsOfNeuronToChange[1] + SpeedFactor(Iteration) * ProximityFunctionType(i) * (TargetPoint.YCoordinate - WeightsOfNeuronToChange[1]);
			}
		}

		public override void Train(int Iteration)
		{
			Shuffler.Shuffle<Point>(Points, Randomizer);

			double error = 0.0;

			for (int i = 0; i < Points.Count(); i++)
			{
				Point Target = Points[i];
				this.SetTargetPoint(Target);
				Shuffler.NeuronListSort(Neurons);
				Winner = Neurons[0];
				error += Neurons[0].CalculateDistanceFrom(Points[i]);
				this.UpdateWeights(Iteration, Target);
				Neurons[0].AddPointInNeuronArea(Target);
			}

			foreach (Neuron Neuron in Neurons)
			{
				Neuron.UpdatePositions(Neuron.GetWeight(0), Neuron.GetWeight(1));
			}

			Errors.Add(new Point(Iteration, error / Points.Count()));
			if (Iteration == 0)
			{
				Pointparser Outlet = new Pointparser();
				Output(Outlet, "G:\\OutputsFromGasNetwork", Iteration);
			}
			if (Iteration == 9)
			{
				Pointparser Outlet = new Pointparser();
				Output(Outlet, "G:\\OutputsFromGasNetwork", Iteration);
			}

			if (Iteration == 49)
			{
				Pointparser Outlet = new Pointparser();
				Output(Outlet, "G:\\OutputsFromGasNetwork", Iteration);
			}

			if (Iteration == 100)
			{
				Pointparser Outlet = new Pointparser();
				Output(Outlet, "G:\\OutputsFromGasNetwork", Iteration);
			}

			if (Iteration == 499)
			{
				Pointparser Outlet = new Pointparser();
				Output(Outlet, "G:\\OutputsFromGasNetwork", Iteration);
			}
		}

	


public void DeleteWorstNeurons()
		{
			Neurons.RemoveRange(Neurons.Count() - 11, 10);
			for (int i = 0; i < 10; i++)
				{
					Neurons.Add(NeuronGen.GetNeurons(1, 10.0, 10.0)[0]);
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
	
	public void SetNeurons(List<Neuron> ImportedNeurons)
	{
		this.Neurons = ImportedNeurons;
	}

	public List<Point> GetPoints()
	{
		return Points;
	}

	public List<Point> GetErrorList()
	{
		return Errors;
	}

	public double GetLast()
	{
		return Errors[Errors.Count() - 1].YCoordinate;
	}

	public void SetPoints(List<Point> Points)
	{
		this.Points = Points;
	}

	public void Output(Pointparser Parser, String BasePathfile, int iteration)
		{
		//neurons as points
		List<Point> NeuronsAsPoints = new List<Point>();
		foreach (Neuron Neuron in Neurons)
			{
			NeuronsAsPoints.Add(Neuron.GetAsPoint());
			}
			Parser.OutputFilePath = BasePathfile + "\\Iteration" + iteration.ToString() + "\\NeuronsAfterIteration.txt";
			Parser.ParseOut(NeuronsAsPoints);
		}
		
	}

}
