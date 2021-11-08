// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Azure.Functions.NodeJs.Tests.E2E
{
    public static class AzureHelpers
    {
        public static string GetNameWithSuffix(string prefix)
        {
            return prefix + GetE2EResourceSuffix();
        }

        public static string GetE2EResourceSuffix()
        {
            var result = Environment.GetEnvironmentVariable("E2EResourceSuffix");
            if (String.IsNullOrWhiteSpace(result))
            {
                throw new Exception("You must define \"E2EResourceSuffix\" as an environment variable before running e2e tests.");
            }
            return result;
        }
    }
}