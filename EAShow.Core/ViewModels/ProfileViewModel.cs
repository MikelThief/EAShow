using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.GeneticAlgorithms.Services;
using EAShow.Shared.DataStructures;
using EAShow.Shared.Events;
using EAShow.Shared.Models;
using EAShow.Shared.Models.ValueObjects;
using Microsoft.Toolkit.Uwp.UI;

namespace EAShow.Core.ViewModels
{
    public class ProfileViewModel : PropertyChangedBase, IHandle<GAGenerationCompletedEvent>
    {
        private Profile _profile;
        private string _name;

        private IEventAggregator _eventAggregator;

        private ConcurrentHashSet<Guid> SeenSenderIds;

        public BindableCollection<AdvancedCollectionView> FitnessFilterCollection { get; }

        public BindableCollection<FitnessDataPoint> FitnessDataPoints { get; }

        private readonly Guid _invokerId;

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
            FitnessFilterCollection = new BindableCollection<AdvancedCollectionView>();
            FitnessDataPoints = new BindableCollection<FitnessDataPoint>();
            SeenSenderIds = new ConcurrentHashSet<Guid>();
            _invokerId = Guid.NewGuid();
            _eventAggregator.SubscribeOnUIThread(subscriber: this);
        }

        public void InjectProfile(Profile profile)
        {
            _profile = profile;
            Name = _profile.Name;
            _gaService.InjectProfile(profile: profile);
            _gaService.InvokerId = _invokerId;
            _gaService.Start();
        }

        public Task HandleAsync(GAGenerationCompletedEvent message, CancellationToken cancellationToken)
        {
            if (message.Invoker != _invokerId)
            {
                // message was meant to be handled by different VM
                return Task.CompletedTask;
            }

            if (!SeenSenderIds.Contains(item: message.Sender))
            {
                SeenSenderIds.Add(item: message.Sender);
                var filter =
                    new Predicate<object>(item => ((FitnessDataPoint) item).SenderId == message.Sender);
                var fitnessFilteringCollection =
                    new AdvancedCollectionView(source: FitnessDataPoints, isLiveShaping: true);

                fitnessFilteringCollection.SortDescriptions.Add(new SortDescription(direction: SortDirection.Ascending,
                    propertyName: nameof(message.Dto.Generation)));
                fitnessFilteringCollection.Filter = filter;
                FitnessFilterCollection.Add(item: fitnessFilteringCollection);
            }

            var fitnessDataPoint = new FitnessDataPoint(bestFitness: message.Dto.BestFitness,
                worstFitness: message.Dto.WorstFitness, averageFitness: message.Dto.AverageFitness,
                senderId: message.Sender, generation: message.Dto.Generation);

            FitnessDataPoints.Add(item: fitnessDataPoint);

            return Task.CompletedTask;
        }
    }
}
