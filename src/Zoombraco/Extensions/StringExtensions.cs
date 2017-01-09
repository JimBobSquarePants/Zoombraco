// <copyright file="StringExtensions.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Extensions
{
    using System;

    /// <summary>
    /// Extension methods for the <see cref="string"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a value indicating whether the string is a valid absolute URL.
        /// </summary>
        /// <param name="url">The string this method extends.</param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool IsAbsoluteUrl(this string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }
    }
}