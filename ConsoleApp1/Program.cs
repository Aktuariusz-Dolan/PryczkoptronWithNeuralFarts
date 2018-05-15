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
			bool Kmeans = false;
			bool GasSwitch = false;
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
			if (KohonenSwitch)
			{
				for (int i = 0; i < 100; i++)
				{
					if (i % 10 == 0) { Console.WriteLine("Progress o 10 %"); }
					NeuralNetworkKohonenStyle.Train(i);
					List<Point> points = NeuralNetworkKohonenStyle.ReturnNeuronsAsPoints();
					VoronoiDiagram.CreateImage(1366, 768, points, 0.9).Save("Voronoi" + i.ToString("D3") + ".png");
				}
			}
			else if(GasSwitch)
			{
				for (int i = 0; i < 500; i++)
				{
					if (i % 50 == 0) { Console.WriteLine("Progress o 10 %"); }
					NeuralNetworkGasStyle.Train(i);
				}
				Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
				Console.ReadKey();
			}
			else if (Kmeans)
			{
				KSrednie KMeans = new KSrednie(40, 30, 30, TrainingPointsList, 150);
				Parser.ParseOut(KMeans.Clusterize());
			}
		}
    }
}
