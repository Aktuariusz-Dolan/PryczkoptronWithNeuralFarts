using System;
using System.Collections.Generic;
using System.Linq;

namespace MyNotSoLittlePryczkoptron
{
    public class NeuronGenerator
    {
        public List<Neuron> GetNeurons(int NumberOfNeurons, double XRange, double YRange)
        {
            List<Neuron> Neurons = new List<Neuron>();
            Random Generator = new Random();
            for (int i = 0; i < NumberOfNeurons; i++)
            {
                double[ ] InitialWeights = {XRange * (Generator.NextDouble() - 0.5), YRange * (Generator.NextDouble() - 0.5) };
                Neurons.Add(new Neuron(InitialWeights) );
                Neurons.Last().SetFirstPosition(Neurons.Last().GetWeight(0), Neurons.Last().GetWeight(1));
            }
            return Neurons;
        }
    }
}
