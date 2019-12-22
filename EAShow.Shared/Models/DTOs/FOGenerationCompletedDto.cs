using System;
using System.Collections.Generic;
using System.Text;
using CSharpFunctionalExtensions;

namespace EAShow.Shared.Models
{
    public class FOGenerationCompletedDto
    {
        public FOGenerationCompletedDto(double bestFitness, double averageFitness, double worstFitness)
        {
            BestFitness = bestFitness;
            AverageFitness = averageFitness;
            WorstFitness = worstFitness;
        }

        public double BestFitness { get; }

        public double AverageFitness { get; }

        public double WorstFitness { get; }
    }
}
