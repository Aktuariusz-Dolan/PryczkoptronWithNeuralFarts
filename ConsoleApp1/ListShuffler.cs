﻿using System;
using System.Collections.Generic;

namespace MyNotSoLittlePryczkoptron
{
    public class ListShuffler
    {
        public void Shuffle<T>(List<T> SourceList, Random Randomizer)
        {
            int ListSize = SourceList.Count;
            while (ListSize > 1)
            {
                ListSize--;
                int k = Randomizer.Next(ListSize + 1);
                T value = SourceList[k];
                SourceList[k] = SourceList[ListSize];
                SourceList[ListSize] = value;
            }
        }

		public void NeuronListSort(List<Neuron> Neurons)
		{
			Neuron Comparer = new Neuron(new double[] { 1.0, 2.0 });
			Neurons.Sort();
		}
    }
}
