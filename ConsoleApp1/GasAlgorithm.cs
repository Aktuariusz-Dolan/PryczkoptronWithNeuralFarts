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

	private int NumberOfEpochs;

	public NeuralGas(List<Neuron> ListOfNeurons, List<Point> ListOfPoints, Configuration Configuration)
    {
        this.Neurons = ListOfNeurons;
        this.Points = ListOfPoints;
        this.SetParams(Configuration);
        this.Errors = new List<Point>();
    }

	public void SetParams(Configuration SourceConfiguration)
		{
			this.MaxRange = SourceConfiguration.GetMaxRange();
			this.MinRange = SourceConfiguration.GetMinRange();
			this.MaxSpeed = SourceConfiguration.GetMaxLearningRate();
			this.MinSpeed = SourceConfiguration.GetMinLearningRate();
			this.NumberOfEpochs = SourceConfiguration.GetAmountOfEpochs();
		}

		public void Learn(int iteration) { 
			Shuffler.Shuffle<Point>(Points, Randomizer);

		double error = 0.0;

		for (int i = 0; i < Points.Count(); i++)
		{
			//setting target
			Point Target = Points[i];
			this.SetTargetPoint(Target);
			//sorting by distance
			(neurons, distanceComparator);
			error += neurons.get(0).getDistance(points.get(i));
			//learning
			this.changeWeights(iteration, Target);
			neurons.get(0).AddPointInNeuronArea(Target);
		}

		for (Neuron Neuron in Neurons)
		{
			neuron.setPositions(neuron.getWeight(0), neuron.getWeight(1));
		}

		errors.add(new Point(iteration, error / points.size()));

	}

	private void changeWeights(int interation, Point targetPoint)
	{
		for (int i = 0; i < neurons.size(); i++)
		{
			double[] w = neurons.get(i).getWeights();
			w[0] = w[0] + calcSpeedFactor(interation) * proximityFunc(i, interation) * (targetPoint.x - w[0]);
			w[1] = w[1] + calcSpeedFactor(interation) * proximityFunc(i, interation) * (targetPoint.y - w[1]);
		}
	}

	private void SetTargetPoint(Point targetPoint)
	{
		for (Neuron neuron : neurons)
		{
			neuron.setTargetPoint(targetPoint);
		}
	}

	private double proximityFunc(int neuronIndex, int iteration)
	{
		double lambda = maxRange * Math.pow(minRange / maxRange, iteration / (double)NumberOfEpochs);
		return Math.exp(-neuronIndex / lambda);
	}

	private double calcSpeedFactor(int iteration)
	{
		return maxSpeed * Math.pow(minSpeed / maxSpeed, iteration / (double)NumberOfEpochs);
	}

	public void deleteTheFarestNeurons()
	{
		neurons.subList(neurons.size() - 10, neurons.size()).clear();
		for (int i = 0; i < 10; i++)
		{
			neurons.add(NeuronsGenerator.getNeuron(new Square(new Point(-1.2, -1.2), new Point(1.2, -1.2), new Point(1.2, 1.2), new Point(-1.2, 1.2))));
		}
	}

	public List<Neuron> getNeurons()
	{
		return neurons;
	}

	public void setNeurons(List<Neuron> neurons)
	{
		this.neurons = neurons;
	}

	public List<Point> getPoints()
	{
		return points;
	}

	public List<Point> getError()
	{
		return errors;
	}

	public double getLast()
	{
		return errors.get(errors.size() - 1).getY();
	}

	public void setPoints(List<Point> points)
	{
		this.points = points;
	}

	public void save(String fileName)
	{
		//neurons as points
		ArrayList<Point> neuronsAsPoints = new ArrayList<>();
		for (Neuron neuron : neurons)
		{
			neuronsAsPoints.add(neuron.getAsPoint());
		}

		Plot plot = new Plot();
		XYDataset dataset1 = plot.createDataset(points, "points");
		XYDataset dataset2 = plot.createDataset(neuronsAsPoints, "neurons");
		XYLineAndShapeRenderer rendere = new XYLineAndShapeRenderer(false, true);
		XYLineAndShapeRenderer rendere2 = new XYLineAndShapeRenderer(false, true);
		rendere.setBaseShape(new Rectangle(4, 4));
		rendere2.setBaseShape(new Rectangle(1, 1));
		JFreeChart chart = plot.createGraph("Neural Gas", dataset2, rendere);
		plot.addDataset(chart, dataset1, rendere2);
		plot.saveGraphToJPG(chart, fileName);
	}

	public void saveErrorGraph(String fileName)
	{
		Plot plot = new Plot();
		XYDataset datasetError = plot.createDataset(errors, "Level of organization of map");
		JFreeChart chart = plot.createGraph("Level of organization of map", datasetError, new XYLineAndShapeRenderer(true, false));
		plot.saveGraphToJPG(chart, fileName);
	}
}

}
