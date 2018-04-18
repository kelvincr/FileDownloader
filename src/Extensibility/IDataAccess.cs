// <copyright file="IDataAccess.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Extensibility
{
    using System;
    using System.Collections.Generic;
    using DataAccess;

    /// <summary>
    /// IData Access Interface, this interface expose main operations to database.
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        ///     Gets the files.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="count">The count.</param>
        /// <returns>A list of Files.</returns>
        IEnumerable<File> GetFiles(int index, int count);

        /// <summary>
        ///     Gets the full file.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Full File information</returns>
        FullFile GetFullFile(int index);

        /// <summary>
        ///     Stores the file.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="name">The name.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="size">The size.</param>
        /// <param name="date">The date.</param>
        /// <returns>The id of inserted File</returns>
        int StoreFile(string server, string name, string filePath, long size, DateTime date);

        /// <summary>
        ///     Ups the date file.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns>True whenever the update was successful.</returns>
        bool UpDateFile(int id, string status);
    }
}