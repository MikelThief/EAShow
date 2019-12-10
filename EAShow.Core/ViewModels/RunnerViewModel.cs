using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Infrastructure.Commands.AsyncCommand;
using EAShow.Infrastructure.Commands.DelegateCommand;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace EAShow.Core.ViewModels
{
    public class RunnerViewModel : Screen
    {
        public BindableCollection<EAViewModel> Tabs { get; }

        public DelegateCommand<TabClosingEventArgs> CloseTabCommand { get; }

        public DelegateCommand AddTabCommand { get; }

        public RunnerViewModel()
        {
            Tabs = new BindableCollection<EAViewModel>();
            CloseTabCommand = new DelegateCommand<TabClosingEventArgs>(executeMethod: CloseTab);
            AddTabCommand = new DelegateCommand(executeMethod: AddTab);

        }
        private void CloseTab(TabClosingEventArgs args)
        {
            if (args.Item is EAViewModel item)
            {
                Tabs.Remove(item);
            }
        }

        private void AddTab()
        {
            var newIndex = Tabs.Any() ? Tabs.Max(t => t.Index) + 1 : 1;
            //Tabs.Add(new TabViewItemData()
            //{
            //    Index = newIndex,
            //    Header = $"Item {newIndex}",
            //    Content = $"This is the content for Item {newIndex}"
            //});
        }
    }
}
