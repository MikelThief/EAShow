using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using LiteDB;

namespace EAShow.Shared.Models
{
    [DebuggerDisplay(value: "Mutations={Mutations.Count}, Crossovers={Crossovers.Count}, Selections={Selections.Count}, Populations={CrosPopulationssovers.Count}",
        Name = "Profile {Name}")]
    public class Profile
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<Mutation> Mutations { get; set; }

        public List<Crossover> Crossovers { get; set; }

        public List<Selection> Selections { get; set; }

        public List<Population> Populations { get; set; }
    }
}
