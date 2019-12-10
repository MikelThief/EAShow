using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.Mvvm;

namespace EAShow.Infrastructure.Abstractions.Interfaces
{
    /// <summary>
    /// A strongly typed async version of <see cref="IAsyncCommand"/>.
    /// </summary>
    public interface IAsyncCommand<T> : IAsyncCommand
    {
        /// <summary>
        /// Executes the asynchronous command.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        Task ExecuteAsync(T parameter);
    }
}