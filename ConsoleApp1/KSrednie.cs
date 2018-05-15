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
		private Dictionary<int, List<Point>> PreviousClusters;
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
			Parallel.ForEach(Points, Point => {
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
			});

			for (int Iteration = 0; Iteration < MaxIters; Iteration++) {
				Error.Add(0.0);
				Parallel.ForEach(Clusters.Keys, i => {
					Double ErrorComponent = 0.0;
					Point Mean = CalculateMean(Clusters[i]);
					ErrorComponent = Neurons[i].CalculateDistanceFrom(Mean);
					Error[Iteration] += ErrorComponent;
					Neurons[i].UpdatePositions(Mean.XCoordinate, Mean.YCoordinate);
				});

				//reassign clusters;
				foreach(int i in Clusters.Keys)
				{
					Clusters[i].Clear();
				}

				Parallel.ForEach(Points, Point => {
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
				});
			}
			List<Point> Centroids = new List<Point>();
			foreach(Neuron Neuron in Neurons)
			{
				Centroids.Add(Neuron.GetAsPoint());
			}
			return Centroids;
		}

		Point CalculateMean(List<Point> Points)
		{
			double X=0, Y=0;
			foreach(Point Point in Points)
			{
				X = X + Point.XCoordinate;
				Y = Y + Point.YCoordinate;
			}
			X /= Points.Count();
			Y /= Points.Count();
			return new Point(X, Y);
		}
	}
}
