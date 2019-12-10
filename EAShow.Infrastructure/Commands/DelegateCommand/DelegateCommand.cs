using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Input;

namespace EAShow.Infrastructure.Commands.DelegateCommand

{
    /// <summary>
    /// An <see cref="ICommand"/> whose delegates do not take any parameters for <see cref="Execute()"/> and <see cref="CanExecute()"/>.
    /// Copied from Prism library source code. Modified to observe multiple properties.
    /// </summary>
    /// <see cref="DelegateCommandBase"/>
    /// <see cref="DelegateCommand{T}"/>
    /// <remarks>Source: https://github.com/PrismLibrary/Prism/blob/2ecbf13ed59b288d8d88446b535780bdc211b6e1/Source/Prism/Commands/DelegateCommand.cs </remarks>
    public class DelegateCommand : DelegateCommandBase
    {
        Action _executeMethod;
        Func<bool> _canExecuteMethod;

        /// <summary>
        /// Creates a new instance of <see cref="DelegateCommand"/> with the <see cref="Action"/> to invoke on execution.
        /// </summary>
        /// <param name="executeMethod">The <see cref="Action"/> to invoke when <see cref="ICommand.Execute(object)"/> is called.</param>
        public DelegateCommand(Action executeMethod)
            : this(executeMethod: executeMethod, canExecuteMethod: () => true)
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="DelegateCommand"/> with the <see cref="Action"/> to invoke on execution
        /// and a <see langword="Func" /> to query for determining if the command can execute.
        /// </summary>
        /// <param name="executeMethod">The <see cref="Action"/> to invoke when <see cref="ICommand.Execute"/> is called.</param>
        /// <param name="canExecuteMethod">The <see cref="Func{TResult}"/> to invoke when <see cref="ICommand.CanExecute"/> is called</param>
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base()
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException(paramName: nameof(executeMethod), message: "DelegateCommand cannot be null.");

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        ///<summary>
        /// Executes the command.
        ///</summary>
        public void Execute()
        {
            _executeMethod();
        }

        /// <summary>
        /// Determines if the command can be executed.
        /// </summary>
        /// <returns>Returns <see langword="true"/> if the command can execute,otherwise returns <see langword="false"/>.</returns>
        public bool CanExecute()
        {
            return _canExecuteMethod();
        }

        /// <summary>
        /// Handle the internal invocation of <see cref="ICommand.Execute(object)"/>
        /// </summary>
        /// <param name="parameter">Command Parameter</param>
        protected override void Execute(object parameter)
        {
            Execute();
        }

        /// <summary>
        /// Handle the internal invocation of <see cref="ICommand.CanExecute(object)"/>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns><see langword="true"/> if the Command Can Execute, otherwise <see langword="false" /></returns>
        protected override bool CanExecute(object parameter)
        {
            return CanExecute();
        }

        /// <summary>
        /// Observes a property that implements INotifyPropertyChanged, and automatically calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
        /// </summary>
        /// <typeparam name="T">The object type containing the property specified in the expression.</typeparam>
        /// <param name="propertyExpression">The property expression. Example: ObservesProperty(() => PropertyName).</param>
        /// <returns>The current instance of DelegateCommand</returns>
        public DelegateCommand ObservesProperty<T>(Expression<Func<T>> propertyExpression)
        {
            ObservesPropertyInternal(propertyExpression: propertyExpression);
            return this;
        }

        /// <summary>
        /// Observes a collection of properties that implement INotifyPropertyChanged, and automatically calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
        /// </summary>
        /// <typeparam name="T">The object type containing the property specified in the expression.</typeparam>
        /// <param name="propertyExpressions">The property expression collection. Example: ObservesProperty(() => PropertyName).</param>
        /// <returns>The current instance of DelegateCommand</returns>
        public DelegateCommand ObservesProperty<T>(IEnumerable<Expression<Func<T>>> propertyExpressions)
        {
            foreach (var propertyExpression in propertyExpressions)
            {
                ObservesPropertyInternal(propertyExpression: propertyExpression);
            }
            return this;
        }

        /// <summary>
        /// Observes a collection of properties that implement INotifyPropertyChanged, and automatically calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
        /// </summary>
        /// <typeparam name="T">The object type containing the property specified in the expression.</typeparam>
        /// <param name="propertyExpressions">The property expression collection. Example: ObservesProperty(() => PropertyName).</param>
        /// <returns>The current instance of DelegateCommand</returns>
        public DelegateCommand ObservesProperty<T>(params Expression<Func<T>>[] propertyExpressions)
        {
            foreach (var propertyExpression in propertyExpressions)
            {
                ObservesPropertyInternal(propertyExpression: propertyExpression);
            }
            return this;
        }

        /// <summary>
        /// Observes a property that is used to determine if this command can execute, and if it implements INotifyPropertyChanged it will automatically call DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
        /// </summary>
        /// <param name="canExecuteExpression">The property expression. Example: ObservesCanExecute(() => PropertyName).</param>
        /// <returns>The current instance of DelegateCommand</returns>
        public DelegateCommand ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            _canExecuteMethod = canExecuteExpression.Compile();
            ObservesPropertyInternal(propertyExpression: canExecuteExpression);
            return this;
        }

        /// <summary>
        /// Observes a collection of properties that is used to determine if this command can execute, and if they implement INotifyPropertyChanged they will automatically call DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
        /// </summary>
        /// <param name="canExecuteExpression">The property expression. Example: ObservesCanExecute(() => PropertyName).</param>
        /// <returns>The current instance of DelegateCommand</returns>
        public DelegateCommand ObservesCanExecute(params Expression<Func<bool>>[] canExecuteExpressions)
        {
            foreach (var canExecuteExpression in canExecuteExpressions)
            {
                _canExecuteMethod = canExecuteExpression.Compile();
                ObservesPropertyInternal(propertyExpression: canExecuteExpression);
            }
            return this;
        }
    }
}
