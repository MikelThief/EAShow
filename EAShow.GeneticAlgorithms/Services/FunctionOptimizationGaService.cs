using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.GeneticAlgorithms.Fitnesses;
using EAShow.Shared.Events;
using EAShow.Shared.Models;
using EAShow.Shared.Models.DTOs;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Infrastructure.Framework.Threading;
using Population = GeneticSharp.Domain.Populations.Population;

namespace EAShow.GeneticAlgorithms.Services
{
    public class FunctionOptimizationGaService
    {
        private float _maxWidth = 2000f;

        private float _maxHeight = 1000f;

        private readonly IEventAggregator _eventAggregator;

        private Dictionary<Guid, GeneticAlgorithm> _geneticAlgorithms;

        private double[] _geneValues = {0, 0, 10, 10};

        public FunctionOptimizationGaService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _geneticAlgorithms = new Dictionary<Guid, GeneticAlgorithm>();
        }

        /// <summary>
        /// Loads presetsProfile into service.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">Thrown when an attempt to load presetsProfile twice is made.</exception>
        public void AddGeneticAlgorithm(GADefinition definition, Guid Key)
        {
            var chromosome = new FloatingPointChromosome(
                minValue: new double[] { 0, 0, 0, 0 },
                maxValue: new double[] { _maxWidth, _maxHeight, _maxWidth, _maxHeight },
                totalBits: new int[] { 20, 20, 20, 20 },
                fractionDigits: new int[] { 0, 0, 0, 0 }, geneValues: _geneValues);

            ICrossover gaCrossover = default;
            switch (definition.Crossover)
            {
                case Crossovers.Uniform:
                    gaCrossover = new UniformCrossover(mixProbability: 0.5f);
                    break;
                case Crossovers.OnePoint:
                    gaCrossover = new OnePointCrossover(swapPointIndex: 40);
                    break;
                case Crossovers.ThreeParent:
                    gaCrossover = new ThreeParentCrossover();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(paramName: nameof(definition.Crossover),
                        actualValue: definition.Crossover, message: "Crossover has wrong value");
            }

            ISelection gaSelection = default;
            switch (definition.Selection)
            {
                case Selections.Elite:
                    gaSelection = new EliteSelection();
                    break;
                case Selections.Roulette:
                    gaSelection = new RouletteWheelSelection();
                    break;
                case Selections.Tournament:
                    gaSelection = new TournamentSelection(
                        size: decimal.ToInt32(d: decimal.Multiply(definition.Population, new decimal(0.2f))));
                    break;
                case Selections.StohasticUniversalSampling:
                    gaSelection = new StochasticUniversalSamplingSelection();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(paramName: nameof(definition.Selection),
                        actualValue: definition.Selection, message: "Selection has wrong value");
            }

            var gaMutation = new UniformMutation(true);


            var gaPopulation = new Population(minSize: definition.Population,
                maxSize: definition.Population, adamChromosome: chromosome);

            var ga = new GeneticAlgorithm(population: gaPopulation,
                fitness: new EuclideanDistanceFitness(), selection: gaSelection,
                crossover: gaCrossover, mutation: gaMutation);
            ga.MutationProbability = (float)definition.Mutation;
            ga.GenerationRan += GeneticAlgorithmOnGenerationRan;
            ga.Termination =
                new FitnessStagnationTermination(expectedStagnantGenerationsNumber: 5);

            _geneticAlgorithms.Add(key: Key, value: ga);
        }

        private async void GeneticAlgorithmOnGenerationRan(object sender, EventArgs e)
        {
            var geneticAlgorithm = sender as GeneticAlgorithm;

            Guid payloadKey = default;

            foreach (var element in _geneticAlgorithms)
            {
                if (element.Value == geneticAlgorithm)
                {
                    payloadKey = element.Key;
                    break;
                }
            }

            // geneticAlgorithm preserves only last 10 iterations
            var chromosomes = geneticAlgorithm.Population
                .Generations[geneticAlgorithm.GenerationsNumber > 10 ? 9 : geneticAlgorithm.GenerationsNumber - 1].Chromosomes;
            var fitnesses = chromosomes.Select(chromosome => geneticAlgorithm.Fitness.Evaluate(chromosome))
                .OrderByDescending(d => d).ToList();

            await _eventAggregator.PublishOnBackgroundThreadAsync(message: new GAGenerationCompletedEvent(
                dto: new FOGenerationCompletedDto(bestFitness: fitnesses.First(),
                    averageFitness: fitnesses.Average(), worstFitness: fitnesses.Last(),
                    generation: geneticAlgorithm.GenerationsNumber), sender: payloadKey));
        }

        public void Start()
        {
            if(_geneticAlgorithms.Count < 1)
            {
                throw new InvalidOperationException(message: "There are no GAs to run.");
            }

            foreach (var geneticAlgorithm in _geneticAlgorithms)
            {
                geneticAlgorithm.Value.Start();
            }
        }
    }
}
