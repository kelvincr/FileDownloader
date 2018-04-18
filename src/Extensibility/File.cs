// <copyright file="File.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace DataAccess
{
    /// <summary>
    ///     File POCO
    /// </summary>
    public class File
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the server.
        /// </summary>
        /// <value>
        ///     The server.
        /// </value>
        public string Server { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the size.
        /// </summary>
        /// <value>
        ///     The size.
        /// </value>
        public long Size { get; set; }

        /// <summary>
        ///     Gets or sets the date.
        /// </summary>
        /// <value>
        ///     The date.
        /// </value>
        public string Date { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>
        ///     The status.
        /// </value>
        public string Status { get; set; }
    }
}