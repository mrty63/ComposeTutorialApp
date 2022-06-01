using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Latency;
using Polly.Contrib.Simmy.Outcomes;
using Polly.Registry;
using Polly.Contrib.Simmy.Behavior;
using Polly.Extensions.Http;
using Polly.Contrib.WaitAndRetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace WebApp.Chaos
{
    public static class SimmyExtensions
    {
        private static OperationChaosSetting GetOperationChaosSettings(this Context context) => context.GetChaosSettings()?.GetSettingsFor(context.OperationKey);

        private static readonly Task<bool> NotEnabled = Task.FromResult(false);
        private static readonly Task<double> NoInjectionRate = Task.FromResult<double>(0);
        private static readonly Task<Exception> NoExceptionResult = Task.FromResult<Exception>(null);
        private static readonly Task<HttpResponseMessage> NoHttpResponse = Task.FromResult<HttpResponseMessage>(null);
        private static readonly Task<TimeSpan> NoLatency = Task.FromResult(TimeSpan.Zero);

        /// <summary>
        /// Add chaos-injection policies to every policy returning <see cref="IAsyncPolicy{HttpResponseMessage}"/>
        /// in the supplied <paramref name="registry"/>
        /// </summary>
        /// <param name="registry">The <see cref="IPolicyRegistry{String}"/> whose policies should be decorated with chaos policies.</param>
        /// <returns>The policy registry.</returns>
        /// 
        
        public static IPolicyRegistry<string> AddChaosInjectors(this IPolicyRegistry<string> registry)
        {
            foreach (KeyValuePair<string, IsPolicy> policyEntry in registry)
            {
                if (policyEntry.Value is IAsyncPolicy<HttpResponseMessage> policy)
                {
                    registry[policyEntry.Key] = policy
                            .WrapAsync(MonkeyPolicy.InjectExceptionAsync(with =>
                                with.Fault<HttpResponseMessage>(GetException)
                                    .InjectionRate(GetInjectionRate)
                                    .EnabledWhen(GetEnabled)))
                            .WrapAsync(MonkeyPolicy.InjectResultAsync<HttpResponseMessage>(with =>
                                with.Result(GetHttpResponseMessage)
                                    .InjectionRate(GetInjectionRate)
                                    .EnabledWhen(GetHttpResponseEnabled)))
                            .WrapAsync(MonkeyPolicy.InjectLatencyAsync<HttpResponseMessage>(with =>
                                with.Latency(GetLatency)
                                    .InjectionRate(GetInjectionRate)
                                    .EnabledWhen(GetEnabled)));
                }

            }

            return registry;
        }

        private static Task<bool> GetEnabled(Context context, CancellationToken token)
        {
            OperationChaosSetting chaosSettings = context.GetOperationChaosSettings();
            if (chaosSettings == null) return NotEnabled;

            return Task.FromResult(chaosSettings.Enabled);
        }

        private static Task<Double> GetInjectionRate(Context context, CancellationToken token)
        {
            OperationChaosSetting chaosSettings = context.GetOperationChaosSettings();
            if (chaosSettings == null) return NoInjectionRate;

            return Task.FromResult(chaosSettings.InjectionRate);
        }

        private static Task<Exception> GetException(Context context, CancellationToken token)
        {
            OperationChaosSetting chaosSettings = context.GetOperationChaosSettings();
            if (chaosSettings == null) return NoExceptionResult;

            string exceptionName = chaosSettings.Exception;
            if (String.IsNullOrWhiteSpace(exceptionName)) return NoExceptionResult;

            try
            {
                Type exceptionType = Type.GetType(exceptionName);
                if (exceptionType == null) return NoExceptionResult;

                if (!typeof(Exception).IsAssignableFrom(exceptionType)) return NoExceptionResult;

                var instance = Activator.CreateInstance(exceptionType);
                return Task.FromResult(instance as Exception);
            }
            catch
            {
                return NoExceptionResult;
            }
        }

        private static Task<bool> GetHttpResponseEnabled(Context context, CancellationToken token)
        {
            if (GetHttpResponseMessage(context, CancellationToken.None) == NoHttpResponse) return NotEnabled;

            return GetEnabled(context, token);
        }

        private static Task<HttpResponseMessage> GetHttpResponseMessage(Context context, CancellationToken token)
        {
            OperationChaosSetting chaosSettings = context.GetOperationChaosSettings();
            if (chaosSettings == null) return NoHttpResponse;

            int statusCode = chaosSettings.StatusCode;
            if (statusCode < 200) return NoHttpResponse;

            try
            {
                return Task.FromResult(new HttpResponseMessage((HttpStatusCode)statusCode));
            }
            catch
            {
                return NoHttpResponse;
            }
        }

        private static Task<TimeSpan> GetLatency(Context context, CancellationToken token)
        {
            OperationChaosSetting chaosSettings = context.GetOperationChaosSettings();
            if (chaosSettings == null) return NoLatency;

            int milliseconds = chaosSettings.LatencyMs;
            if (milliseconds <= 0) return NoLatency;

            return Task.FromResult(TimeSpan.FromMilliseconds(milliseconds));
        }




        /// <summary>
        /// Add chaos-injection policies to every policy returning <see cref="IAsyncPolicy{HttpResponseMessage}"/>
        /// in the supplied <paramref name="registry"/>
        /// </summary>
        /// <param name="registry">The <see cref="IPolicyRegistry{String}"/> whose policies should be decorated with chaos policies.</param>
        /// <returns>The policy registry.</returns>
        
        //public static IPolicyRegistry<string> AddHttpChaosInjectors(this IPolicyRegistry<string> registry)
        //{
        //    foreach (var policyEntry in registry)
        //    {
        //        if (policyEntry.Value is IAsyncPolicy<HttpResponseMessage> policy)
        //        {
        //            registry[policyEntry.Key] = policy
        //                .WrapAsync(MonkeyPolicy.InjectExceptionAsync(with =>
        //                    with.Fault(GetException)
        //                        .InjectionRate(GetInjectionRate)
        //                        .EnabledWhen(GetEnabled)))
        //                .WrapAsync(MonkeyPolicy.InjectResultAsync<HttpResponseMessage>(with =>
        //                    with.Result(GetHttpResponseMessage)
        //                        .InjectionRate(GetInjectionRate)
        //                        .EnabledWhen(GetEnabled)))
        //                .WrapAsync(MonkeyPolicy.InjectLatencyAsync<HttpResponseMessage>(with =>
        //                    with.Latency(GetLatency)
        //                        .InjectionRate(GetInjectionRate)
        //                        .EnabledWhen(GetEnabled)))
        //                .WrapAsync(MonkeyPolicy.InjectBehaviourAsync<HttpResponseMessage>(with =>
        //                    with.Behaviour(RestartNodes)
        //                        .InjectionRate(GetClusterChaosInjectionRate)
        //                        .EnabledWhen(GetClusterChaosEnabled)))
        //                .WrapAsync(MonkeyPolicy.InjectBehaviourAsync<HttpResponseMessage>(with =>
        //                    with.Behaviour(StopNodes)
        //                        .InjectionRate(GetClusterChaosInjectionRate)
        //                        .EnabledWhen(GetClusterChaosEnabled)));
        //        }
        //    }

        //    return registry;
        //}

        ///// <summary>
        ///// Add chaos-injection policies to every policy returning <see cref="IAsyncPolicy{T}"/>
        ///// in the supplied <paramref name="registry"/>
        ///// </summary>
        ///// <param name="registry">The <see cref="IPolicyRegistry{String}"/> whose policies should be decorated with chaos policies.</param>
        ///// <returns>The policy registry.</returns>
        //public static IPolicyRegistry<string> AddChaosInjectors<T>(this IPolicyRegistry<string> registry)
        //{
        //    foreach (var policyEntry in registry)
        //    {
        //        if (policyEntry.Value is IAsyncPolicy<T> policy)
        //        {
        //            registry[policyEntry.Key] = policy
        //                .WrapAsync(MonkeyPolicy.InjectExceptionAsync((with =>
        //                    with.Fault<T>(GetException)
        //                        .InjectionRate(GetInjectionRate)
        //                        .EnabledWhen(GetEnabled))))
        //                .WrapAsync(MonkeyPolicy.InjectLatencyAsync<T>(with =>
        //                    with.Latency(GetLatency)
        //                        .InjectionRate(GetInjectionRate)
        //                        .EnabledWhen(GetEnabled)))
        //                .WrapAsync(MonkeyPolicy.InjectBehaviourAsync<T>(with =>
        //                    with.Behaviour(RestartNodes)
        //                        .InjectionRate(GetClusterChaosInjectionRate)
        //                        .EnabledWhen(GetClusterChaosEnabled)))
        //                .WrapAsync(MonkeyPolicy.InjectBehaviourAsync<T>(with =>
        //                    with.Behaviour(StopNodes)
        //                        .InjectionRate(GetClusterChaosInjectionRate)
        //                        .EnabledWhen(GetClusterChaosEnabled)));
        //        }
        //    }

        //    return registry;
        //}

        ///// <summary>
        ///// Add chaos-injection policies to every policy./>
        ///// in the supplied <paramref name="registry"/>
        ///// </summary>
        ///// <param name="registry">The <see cref="IPolicyRegistry{String}"/> whose policies should be decorated with chaos policies.</param>
        ///// <returns>The policy registry.</returns>


        //private static Task<bool> GetClusterChaosEnabled(Context context, CancellationToken ct)
        //{
        //    var chaosSettings = context.GetChaosSettings();
        //    if (chaosSettings == null) return NotEnabled;

        //    return Task.FromResult(chaosSettings.ClusterChaosEnabled);
        //}

        //private static Task<double> GetClusterChaosInjectionRate(Context context, CancellationToken ct)
        //{
        //    var chaosSettings = context.GetChaosSettings();
        //    if (chaosSettings == null) return NoInjectionRate;

        //    return Task.FromResult(chaosSettings.ClusterChaosInjectionRate);
        //}

        //private static Task<bool> GetEnabled(Context context, CancellationToken ct)
        //{
        //    var chaosSettings = context.GetOperationChaosSettings();
        //    if (chaosSettings == null) return NotEnabled;

        //    return Task.FromResult(chaosSettings.Enabled);
        //}

        //private static Task<double> GetInjectionRate(Context context, CancellationToken ct)
        //{
        //    var chaosSettings = context.GetOperationChaosSettings();
        //    if (chaosSettings == null) return NoInjectionRate;

        //    return Task.FromResult(chaosSettings.InjectionRate);
        //}

        //private static Task<Exception> GetException(Context context, CancellationToken token)
        //{
        //    var chaosSettings = context.GetOperationChaosSettings();
        //    if (chaosSettings == null) return NoExceptionResult;

        //    string exceptionName = chaosSettings.Exception;
        //    if (string.IsNullOrWhiteSpace(exceptionName)) return NoExceptionResult;

        //    try
        //    {
        //        if (exceptionName == typeof(SqlError).FullName) return Task.FromResult(CreateSqlException() as Exception);

        //        Type exceptionType = Type.GetType(exceptionName);
        //        if (exceptionType == null) return NoExceptionResult;

        //        if (!typeof(Exception).IsAssignableFrom(exceptionType)) return NoExceptionResult;

        //        var instance = Activator.CreateInstance(exceptionType);
        //        return Task.FromResult(instance as Exception);
        //    }
        //    catch
        //    {
        //        return NoExceptionResult;
        //    }
        //}

        //private static Task<bool> GetHttpResponseEnabled(Context context)
        //{
        //    if (GetHttpResponseMessage(context, CancellationToken.None) == NoHttpResponse) return NotEnabled;

        //    return GetEnabled(context, CancellationToken.None);
        //}

        //private static Task<HttpResponseMessage> GetHttpResponseMessage(Context context, CancellationToken token)
        //{
        //    var chaosSettings = context.GetOperationChaosSettings();
        //    if (chaosSettings == null) return NoHttpResponse;

        //    int statusCode = chaosSettings.StatusCode;
        //    if (statusCode < 200) return NoHttpResponse;

        //    try
        //    {
        //        return Task.FromResult(new HttpResponseMessage((HttpStatusCode)statusCode));
        //    }
        //    catch
        //    {
        //        return NoHttpResponse;
        //    }
        //}

        //private static Task<TimeSpan> GetLatency(Context context, CancellationToken token)
        //{
        //    var chaosSettings = context.GetOperationChaosSettings();
        //    if (chaosSettings == null) return NoLatency;

        //    int milliseconds = chaosSettings.LatencyMs;
        //    if (milliseconds <= 0) return NoLatency;

        //    return Task.FromResult(TimeSpan.FromMilliseconds(milliseconds));
        //}

        //private static Task RestartNodes(Context context, CancellationToken token)
        //{
        //    var chaosGeneralSettings = context.GetChaosSettings();
        //    if (chaosGeneralSettings == null) return NoHttpResponse;
        //    if (chaosGeneralSettings.PercentageNodesToRestart <= 0) return NoHttpResponse;

        //    return ClusterChaosManager.RestartNodes(context.GetChaosSettings(), chaosGeneralSettings.PercentageNodesToRestart);
        //}

        //private static Task StopNodes(Context context, CancellationToken token)
        //{
        //    var chaosGeneralSettings = context.GetChaosSettings();
        //    if (chaosGeneralSettings == null) return NoHttpResponse;
        //    if (chaosGeneralSettings.PercentageNodesToStop <= 0) return NoHttpResponse;

        //    return ClusterChaosManager.StopNodes(context.GetChaosSettings(), chaosGeneralSettings.PercentageNodesToStop);
        //}

        //private static SqlException CreateSqlException()
        //{
        //    var collectionConstructor = typeof(SqlErrorCollection)
        //        .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, //visibility
        //            null, //binder
        //            new Type[0],
        //            null);

        //    var addMethod = typeof(SqlErrorCollection).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance);
        //    var errorCollection = (SqlErrorCollection)collectionConstructor.Invoke(null);
        //    var errorConstructor = typeof(SqlError).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null,
        //        new[]
        //        {
        //                typeof (int), typeof (byte), typeof (byte), typeof (string), typeof(string), typeof (string),
        //                typeof (int), typeof (uint), typeof(Exception)
        //        }, null);

        //    var error = errorConstructor.Invoke(new object[] { ServiceCurrentlyBusySqlErrorNumber, (byte)0, (byte)0, "server", "errMsg", "proccedure", 100, (uint)0, null });
        //    addMethod.Invoke(errorCollection, new[] { error });

        //    var constructor = typeof(SqlException)
        //        .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, //visibility
        //            null, //binder
        //            new[] { typeof(string), typeof(SqlErrorCollection), typeof(Exception), typeof(Guid) },
        //            null); //param modifiers

        //    return (SqlException)constructor.Invoke(new object[] { $"Error message: {ServiceCurrentlyBusySqlErrorNumber}", errorCollection, new DataException(), Guid.NewGuid() });
        //}
        
        public static IServiceCollection AddChaosApiHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddHttpClient<ChaosApiHttpClient>(client =>
            //{
            //    //client.Timeout = TimeSpan.FromSeconds(5);
            //    //client.BaseAddress = new Uri(configuration.GetValue<string>("ChaosApiSettings:BaseUrl"));
            //});

            services.AddScoped<Lazy<Task<AppChaosSettings>>>(sp =>
            {
                // we use LazyThreadSafetyMode.None in order to avoid locking.
                var chaosApiHttpClient = sp.GetRequiredService<ChaosApiHttpClient>();
                return new Lazy<Task<AppChaosSettings>>(() => chaosApiHttpClient.GetGeneralChaosSettings(), LazyThreadSafetyMode.None);
            });

            return services;
        }
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
              // Handle HttpRequestExceptions, 408 and 5xx status codes
              .HandleTransientHttpError()
              // Handle 404 not found
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
              // Handle 401 Unauthorized
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
              // What to do if any of the above erros occur:
              // Retry 3 times, each time wait 1,2 and 4 seconds before retrying.
              .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
        private static IAsyncPolicy<HttpResponseMessage> GetJitterRetryPolicy()
        {
            //jitter
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 5);


            return HttpPolicyExtensions
              // Handle HttpRequestExceptions, 408 and 5xx status codes
              .HandleTransientHttpError()
              // Handle 404 not found
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
              // Handle 401 Unauthorized
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                // What to do if any of the above erros occur:
                // Retry 3 times, each time wait 1,2 and 4 seconds before retrying.
                .WaitAndRetryAsync(delay);
        }
        private static IAsyncPolicy<HttpResponseMessage> GetResiliencePolicy()
        {
            // Define a policy which will form our resilience strategy.  These could be anything.  The settings for them could obviously be drawn from config too.
            var retry = HttpPolicyExtensions.HandleTransientHttpError()
                .RetryAsync(2);

            return retry;
        }

    }
}
