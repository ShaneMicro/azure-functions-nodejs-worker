using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Azure.Functions.NodeJs.Tests.E2E
{
    public class FunctionAppFixture : IDisposable, IAsyncLifetime
    {
        private readonly ILogger _logger;
        private bool _disposed;
        private Process _funcProcess;

        public FunctionAppFixture()
        {
            // initialize logging
#pragma warning disable CS0618 // Type or member is obsolete
            ILoggerFactory loggerFactory = new LoggerFactory().AddConsole();
#pragma warning restore CS0618 // Type or member is obsolete
            _logger = loggerFactory.CreateLogger<FunctionAppFixture>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _logger.LogInformation("FunctionAppFixture disposing.");

                    if (_funcProcess != null)
                    {
                        _logger.LogInformation($"Shutting down functions host for {Constants.FunctionAppCollectionName}");
                        _funcProcess.Kill();
                        _funcProcess.Dispose();
                    }
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public async Task InitializeAsync()
        {
            await EventHubsHelpers.VerifyEventHubsExist();
            await CosmosDBHelpers.CreateDocumentCollections();

            // start host via CLI if testing locally
            if (Constants.FunctionsHostUrl.Contains("localhost"))
            {
                // kill existing func processes
                _logger.LogInformation("Shutting down any running functions hosts..");
                FixtureHelpers.KillExistingFuncHosts();

                await FixtureHelpers.SetResourceSuffixInTestFunctionApp(_logger);

                // start functions process
                _logger.LogInformation($"Starting functions host for {Constants.FunctionAppCollectionName}..");
                _funcProcess = FixtureHelpers.GetFuncHostProcess();

                FixtureHelpers.StartProcessWithLogging(_funcProcess);

                Thread.Sleep(TimeSpan.FromSeconds(30));
            }
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }

    [CollectionDefinition(Constants.FunctionAppCollectionName)]
    public class FunctionAppCollection : ICollectionFixture<FunctionAppFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
