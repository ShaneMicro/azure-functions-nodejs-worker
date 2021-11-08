// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Xunit;

namespace Azure.Functions.NodeJs.Tests.E2E
{
    [Collection(Constants.FunctionAppCollectionName)]
    public class StorageEndToEndTests
    {
        private FunctionAppFixture _fixture;

        public StorageEndToEndTests(FunctionAppFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task QueueTriggerAndOutput_Succeeds()
        {
            string expectedQueueMessage = Guid.NewGuid().ToString();
            var outputQueueName = AzureHelpers.GetNameWithSuffix(Constants.Queue.OutputBindingPrefix);
            var inputQueueName = AzureHelpers.GetNameWithSuffix(Constants.Queue.InputBindingPrefix);

            //Clear queue
            await StorageHelpers.ClearQueue(outputQueueName);
            await StorageHelpers.ClearQueue(inputQueueName);

            //Set up and trigger            
            await StorageHelpers.CreateQueue(outputQueueName);
            await StorageHelpers.InsertIntoQueue(inputQueueName, expectedQueueMessage);

            //Verify
            var queueMessage = await StorageHelpers.ReadFromQueue(outputQueueName);
            Assert.Equal(expectedQueueMessage, queueMessage);
        }
    }
}