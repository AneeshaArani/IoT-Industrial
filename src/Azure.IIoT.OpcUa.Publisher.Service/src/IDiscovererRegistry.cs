// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Publisher.Service
{
    using Azure.IIoT.OpcUa.Publisher.Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Discoverer registry
    /// </summary>
    public interface IDiscovererRegistry
    {
        /// <summary>
        /// Get all discoverers in paged form
        /// </summary>
        /// <param name="continuation"></param>
        /// <param name="pageSize"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DiscovererListModel> ListDiscoverersAsync(
            string? continuation, int? pageSize = null,
            CancellationToken ct = default);

        /// <summary>
        /// Find discoverers using specific criterias.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageSize"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DiscovererListModel> QueryDiscoverersAsync(
            DiscovererQueryModel query, int? pageSize = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get discoverer registration by identifer.
        /// </summary>
        /// <param name="discovererId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DiscovererModel> GetDiscovererAsync(string discovererId,
            CancellationToken ct = default);

        /// <summary>
        /// Update discoverer, e.g. set discovery mode
        /// </summary>
        /// <param name="discovererId"></param>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task UpdateDiscovererAsync(string discovererId,
            DiscovererUpdateModel request,
            CancellationToken ct = default);
    }
}
