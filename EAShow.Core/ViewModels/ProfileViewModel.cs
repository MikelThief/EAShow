using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Core.POCO;
using EAShow.GeneticAlgorithms.Services;
using EAShow.Infrastructure.DataStructures;
using EAShow.Shared.DataStructures;
using EAShow.Shared.Events;
using EAShow.Shared.Models;
using EAShow.Shared.Models.DTOs;
using EAShow.Shared.Models.ValueObjects;
using Microsoft.Toolkit.Uwp.UI;

namespace EAShow.Core.ViewModels
{
    public class ProfileViewModel : PropertyChangedBase, IHandle<GAGenerationCompletedEvent>
    {
        private PresetsProfile _presetsProfile;
        private string _name;

        private IEventAggregator _eventAggregator;

        private List<Guid> GaKeys;

        public BindableCollection<ChartData> Charts { get; }

        public SynchronizedObservableCollection<FitnessDataPoint> FitnessDataPoints { get; }

        public string Name
        {
            get => _name;
            set => Set(oldValue: ref _name, newValue: value, propertyName: nameof(Name));
        }

        private readonly FunctionOptimizationGaService _gaService;

        public ProfileViewModel(FunctionOptimizationGaService gaService, IEventAggregator eventAggregator)
        {
            _gaService = gaService;
            _eventAggregator = eventAggregator;
            Charts = new BindableCollection<ChartData>();
            FitnessDataPoints = new SynchronizedObservableCollection<FitnessDataPoint>();
            GaKeys = new List<Guid>();
            _eventAggregator.SubscribeOnPublishedThread(subscriber: this);
        }

        public void InjectProfile(PresetsProfile profile)
        {
            _presetsProfile = profile;
            CreateCharts(profile: profile);
            Name = _presetsProfile.Name;
            _gaService.Start();
        }

        public Task HandleAsync(GAGenerationCompletedEvent message, CancellationToken cancellationToken)
        {
            var fitnessDataPoint = new FitnessDataPoint(bestFitness: message.Dto.BestFitness,
                worstFitness: message.Dto.WorstFitness, averageFitness: message.Dto.AverageFitness,
                senderId: message.Sender, generation: message.Dto.Generation);
            FitnessDataPoints.Add(item: fitnessDataPoint);
            return Task.CompletedTask;
        }

        private void CreateCharts(PresetsProfile profile)
        {
            foreach (var selection in profile.Selections)
            {
                foreach (var crossover in profile.Crossovers)
                {
                    foreach (var mutation in profile.Mutations)
                    {
                        foreach (var population in profile.Populations)
                        {
                            var gaKey = Guid.NewGuid();

                            var gaDefinition = new GADefinition(
                                mutation: mutation.Value,
                                population: population.Value,
                                crossover: crossover.Value,
                                selection: selection.Value);

                            GaKeys.Add(item: gaKey);

                            var filter =
                                new Predicate<object>(item => ((FitnessDataPoint)item).SenderId == gaKey);
                            var filteringCollection =
                                new AdvancedCollectionView(source: FitnessDataPoints, isLiveShaping: true);
                            filteringCollection.SortDescriptions.Add(item: new SortDescription(direction: SortDirection.Ascending,
                                propertyName: "Generation"));
                            filteringCollection.Filter = filter;

                            _gaService.AddGeneticAlgorithm(definition: gaDefinition, gaKey);
                            Charts.Add(item: new ChartData(gaDefinition: gaDefinition, filteringCollection, key: gaKey));
                        }
                    }
                }
            }
        }
    }
}
