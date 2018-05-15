using System;

namespace MyNotSoLittlePryczkoptron
{
    public enum Metric { Euclidean, Taxi, Maximum };
    public enum Proximity { Gauss, Rekt }

    public class ProximityFunction
    { 
        private Metric MetricChoice;
        private Proximity ProximityChoice;
        public double LambdaCoefficient = 1.0;

        public ProximityFunction(Metric MetricChoice, Proximity ProximityChoice)
        {
            this.MetricChoice = MetricChoice;
            this.ProximityChoice = ProximityChoice;
        }
        
        private double CalculateMetric(Neuron FirstNeuron, Neuron OtherNeuron)
        {
            if (MetricChoice == Metric.Taxi) return Math.Abs(FirstNeuron.GetAsPoint().XCoordinate - OtherNeuron.GetAsPoint().XCoordinate) + Math.Abs(FirstNeuron.GetAsPoint().YCoordinate - OtherNeuron.GetAsPoint().YCoordinate);
            if (MetricChoice == Metric.Euclidean) return Math.Sqrt( Math.Pow((FirstNeuron.GetAsPoint().XCoordinate - OtherNeuron.GetAsPoint().XCoordinate),2) + Math.Pow((FirstNeuron.GetAsPoint().YCoordinate - OtherNeuron.GetAsPoint().YCoordinate),2));
            if (MetricChoice == Metric.Maximum) return Math.Max(Math.Abs(FirstNeuron.GetAsPoint().XCoordinate - OtherNeuron.GetAsPoint().XCoordinate), Math.Abs(FirstNeuron.GetAsPoint().YCoordinate - OtherNeuron.GetAsPoint().YCoordinate));
            else
                return 0.0;
        }

        public double CalculateProximity(Neuron FirstNeuron, Neuron OtherNeuron)
        {
            double d = CalculateMetric(FirstNeuron, OtherNeuron);
            if(ProximityChoice == Proximity.Gauss) return Math.Exp((-Math.Pow(d, 2)) / (2 * Math.Pow(LambdaCoefficient, 2)));
            if(ProximityChoice == Proximity.Rekt) return  (d > LambdaCoefficient) ? 0 : 1;
            else
            return 0.0;
        }

		public double CalculateGasProximity(int z)
		{
			if (ProximityChoice == Proximity.Gauss) return Math.Exp((- z/ (2 * Math.Pow(LambdaCoefficient, 2))));
			else
				return 0.0;
		}
    }
}
