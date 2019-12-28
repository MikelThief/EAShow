using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EAShow.Shared.Models.DTOs;
using LiteDB;

namespace EAShow.Shared.Models
{
    [DebuggerDisplay(value: "Mutations={Mutations.Count}, Crossovers={Crossovers.Count}, Selections={Selections.Count}, Populations={Populations.Count}",
        Name = "PresetsProfile {Name}")]
    public class PresetsProfile
    {
        public PresetsProfile(ObjectId id, string name, List<Mutation> mutations, List<Crossover> crossovers, List<Selection> selections, List<Population> populations)
        {
            Id = id;
            Name = name;
            Mutations = mutations;
            Crossovers = crossovers;
            Selections = selections;
            Populations = populations;
        }

        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public List<Mutation> Mutations { get; set; }

        public List<Crossover> Crossovers { get; set; }

        public List<Selection> Selections { get; set; }

        public List<Population> Populations { get; set; }

        public static PresetsProfile From(ProfileDbDto dto)
        {
            var mutations = new List<Mutation>(capacity: dto.Mutations.Capacity);
            mutations.AddRange(collection: dto.Mutations.Select(x => new Mutation(value: x)));

            var populations = new List<Population>(capacity: dto.Populations.Capacity);
            populations.AddRange(collection: dto.Populations.Select(x => new Population(value: x)));

            var selections = new List<Selection>(capacity: dto.Selections.Capacity);
            selections.AddRange(collection: dto.Selections.Select(x => new Selection(value: x)));

            var crossovers = new List<Crossover>(capacity: dto.Crossovers.Capacity);
            crossovers.AddRange(collection: dto.Crossovers.Select(x => new Crossover(value: x)));

            return new PresetsProfile(id: dto.Id, name: dto.Name, mutations: mutations, crossovers: crossovers, selections: selections, populations: populations);
        }
    }
}
