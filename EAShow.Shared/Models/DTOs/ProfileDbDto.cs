using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteDB;

namespace EAShow.Shared.Models.DTOs
{
    public class ProfileDbDto
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public List<decimal> Mutations { get; set; }

        public List<Crossovers> Crossovers { get; set; }

        public List<Selections> Selections { get; set; }

        public List<int> Populations { get; set; }

        public static ProfileDbDto From(PresetsProfile presetsProfileEntity)
        {
            var mutations = new List<decimal>(presetsProfileEntity.Mutations.Capacity);
            mutations.AddRange(collection: presetsProfileEntity.Mutations.Select(selector: x => x.Value));

            var populations = new List<int>(presetsProfileEntity.Populations.Capacity);
            populations.AddRange(collection: presetsProfileEntity.Populations.Select(selector: x => x.Value));

            var selections = new List<Selections>(capacity: presetsProfileEntity.Selections.Capacity);
            selections.AddRange(collection: presetsProfileEntity.Selections.Select(selector: x => x.Value));

            var crossovers = new List<Crossovers>(capacity: presetsProfileEntity.Crossovers.Capacity);
            crossovers.AddRange(collection: presetsProfileEntity.Crossovers.Select(selector: x => x.Value));

            return new ProfileDbDto()
            {
                Id = presetsProfileEntity.Id,
                Name = presetsProfileEntity.Name,
                Crossovers = crossovers,
                Mutations = mutations,
                Populations = populations,
                Selections = selections
            };

        }
    }
}
