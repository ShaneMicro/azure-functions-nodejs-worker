// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Functions.NodeJs.Tests.E2E
{
    public class EventHubsHelpers
    {
        public static async Task SendJSONMessagesAsync(string eventId, string eventHubName)
        {
            // write 3 events
            List<EventData> events = new List<EventData>();
            string[] ids = new string[3];
            for (int i = 0; i < 3; i++)
            {
                ids[i] = eventId + $"TestEvent{i}";
                JObject jo = new JObject
                {
                    { "value", ids[i] }
                };
                var evt = new EventData(Encoding.UTF8.GetBytes(jo.ToString(Formatting.None)));
                evt.Properties.Add("TestIndex", i);
                events.Add(evt);
            }

            EventHubsConnectionStringBuilder builder = new EventHubsConnectionStringBuilder(Constants.EventHubs.EventHubsConnectionStringSetting);
            builder.EntityPath = eventHubName;
            EventHubClient eventHubClient = EventHubClient.CreateFromConnectionString(builder.ToString());
            await eventHubClient.SendAsync(events);
        }

        public static async Task SendMessagesAsync(string eventId, string evenHubName)
        {
            // write 3 events
            List<EventData> events = new List<EventData>();
            string[] ids = new string[3];
            for (int i = 0; i < 3; i++)
            {
                ids[i] = eventId + $"TestEvent{i}";
                var evt = new EventData(Encoding.UTF8.GetBytes(ids[i]));
                evt.Properties.Add("TestIndex", i);
                events.Add(evt);
            }

            EventHubsConnectionStringBuilder builder = new EventHubsConnectionStringBuilder(Constants.EventHubs.EventHubsConnectionStringSetting);
            builder.EntityPath = evenHubName;
            EventHubClient eventHubClient = EventHubClient.CreateFromConnectionString(builder.ToString());
            await eventHubClient.SendAsync(events);
        }

        /// <summary>
        /// Note: Creating event hubs has to be done at the ARM level which would require a service principal, so for now we'll just verify they exist and tell people to create them manually.
        /// This is unlike creating a Cosmos DB container, which can be done at the data-plane level with just a connection string
        /// </summary>
        public async static Task VerifyEventHubsExist()
        {
            EventHubsConnectionStringBuilder builder = new EventHubsConnectionStringBuilder(Constants.EventHubs.EventHubsConnectionStringSetting);

            var hubPrefixes = new string[]{
                Constants.EventHubs.Cardinality_One_Test.InputPrefix,
                Constants.EventHubs.Cardinality_One_Test.OutputPrefix,
                Constants.EventHubs.Json_Test.InputPrefix,
                Constants.EventHubs.Json_Test.OutputPrefix,
                Constants.EventHubs.String_Test.InputPrefix,
                Constants.EventHubs.String_Test.OutputPrefix,
            };

            var missingHubs = new List<string>();
            foreach (var hubPrefix in hubPrefixes)
            {
                builder.EntityPath = AzureHelpers.GetNameWithSuffix(hubPrefix);
                EventHubClient eventHubClient = eventHubClient = EventHubClient.CreateFromConnectionString(builder.ToString());
                try
                {
                    await eventHubClient.GetRuntimeInformationAsync();
                }
                catch
                {
                    missingHubs.Add(builder.EntityPath);
                }
                finally
                {
                    await eventHubClient.CloseAsync();
                }
            }

            if (missingHubs.Count > 0)
            {
                throw new Exception($"You must create the following event hubs before running e2e tests: \"{String.Join("\", \"", missingHubs)}\"");
            }
        }
    }
}