using System;
using System.Collections.Generic;
using System.Text;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;

namespace EAShow.GeneticAlgorithms.Fitnesses
{
    public class EuclideanDistanceFitness : IFitness
    {
        public double Evaluate(IChromosome chromosome)
        {
            var fpChromosome = chromosome as FloatingPointChromosome;

            var intChromosome = chromosome as IntegerChromosome;

            var values = fpChromosome.ToFloatingPoints();

            var x1 = values[0];
            var y1 = values[1];
            var x2 = values[2];
            var y2 = values[3];

            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}
