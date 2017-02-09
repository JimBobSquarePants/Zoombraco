// <copyright file="EmbeddedResourceHelper.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Resources
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Umbraco.Core;
    using Zoombraco.Controllers;

    /// <summary>
    /// Provides methods for retrieving embedded resources.
    /// </summary>
    internal class EmbeddedResourceHelper
    {
        /// <summary>
        /// Returns a value indicating whether the given resource exists.
        /// </summary>
        /// <param name="resource">The resource name.</param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ResourceExists(string resource)
        {
            // Sanitize the resource request.
            resource = SanitizeResourceName(resource);

            // Check this assembly.
            Assembly assembly = typeof(ZoombracoEmbeddedResourceController).Assembly;

            // Find the resource name; not case sensitive.
            string resourceName = assembly.GetManifestResourceNames().FirstOrDefault(r => r.InvariantEndsWith(resource));

            // Look for any embedded resources that have been added in inheriting code.
            if (string.IsNullOrWhiteSpace(resourceName))
            {
                // We need to loop through the loaded resources and check each one.
                Assembly localAssembly = assembly;
                IEnumerable<Type> externalResource = PluginManager.Current.ResolveTypes<IZoombracoEmbeddedResource>()
                                                                  .Where(t => t.Assembly != localAssembly);

                foreach (Type type in externalResource)
                {
                    assembly = type.Assembly;
                    resourceName = assembly.GetManifestResourceNames().FirstOrDefault(r => r.InvariantEndsWith(resource));

                    if (!string.IsNullOrWhiteSpace(resourceName))
                    {
                        return true;
                    }
                }
            }

            return !string.IsNullOrWhiteSpace(resourceName);
        }

        /// <summary>
        /// Gets a stream containing the content of the embedded resource.
        /// </summary>
        /// <param name="assembly">The assembly containing the resource.</param>
        /// <param name="resource">The path to the resource.</param>
        /// <param name="resourceName">The resource name.</param>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        public static Stream GetResource(Assembly assembly, string resource, out string resourceName)
        {
            // Sanitize the resource request.
            resource = SanitizeResourceName(resource);

            // Find the resource name; not case sensitive.
            resourceName = assembly.GetManifestResourceNames().FirstOrDefault(r => r.InvariantEndsWith(resource));

            if (resourceName != null)
            {
                return assembly.GetManifestResourceStream(resourceName);
            }

            return null;
        }

        /// <summary>
        /// Gets a sanitized name for an embedded resource.
        /// </summary>
        /// <param name="resource">The path to the resource.</param>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        public static string SanitizeResourceName(string resource)
        {
            string resourceRoot = ZoombracoConstants.EmbeddedResources.ResourceRoot;
            string extension = ZoombracoConstants.EmbeddedResources.ResourceExtension;

            if (resource.StartsWith(resourceRoot))
            {
                resource = resource.TrimStart(resourceRoot).Replace("/", ".").TrimEnd(extension);
            }
            else if (resource.EndsWith(extension))
            {
                resource = resource.TrimEnd(extension);
            }

            return resource;
        }
    }
}