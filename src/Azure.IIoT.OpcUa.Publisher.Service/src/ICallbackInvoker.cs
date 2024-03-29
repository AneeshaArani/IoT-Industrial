// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Publisher.Service
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Callback invoker
    /// </summary>
    /// <typeparam name="THub"></typeparam>
    public interface ICallbackInvoker<THub> : IDisposable
    {
        /// <summary>
        /// Call a method on a module or device identity with
        /// json payload.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="arguments"></param>
        /// <param name="ct"></param>
        /// <returns>response json payload</returns>
        Task BroadcastAsync(string method, object[] arguments,
            CancellationToken ct = default);

        /// <summary>
        /// Multicast message
        /// </summary>
        /// <param name="group"></param>
        /// <param name="method"></param>
        /// <param name="arguments"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task MulticastAsync(string group, string method, object[] arguments,
            CancellationToken ct = default);
    }
}
