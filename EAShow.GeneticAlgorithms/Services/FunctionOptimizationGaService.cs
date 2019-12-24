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
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Infrastructure.Framework.Threading;
using Population = GeneticSharp.Domain.Populations.Population;

namespace EAShow.GeneticAlgorithms.Services
{
    public class FunctionOptimizationGaService
    {
        private Profile _profile;

        private readonly IEventAggregator _eventAggregator;

        private Dictionary<Guid, GeneticAlgorithm> _geneticAlgorithms;

        private bool IsReady => _profile != null;

        public FunctionOptimizationGaService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Loads profile into service.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">Thrown when an attempt to load profile twice is made.</exception>
        public void InjectProfile(Profile profile)
        {
            var capacity = profile.Crossovers.Count *
                           _profile.Mutations.Count *
                           _profile.Selections.Count *
                           _profile.Populations.Count;

            if (!IsReady)
            {
                _profile = profile;
                _geneticAlgorithms = new Dictionary<Guid, GeneticAlgorithm>(
                    capacity: capacity);

                var taskExecutor = new LinearTaskExecutor();

                ICrossover gaCrossover = default;
                ISelection gaSelection = default;
                var gaMutation = new FlipBitMutation();



                foreach (var crossover in _profile.Crossovers)
                {
                    switch (crossover.Value)
                    {
                        case Crossovers.Uniform:
                            gaCrossover = new UniformCrossover(mixProbability: 0.5f);
                            break;
                        case Crossovers.OnePoint:
                            gaCrossover = new OnePointCrossover(swapPointIndex: 20);
                            break;
                        case Crossovers.ThreeParent:
                            gaCrossover = new ThreeParentCrossover();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(paramName: nameof(profile.Crossovers),
                                actualValue: crossover, message: "Crossover has wrong value");
                    }

                    foreach (var population in _profile.Populations)
                    {
                        foreach (var selection in _profile.Selections)
                        {
                            switch (selection.Value)
                            {
                                case Selections.Elite:
                                    gaSelection = new EliteSelection();
                                    break;
                                case Selections.Roulette:
                                    gaSelection = new RouletteWheelSelection();
                                    break;
                                case Selections.Tournament:
                                    gaSelection = new TournamentSelection(size: decimal.ToInt32(d: decimal.Multiply(population.Value, new decimal(0.2f))));
                                    break;
                                case Selections.StohasticUniversalSampling:
                                    gaSelection = new StochasticUniversalSamplingSelection();
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(paramName: nameof(profile.Selections),
                                        actualValue: selection, message: "Selection has wrong value");
                            }

                            var gaPopulation = new Population(minSize: (int) population.Value,
                                maxSize: (int) population.Value, null);

                            foreach (var mutation in _profile.Mutations)
                            {
                                for (int counter = 0; counter < capacity; counter++)
                                {
                                    var ga = new GeneticAlgorithm(population: gaPopulation, fitness: new EuclideanDistanceFitness(), selection: gaSelection, crossover: gaCrossover, mutation: gaMutation);
                                    ga.MutationProbability = (float) mutation.Value;
                                    ga.GenerationRan += GeneticAlgorithmOnGenerationRan;
                                    ga.TaskExecutor = taskExecutor;
                                }
                            }

                        }
                    }
                }

                Parallel.ForEach(source: _geneticAlgorithms, body: (pair, _) => pair.Value.Start(),
                    parallelOptions: new ParallelOptions()
                    {
                        MaxDegreeOfParallelism = Environment.ProcessorCount
                    });
            }
            else throw new InvalidOperationException(
                    message:
                    "Cannot load profile more than once. Use EjectProfile() to eject currently loaded profile.");
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

                var chromosomes = geneticAlgorithm.Population.Generations[geneticAlgorithm.GenerationsNumber]
                    .Chromosomes;

                var fitnesses = chromosomes.Select(chromosome => geneticAlgorithm.Fitness.Evaluate(chromosome))
                    .OrderByDescending(d => d).ToList();

                await _eventAggregator.PublishOnBackgroundThreadAsync(message: new GAGenerationCompletedEvent(
                    dto: new FOGenerationCompletedDto(bestFitness: fitnesses.First(),
                        averageFitness: fitnesses.Average(), worstFitness: fitnesses.Last()), sender: payloadKey));
            }

            public void EjectProfile()
            {
                if (IsReady)
                {
                    _profile = null;
                    _geneticAlgorithms.Clear();
                    _geneticAlgorithms = null;
                }
            }




        }
    }
