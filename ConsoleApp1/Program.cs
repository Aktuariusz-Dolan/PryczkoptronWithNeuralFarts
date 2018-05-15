using System;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;

namespace MyNotSoLittlePryczkoptron
{
    class Program
    {
        static void Main(string[] args)
        {
			bool KohonenSwitch = false;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            NeuronGenerator NeuralGenerator = new NeuronGenerator();
            Configuration Configuration = new Configuration();
            Pointparser Parser = new Pointparser();
            Parser.Filepath = "zestaw1.txt";
			double InitialXRange = 2.0;
			double InitialYRange = 2.0; //describes initial square (centered at 0,0) from which the neurons are generated
			List<Neuron> Neurons = NeuralGenerator.GetNeurons(Configuration.GetAmountOfNeurons(), InitialXRange, InitialYRange);
            List<Point> TrainingPointsList = Parser.Parse();
            KohonenLearning NeuralNetworkKohonenStyle = new KohonenLearning(Neurons, TrainingPointsList, Configuration);
			NeuralGas NeuralNetworkGasStyle = new NeuralGas(Neurons, TrainingPointsList, Configuration, NeuralGenerator);
			if (KohonenSwitch == true)
			{
				for (int i = 0; i < 1000; i++)
				{
					if (i % 50 == 0) { Console.WriteLine("Progress o 10 %"); }
					NeuralNetworkKohonenStyle.Train(i);
				}
				Parser.OutputFilePath = "G:\\Outcome.txt";
				List<Point> points = NeuralNetworkKohonenStyle.ReturnNeuronsAsPoints();
				Parser.ParseOut(points);
				VoronoiDiagram.CreateImage(1366, 768, points, 0.9).Save("Voronoi.png");
			}
			else
			{
				for (int i = 0; i < 500; i++)
				{
					if (i % 50 == 0) { Console.WriteLine("Progress o 10 %"); }
					NeuralNetworkGasStyle.Train(i);
				}
				Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
				Console.ReadKey();
			}
		}
    }
}
