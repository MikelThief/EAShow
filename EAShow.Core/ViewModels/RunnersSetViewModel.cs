using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Core.Helpers;
using EAShow.Infrastructure.Commands.AsyncCommand;
using EAShow.Infrastructure.Commands.DelegateCommand;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace EAShow.Core.ViewModels
{
    public class RunnersSetViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public DelegateCommand<TabClosingEventArgs> CloseTabCommand { get; }

        public DelegateCommand AddTabCommand { get; }

        public RunnersSetViewModel()
        {
            CloseTabCommand = new DelegateCommand<TabClosingEventArgs>(executeMethod: CloseTab);
            AddTabCommand = new DelegateCommand(executeMethod: AddTab);
        }

        private void CloseTab(TabClosingEventArgs args)
        {
            if (args.Item is RunnerInstanceViewModel item)
            {
                Items.Remove(item);
            }
        }

        private void AddTab()
        {
            var newIndex = Items.Any() ? Items.Max((IScreen screen) => (screen as RunnerInstanceViewModel).Index) + 1 : 1;

            Items.Add(new RunnerInstanceViewModel
            {
                Index = (short) newIndex,
                Header = "RunnerInstance_EmptyHeader".GetLocalized()
            });
        }
    }
}
