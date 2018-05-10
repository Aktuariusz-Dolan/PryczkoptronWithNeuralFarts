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
               for( int i = 0; i < 1000; i++)
				{
					if (i%50 == 0) { Console.WriteLine("ZUPA"); }
					NeuralNetworkKohonenStyle.Train(i);
				}
            Parser.OutputFilePath = "G:\\Outcome.txt";
            Parser.ParseOut(NeuralNetworkKohonenStyle.ReturnNeuronsAsPoints());
            Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
            Console.ReadKey();
        }
    }
}
