// <copyright file="IRenderPage.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    using System.Globalization;

    /// <summary>
    /// Encapsulates properties required rendering pages with metadata.
    /// </summary>
    /// <typeparam name="T">
    /// The type of object to create the render model for.
    /// </typeparam>
    public interface IRenderPage<out T>
        where T : Page
    {
        /// <summary>
        /// Gets the content.
        /// </summary>
        T Content { get; }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        CultureInfo CurrentCulture { get; }
    }
}
