// <copyright file="IProtocolHandlerFactory.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Extensibility
{
    using System;

    /// <summary>
    /// Protocol Handler Factory.
    /// </summary>
    public interface IProtocolHandlerFactory
    {
        /// <summary>
        /// Gets the handler.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>An instance of the Handler.</returns>
        IProtocolHandler GetHandler(Uri uri);
    }
}