using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace EAShow.Infrastructure.Commands.DelegateCommand
{
    /// <summary>
    /// Provide a way to observe property changes of INotifyPropertyChanged objects and invokes a
    /// custom action when the PropertyChanged event is fired.
    /// </summary>
    internal class PropertyObserver
    {
        private readonly Action _action;

        private PropertyObserver(Expression propertyExpression, Action action)
        {
            _action = action;
            SubscribeListeners(propertyExpression: propertyExpression);
        }

        private void SubscribeListeners(Expression propertyExpression)
        {
            var propNameStack = new Stack<string>();
            while (propertyExpression is MemberExpression temp) // Gets the root of the property chain.
            {
                propertyExpression = temp.Expression;
                propNameStack.Push(item: temp.Member.Name); // Records the name of each property.
            }

            if (!(propertyExpression is ConstantExpression constantExpression))
                throw new NotSupportedException(message: "Operation not supported for the given expression type. " +
                                                "Only MemberExpression and ConstantExpression are currently supported.");

            var propObserverNodeRoot = new PropertyObserverNode(propertyName: propNameStack.Pop(), action: _action);
            PropertyObserverNode previousNode = propObserverNodeRoot;
            foreach (var propName in propNameStack) // Create a node chain that corresponds to the property chain.
            {
                var currentNode = new PropertyObserverNode(propertyName: propName, action: _action);
                previousNode.Next = currentNode;
                previousNode = currentNode;
            }

            object propOwnerObject = constantExpression.Value;

            if (!(propOwnerObject is INotifyPropertyChanged inpcObject))
                throw new InvalidOperationException(message: "Trying to subscribe PropertyChanged listener in object that " +
                                                    $"owns '{propObserverNodeRoot.PropertyName}' property, but the object does not implements INotifyPropertyChanged.");

            propObserverNodeRoot.SubscribeListenerFor(inpcObject: inpcObject);
        }

        /// <summary>
        /// Observes a property that implements INotifyPropertyChanged, and automatically calls a custom action on
        /// property changed notifications. The given expression must be in this form: "() => Prop.NestedProp.PropToObserve".
        /// </summary>
        /// <param name="propertyExpression">Expression representing property to be observed. Ex.: "() => Prop.NestedProp.PropToObserve".</param>
        /// <param name="action">Action to be invoked when PropertyChanged event occours.</param>
        internal static PropertyObserver Observes<T>(Expression<Func<T>> propertyExpression, Action action)
        {
            return new PropertyObserver(propertyExpression: propertyExpression.Body, action: action);
        }
    }
}
