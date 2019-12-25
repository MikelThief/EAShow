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

        public static ProfileDbDto From(Profile profileEntity)
        {
            var mutations = new List<decimal>(profileEntity.Mutations.Capacity);
            mutations.AddRange(collection: profileEntity.Mutations.Select(selector: x => x.Value));

            var populations = new List<int>(profileEntity.Populations.Capacity);
            populations.AddRange(collection: profileEntity.Populations.Select(selector: x => x.Value));

            var selections = new List<Selections>(capacity: profileEntity.Selections.Capacity);
            selections.AddRange(collection: profileEntity.Selections.Select(selector: x => x.Value));

            var crossovers = new List<Crossovers>(capacity: profileEntity.Crossovers.Capacity);
            crossovers.AddRange(collection: profileEntity.Crossovers.Select(selector: x => x.Value));

            return new ProfileDbDto()
            {
                Id = profileEntity.Id,
                Name = profileEntity.Name,
                Crossovers = crossovers,
                Mutations = mutations,
                Populations = populations,
                Selections = selections
            };

        }
    }
}
