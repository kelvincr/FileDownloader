// <copyright file="CompletedState.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Extensibility
{
    /// <summary>
    ///     Downloaded completed states
    /// </summary>
    public enum CompletedState
    {
        /// <summary>
        ///     Non started
        /// </summary>
        NonStarted,

        /// <summary>
        ///     Download successful
        /// </summary>
        Succeeded,

        /// <summary>
        ///     Download partial
        /// </summary>
        Partial,

        /// <summary>
        ///     Download canceled
        /// </summary>
        Canceled,

        /// <summary>
        ///     Download failed
        /// </summary>
        Failed
    }
}