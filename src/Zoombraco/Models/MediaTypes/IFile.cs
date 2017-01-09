// <copyright file="IFile.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    /// <summary>
    /// Represent a file in the media section.
    /// </summary>
    public interface IFile
    {
        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Gets or sets the size of the media file in bytes.
        /// </summary>
        int Bytes { get; set; }

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        string Extension { get; set; }
    }
}
