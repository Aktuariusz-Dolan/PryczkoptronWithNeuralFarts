using System;
using System.Collections.Generic;

namespace MyNotSoLittlePryczkoptron
{
    public class Neuron : IComparable<Neuron>
    {
        private double Potential = 1;
        private List<Point> AreaOfInfluence;
        private double[] Weights;
        private Point TargetPoint = null;
        private double StartingX;
        private double StartingY;
        private List<Point> Positions;

        public Neuron(double[] weights)
        {

            this.Weights = weights;
            AreaOfInfluence = new List<Point>();
            Positions = new List<Point>();
        }
        public void UpdatePositions(double x, double y)
        {
            Positions.Add(new Point( x, y));
        }
        public List<Point> GetPositions()
        {
            return Positions;
        }
        public void AddPointInNeuronArea(Point point)
        {
            AreaOfInfluence.Add(point);
        }
        public void SetFirstPosition(double w1, double w2)
        {
            this.StartingX = w1;
            this.StartingY = w2;
        }

        public double GetWeight(int index)
        {
            return Weights[index];
        }

        public double[] GetWeightsArray()
        {
            return this.Weights;
        }

        public bool IsDead()
        {
            if (this.StartingX == GetWeight(0) && this.StartingY == GetWeight(1))
            {
                return true;
            }
            
            else if (Math.Abs(this.StartingX - GetWeight(0)) < 0.01 && Math.Abs(this.StartingY - GetWeight(1)) < 0.01)
            {
                return true;
            }
            else
                return false;
        }

        public void Learn(double SpeedFactor, double NeighbourshipFunctionValue, double[] InputArray)
        {
            for (int i = 0; i < this.Weights.Length; i++)
            {
                this.Weights[i] = this.Weights[i] + SpeedFactor * NeighbourshipFunctionValue * (InputArray[i] - this.Weights[i]);
            }
        }


        public Point GetAsPoint()
        {
            if (Weights.Length != 2)
            {
                throw new Exception("Fatal Error !");
            }

            return new Point(Weights[0], Weights[1]);
        }

        public double CalculateDistanceFrom(Point TargetPoint)
        {
            if (Weights.Length != 2)
            {
                throw new Exception("Fatal Error !");
            }
            
            return Math.Sqrt(Math.Pow(TargetPoint.XCoordinate - Weights[0], 2) + Math.Pow(TargetPoint.YCoordinate - Weights[1], 2));
        }

        public double GetDistanceFromTarget()
        {
            if (Weights.Length != 2)
            {
                throw new Exception("Fatal Error !");
            }

            return Math.Sqrt(Math.Pow(TargetPoint.XCoordinate - Weights[0], 2) + Math.Pow(TargetPoint.YCoordinate - Weights[1], 2));
        }

        public Point GetTargetPoint()
        {
            return TargetPoint;
        }

        public void SetTargetPoint(Point TargetPoint)
        {
            this.TargetPoint = TargetPoint;
        }

        public void UpdatePotential(double CurrentPotential)
        {
            this.Potential = CurrentPotential;
        }

        public double GetPotential()
        {
            return Potential;
        }
		public bool CompareNeurons(Neuron o1, Neuron o2)
		{
			double d1 = o1.CalculateDistanceFrom(o1.GetTargetPoint());
			double d2 = o2.CalculateDistanceFrom(o2.GetTargetPoint());
			return d1 <= d2;
		}

		public int CompareTo(Neuron Neur2)
		{
			double d1 = this.CalculateDistanceFrom(this.GetTargetPoint());
			double d2 = Neur2.CalculateDistanceFrom(Neur2.GetTargetPoint());
			return d1.CompareTo(Neur2);
		}
	}
}

