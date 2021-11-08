// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Azure.Functions.NodeJs.Tests.E2E
{
    public static class Constants
    {
        public static string FunctionsHostUrl = Environment.GetEnvironmentVariable("FunctionAppUrl") ?? "http://localhost:7071";

        //Queue tests
        public static class Queue {
            public static string StorageConnectionStringSetting = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            public static string OutputBindingPrefix = "test-output-";
            public static string InputBindingPrefix = "test-input-";
        }

        // CosmosDB tests
        public static class CosmosDB {
            public static string CosmosDBConnectionStringSetting = Environment.GetEnvironmentVariable("AzureWebJobsCosmosDBConnectionString");
            public static string DbName = "ItemDb";
            public static string InputCollectionPrefix = "ItemCollectionIn";
            public static string OutputCollectionPrefix = "ItemCollectionOut";
            public static string LeaseCollectionName = "leases";
        }

        // EventHubs
        public static class EventHubs {
            public static string EventHubsConnectionStringSetting = Environment.GetEnvironmentVariable("AzureWebJobsEventHubSender");

            public static class Json_Test {
                public static string OutputPrefix = "test-output-object-";
                public static string InputPrefix = "test-input-object-";
            }
            
            public static class String_Test {
                public static string OutputPrefix = "test-output-string-";
                public static string InputPrefix = "test-input-string-";
            }

            public static class Cardinality_One_Test {
                public static string InputPrefix = "test-input-one-";
                public static string OutputPrefix = "test-output-one-";
            }
        }

        // Xunit Fixtures and Collections
        public const string FunctionAppCollectionName = "FunctionAppCollection";
    }
}