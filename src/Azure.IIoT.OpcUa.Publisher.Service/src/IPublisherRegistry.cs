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
    /// Publisher registry
    /// </summary>
    public interface IPublisherRegistry
    {
        /// <summary>
        /// Get all publishers in paged form
        /// </summary>
        /// <param name="continuation"></param>
        /// <param name="onlyServerState"></param>
        /// <param name="pageSize"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PublisherListModel> ListPublishersAsync(
            string? continuation, bool onlyServerState = false,
            int? pageSize = null, CancellationToken ct = default);

        /// <summary>
        /// Find publishers using specific criterias.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="onlyServerState"></param>
        /// <param name="pageSize"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PublisherListModel> QueryPublishersAsync(
            PublisherQueryModel query, bool onlyServerState = false,
            int? pageSize = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get publisher registration by identifer.
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="onlyServerState"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PublisherModel> GetPublisherAsync(
            string publisherId, bool onlyServerState = false,
            CancellationToken ct = default);

        /// <summary>
        /// Update publisher configuration
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task UpdatePublisherAsync(string publisherId,
            PublisherUpdateModel request,
            CancellationToken ct = default);
    }
}
