﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace IIoTPlatformE2ETests.TestExtensions
{
    using Xunit;

    [CollectionDefinition("IIoT Standalone Direct Methods Test Collection", DisableParallelization = true)]
    public class IIoTStandaloneDirectMethodsTestCollection : ICollectionFixture<IIoTMultipleNodesTestContext>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}