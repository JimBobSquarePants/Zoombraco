// <copyright file="VersionParser.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using Semver;
    using Umbraco.Core;

    /// <summary>
    /// Provides methods that allow the parsing of version numbers.
    /// </summary>
    internal static class VersionParser
    {
        /// <summary>
        /// Gets the Zoombraco Assembly.
        /// </summary>
        public static Assembly Assembly { get; } = typeof(VersionParser).Assembly;

        /// <summary>
        /// Returns a Semver instance from the given string or <c>null</c> if it cannot be parsed.
        /// </summary>
        /// <param name="version">The string containing the version number.</param>
        /// <returns>The <see cref="SemVersion"/></returns>
        public static SemVersion FromSemanticString(string version)
        {
            Mandate.ParameterNotNull(version, nameof(version));

            Version currentVersion;
            string[] versionNumber = version.Split('-');
            string[] buildRelease = versionNumber.Length > 1 ? versionNumber[1].Split('+') : null;
            string prerelease = buildRelease?[0];
            string build = buildRelease != null && buildRelease.Length > 1 ? buildRelease[1] : null;
            if (Version.TryParse(versionNumber[0], out currentVersion))
            {
                return new SemVersion(
                    currentVersion.Major,
                    currentVersion.Minor,
                    currentVersion.Build,
                    prerelease,
                    build);
            }

            return null;
        }

        /// <summary>
        /// Gets the Zoombraco product version.
        /// </summary>
        /// <returns>The <see cref="SemVersion"/></returns>
        public static SemVersion ZoombracoProductVersion()
        {
            Assembly assembly = typeof(VersionParser).Assembly;

            // We're always loading from disk so this shouldn't be an issue.
            // ReSharper disable once AssignNullToNotNullAttribute
            return FromSemanticString(FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion);
        }
    }
}
