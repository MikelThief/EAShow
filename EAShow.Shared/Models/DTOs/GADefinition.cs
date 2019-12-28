using System;
using System.Collections.Generic;
using System.Text;

namespace EAShow.Shared.Models.DTOs
{
    public class GADefinition
    {
        public GADefinition(decimal mutation, int population, Crossovers crossover, Selections selection)
        {
            Mutation = mutation;
            Population = population;
            Crossover = crossover;
            Selection = selection;
        }

        public decimal Mutation { get;}

        public int Population { get; }

        public Crossovers Crossover { get; }

        public Selections Selection { get; }
    }
}
