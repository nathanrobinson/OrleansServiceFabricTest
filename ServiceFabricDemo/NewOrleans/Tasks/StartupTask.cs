using System;
using System.Threading;
using System.Threading.Tasks;
using Grains.Interfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;

namespace NewOrleans.Tasks
{
    public class StartupTask : IStartupTask
    {
        private readonly IGrainFactory _grainFactory;
        private readonly ILogger<StartupTask> _logger;

        public StartupTask(IGrainFactory grainFactory, ILogger<StartupTask> logger)
        {
            _grainFactory = grainFactory;
            _logger = logger;
        }

        public Task Execute(CancellationToken cancellationToken)
        {
            // Message the grain repeatedly.
            var grain = _grainFactory.GetGrain<ICalculatorGrain>(Guid.Empty);
            Task.Factory
                .StartNew(
                    async () =>
                    {
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            try
                            {
                                var value = await grain.Add(1);
                                _logger.Info($"{value - 1} + 1 = {value}");
                                await Task.Delay(TimeSpan.FromSeconds(4), cancellationToken);
                            }
                            catch (Exception exception)
                            {
                               _logger.Warn(exception.HResult, "Exception in bootstrap provider. Ignoring.", exception);
                            }
                        }
                    }, cancellationToken)
                .Ignore();
            return Task.CompletedTask;
        }
    }
}