using System;
using System.Collections.Generic;
using System.Text;
using Caliburn.Micro;
using EAShow.Shared.Models;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;

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
            if (!IsReady)
            {
                _profile = profile;
                _geneticAlgorithms = new Dictionary<Guid, GeneticAlgorithm>(
                    capacity: _profile.Crossovers.Count *
                              _profile.Mutations.Count *
                              _profile.Selections.Count *
                              _profile.Populations.Count);


                foreach (var crossover in _profile.Crossovers)
                {
                    foreach (var mutation in _profile.Mutations)
                    {
                        foreach (var population in _profile.Populations)
                        {
                            foreach (var selection in _profile.Selections)
                            {

                                var key = new Guid();
                                // fill dictionary
                            }
                        }
                    }
                }

                foreach (var geneticAlgorithm in _geneticAlgorithms.Values)
                {
                    geneticAlgorithm.GenerationRan += GeneticAlgorithmOnGenerationRan;
                }
            }
            else throw new InvalidOperationException(message: "Cannot load profile more than once. Use EjectProfile() to eject currently loaded profile.");
        }

        private void GeneticAlgorithmOnGenerationRan(object sender, EventArgs e)
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

            // eventaggregator broadcasts data packet


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
