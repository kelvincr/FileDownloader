// <copyright file="FullFile.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace DataAccess
{
    /// <summary>
    ///     Full File Information.
    /// </summary>
    /// <seealso cref="DataAccess.File" />
    public class FullFile : File
    {
        /// <summary>
        ///     Gets or sets the local file path.
        /// </summary>
        /// <value>
        ///     The local file path.
        /// </value>
        public string LocalFilePath { get; set; }

        /// <summary>
        ///     Gets or sets the extension.
        /// </summary>
        /// <value>
        ///     The extension.
        /// </value>
        public string Extension { get; set; }
    }
}