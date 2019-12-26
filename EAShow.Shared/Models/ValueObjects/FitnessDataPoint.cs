using System;
using System.Collections.Generic;
using System.Text;
using CSharpFunctionalExtensions;

namespace EAShow.Shared.Models.ValueObjects
{
    public class FitnessDataPoint : ValueObject
    {
        public FitnessDataPoint(double bestFitness, double worstFitness, double averageFitness, Guid senderId, int generation)
        {
            BestFitness = bestFitness;
            WorstFitness = worstFitness;
            AverageFitness = averageFitness;
            SenderId = senderId;
            Generation = generation;
        }

        public double BestFitness { get; }

        public double WorstFitness { get; }

        public double AverageFitness { get; }

        public Guid SenderId { get; }

        public int Generation { get;  }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return BestFitness;
            yield return SenderId;
            yield return Generation;
            yield return WorstFitness;
            yield return AverageFitness;
        }
    }
}
