using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading;
using System.Windows.Input;

namespace EAShow.Infrastructure.Commands.DelegateCommand
{
    /// <summary>
    /// An <see cref="ICommand"/> whose delegates can be attached for <see cref="Execute"/> and <see cref="CanExecute"/>.
    /// Copied from Prism library source code. IActiveAware interface was removed
    /// </summary>
    /// <remarks>Source: https://github.com/PrismLibrary/Prism/blob/2ecbf13ed59b288d8d88446b535780bdc211b6e1/Source/Prism/Commands/DelegateCommandBase.cs</remarks>
    public abstract class DelegateCommandBase : ICommand
    {
        private SynchronizationContext _synchronizationContext;
        private readonly HashSet<string> _observedPropertiesExpressions = new HashSet<string>();

        /// <summary>
        /// Creates a new instance of a <see cref="DelegateCommandBase"/>, specifying both the execute action and the can execute function.
        /// </summary>
        protected DelegateCommandBase()
        {
            _synchronizationContext = SynchronizationContext.Current;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public virtual event EventHandler CanExecuteChanged;

        /// <summary>
        /// Raises <see cref="ICommand.CanExecuteChanged"/> so every
        /// command invoker can requery <see cref="ICommand.CanExecute"/>.
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                if (_synchronizationContext != null && _synchronizationContext != SynchronizationContext.Current)
                    _synchronizationContext.Post(d: (o) => handler.Invoke(sender: this, e: EventArgs.Empty), state: null);
                else
                    handler.Invoke(sender: this, e: EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises <see cref="CanExecuteChanged"/> so every command invoker
        /// can requery to check if the command can execute.
        /// <remarks>Note that this will trigger the execution of <see cref="CanExecuteChanged"/> once for each invoker.</remarks>
        /// </summary>
        [SuppressMessage(category: "Microsoft.Design", checkId: "CA1030:UseEventsWhereAppropriate")]
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        void ICommand.Execute(object parameter)
        {
            Execute(parameter: parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter: parameter);
        }

        /// <summary>
        /// Handle the internal invocation of <see cref="ICommand.Execute(object)"/>
        /// </summary>
        /// <param name="parameter">Command Parameter</param>
        protected abstract void Execute(object parameter);

        /// <summary>
        /// Handle the internal invocation of <see cref="ICommand.CanExecute(object)"/>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns><see langword="true"/> if the Command Can Execute, otherwise <see langword="false" /></returns>
        protected abstract bool CanExecute(object parameter);

        /// <summary>
        /// Observes a property that implements INotifyPropertyChanged, and automatically calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
        /// </summary>
        /// <typeparam name="T">The object type containing the property specified in the expression.</typeparam>
        /// <param name="propertyExpression">The property expression. Example: ObservesProperty(() => PropertyName).</param>
        protected internal void ObservesPropertyInternal<T>(Expression<Func<T>> propertyExpression)
        {
            if (_observedPropertiesExpressions.Contains(item: propertyExpression.ToString()))
            {
                throw new ArgumentException(message: $"{propertyExpression.ToString()} is already being observed.",
                    paramName: nameof(propertyExpression));
            }
            else
            {
                _observedPropertiesExpressions.Add(item: propertyExpression.ToString());
                PropertyObserver.Observes(propertyExpression: propertyExpression, action: RaiseCanExecuteChanged);
            }
        }
    }
}
