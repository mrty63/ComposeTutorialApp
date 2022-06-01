using Polly;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Pages.Extensions
{
    interface IPolicyAsyncExecutor
    {
        PolicyRegistry PolicyRegistry { get; set; }

        Task<T> ExecuteAsync<T>(Func<Task<T>> action);

        Task<T> ExecuteAsync<T>(Func<Context, Task<T>> action, Context context);

        Task ExecuteAsync(Func<Task> action);

        Task ExecuteAsync(Func<Context, Task> action, Context context);
    }
}
