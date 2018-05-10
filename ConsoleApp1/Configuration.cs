using System;

namespace MyNotSoLittlePryczkoptron
{
    public class Configuration
    {   private int NeuronsAmount = 50;
        private int AmountOfEpochs = 1000;

        private double MaxRange = 2.5;
        private double MinRange = 0.01;

        private double MaxLearningRate = 0.2;
        private double MinLearningRate = 0.001;

        private double MinPotential = 0.75;

        private Func<Neuron, Neuron, double> ProximityFunctionType =(Neuron A, Neuron B) => (new ProximityFunction(Metric.Taxi, Proximity.Rekt )).CalculateProximity(A,B);

        private String ErrorPathFile = "error.jpg";
        private String ReportPathFile = "report.txt";
        private String GraphDescription = "Graph";

        public int GetAmountOfNeurons()
        {
            return NeuronsAmount;
        }

        public double GetMaxRange()
        {
            return MaxRange;
        }

        public double GetMinRange()
        {
            return MinRange;
        }

        public double GetMaxLearningRate()
        {
            return MaxLearningRate;
        }

        public double GetMinLearningRate()
        {
            return MinLearningRate;
        }

        public String GetErrorPathFile()
        {
            return ErrorPathFile;
        }

        public String GetReportPathFile()
        {
            return ReportPathFile;
        }

        public String GetGraphDescription()
        {
            return GraphDescription;
        }

        public int GetAmountOfEpochs()
        {
            return AmountOfEpochs;
        }

        public double GetMinPotential()
        {
            return MinPotential;
        }

        public  Func<Neuron, Neuron, double>  GetFunction()
        {
            return ProximityFunctionType;
        }

    }


}
