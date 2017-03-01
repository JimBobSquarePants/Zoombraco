// <copyright file="IOHelper.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Helpers
{
    using System;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Provides methods to help with filesystem actions.
    /// </summary>
    internal static class IOHelper
    {
        /// <summary>
        /// The root directory
        /// </summary>
        private static string rootDir;

        /// <summary>
        /// Returns the path to the root of the application, by getting the path to where the assembly where this
        /// method is included is present, then traversing until it's past the /bin directory. Ie. this makes it work
        /// even if the assembly is in a /bin/debug or /bin/release folder
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public static string GetRootDirectorySafe()
        {
            if (!string.IsNullOrEmpty(rootDir))
            {
                return rootDir;
            }

            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            Uri uri = new Uri(codeBase);
            string path = uri.LocalPath;
            string baseDirectory = Path.GetDirectoryName(path);

            if (string.IsNullOrEmpty(baseDirectory))
            {
                throw new Exception("No root directory could be resolved. Please ensure that your Umbraco solution is correctly configured.");
            }

            rootDir = baseDirectory.Contains("bin")
                           ? baseDirectory.Substring(0, baseDirectory.LastIndexOf("bin", StringComparison.OrdinalIgnoreCase) - 1)
                           : baseDirectory;

            return rootDir;
        }
    }
}
