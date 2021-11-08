// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Azure.Functions.NodeJs.Tests.E2E
{
    [Collection(Constants.FunctionAppCollectionName)]
    public class EventHubsEndToEndTests
    {
        private readonly FunctionAppFixture _fixture;

        public EventHubsEndToEndTests(FunctionAppFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task EventHubTriggerAndOutputJSON_Succeeds()
        {
            string expectedEventId = Guid.NewGuid().ToString();
            var outputQueueName = AzureHelpers.GetNameWithSuffix(Constants.EventHubs.Json_Test.OutputPrefix);
            var inputQueueName = AzureHelpers.GetNameWithSuffix(Constants.EventHubs.Json_Test.InputPrefix);
            try
            {
                await SetupQueue(outputQueueName);

                // Need to setup EventHubs: test-inputjson-java and test-outputjson-java
                await EventHubsHelpers.SendJSONMessagesAsync(expectedEventId, inputQueueName);

                //Verify
                var queueMessage = await StorageHelpers.ReadFromQueue(outputQueueName);
                JObject json = JObject.Parse(queueMessage);
                Assert.Contains(expectedEventId, json["value"].ToString());
            }
            finally
            {
                //Clear queue
                await StorageHelpers.ClearQueue(outputQueueName);
            }
        }

        [Fact]
        public async Task EventHubTriggerAndOutputString_Succeeds()
        {
            string expectedEventId = Guid.NewGuid().ToString();
            var outputQueueName = AzureHelpers.GetNameWithSuffix(Constants.EventHubs.String_Test.OutputPrefix);
            var inputQueueName = AzureHelpers.GetNameWithSuffix(Constants.EventHubs.String_Test.InputPrefix);
            try
            {
                await SetupQueue(outputQueueName);

                // Need to setup EventHubs: test-input-one-node
                await EventHubsHelpers.SendMessagesAsync(expectedEventId, inputQueueName);

                //Verify
                var queueMessage = await StorageHelpers.ReadFromQueue(outputQueueName);
                Assert.Contains(expectedEventId, queueMessage);
            }
            finally
            {
                //Clear queue
                await StorageHelpers.ClearQueue(outputQueueName);
            }
        }

        [Fact]
        public async Task EventHubTriggerCardinalityOne_Succeeds()
        {
            string expectedEventId = Guid.NewGuid().ToString();
            var outputQueueName = AzureHelpers.GetNameWithSuffix(Constants.EventHubs.Cardinality_One_Test.OutputPrefix);
            var inputQueueName = AzureHelpers.GetNameWithSuffix(Constants.EventHubs.Cardinality_One_Test.InputPrefix);
            try
            {
                await SetupQueue(outputQueueName);

                // Need to setup EventHubs: test-inputOne-java and test-outputone-java
                await EventHubsHelpers.SendMessagesAsync(expectedEventId, inputQueueName);

                //Verify
                IEnumerable<string> queueMessages = await StorageHelpers.ReadMessagesFromQueue(outputQueueName);
                Assert.True(queueMessages.All(msg => msg.Contains(expectedEventId)));
            }
            finally
            {
                //Clear queue
                await StorageHelpers.ClearQueue(outputQueueName);
            }
        }

        private static async Task SetupQueue(string queueName)
        {
            //Clear queue
            await StorageHelpers.ClearQueue(queueName);

            //Set up and trigger            
            await StorageHelpers.CreateQueue(queueName);
        }
    }
}