using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAShow.Shared.Models.DTOs;
using Microsoft.Toolkit.Uwp.UI;

namespace EAShow.Core.POCO
{
    public class ChartData
    {
        public ChartData(GADefinition gaDefinition, AdvancedCollectionView filteringCollection, Guid key)
        {
            GaDefinition = gaDefinition;
            FilteringCollection = filteringCollection;
            Key = key;
        }

        public Guid Key { get; }
        public GADefinition GaDefinition { get; }
        public AdvancedCollectionView FilteringCollection { get; }
    }
}
