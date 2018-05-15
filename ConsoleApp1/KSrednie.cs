using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotSoLittlePryczkoptron
{
	public class KSrednie
	{
		private	int ClusterCount;
		private Dictionary<int, List<Point>> Clusters;
		private List<Point> Points;
		private List<Neuron> Neurons; // aka centroidy
		private NeuronGenerator NeurGen;
		private double Xrange;
		private double Yrange;
		public List<double> Error;
		private int MaxIters;

		public KSrednie(int ClusterCount , double Xrange, double Yrange, List<Point> Points, int Maxiters)
		{
			this.ClusterCount = ClusterCount;
			this.NeurGen = new NeuronGenerator();
			this.Xrange = Xrange;
			this.Yrange = Yrange;
			this.Points = Points;
			this.Clusters = new Dictionary<int, List<Point>>();
			for (int i =0; i < ClusterCount; i++)
			{
				Clusters.Add(i, new List<Point>());
			}
			this.Error = new List<Double>();
			this.MaxIters = Maxiters;
		}

		public List<Point> Clusterize()
		{
			Neurons = NeurGen.GetNeurons(ClusterCount, Xrange, Yrange);
			foreach(Point Point in Points) {
				double ClosestDistance = double.MaxValue;
				int BestSoFar = -1;
				for (int i = 0; i < ClusterCount; i++)
				{
					if(Neurons[i].CalculateDistanceFrom(Point) < ClosestDistance)
					{
						BestSoFar = i;
					}
				}
				Clusters[BestSoFar].Add(Point);
			};
			Error.Add(0.0);
			for (int Iteration = 0; Iteration < MaxIters; Iteration++) {
				Error.Add(0.0);

				for(int i=0; i < ClusterCount; i++){ {
					Double ErrorComponent = 0.0;
					Point Mean = CalculateMean(Clusters[i]);
					ErrorComponent = Neurons[i].CalculateDistanceFrom(Mean);
					Error[Iteration] += ErrorComponent;
					Neurons[i].UpdatePositions(Mean.XCoordinate, Mean.YCoordinate);
					}
				};

				//reassign clusters;
				for(int i = 0; i < ClusterCount; i++)
				{
					Clusters[i] = new List<Point>();
				}

				foreach(Point Point in Points) {
					double ClosestDistance = double.MaxValue;
					int BestSoFar = -1;
					for (int i = 0; i < ClusterCount; i++)
					{

						if (Neurons[i].CalculateDistanceFrom(Point) < ClosestDistance)
						{
							BestSoFar = i;
						}
					}
					Clusters[BestSoFar].Add(Point);
				};
			}
			List<Point> Centroids = new List<Point>();
			foreach(Neuron Neuron in Neurons)
			{
				Centroids.Add(Neuron.GetAsPoint());
			}
			return Centroids;
		}

		Point CalculateMean(List<Point> ClusterPoints)
		{
			double X, Y;
			X = 0.0;
			Y = 0.0;
			if (ClusterPoints.Count != 0)
			{
				for (int i = 0; i < ClusterPoints.Count(); i++)
				{
					X = X + ClusterPoints[i].XCoordinate;
					Y = Y + ClusterPoints[i].YCoordinate;
				}
				X /= ClusterPoints.Count();
				Y /= ClusterPoints.Count();
			}
			return new Point(X, Y);
		}
	}
}
