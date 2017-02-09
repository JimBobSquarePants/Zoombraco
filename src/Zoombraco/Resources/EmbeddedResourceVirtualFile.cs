// <copyright file="EmbeddedResourceVirtualFile.cs" company="James Jackson-South">
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
    using ClientDependency.Core.CompositeFiles;
    using Umbraco.Core;
    using Zoombraco.Controllers;

    /// <summary>
    /// The embedded resource virtual file.
    /// </summary>
    internal class EmbeddedResourceVirtualFile : IVirtualFile
    {
        /// <summary>
        /// The virtual path to the resource.
        /// </summary>
        private readonly string virtualPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedResourceVirtualFile"/> class.
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path to the resource represented by this instance.
        /// </param>
        public EmbeddedResourceVirtualFile(string virtualPath)
        {
            this.virtualPath = virtualPath;
        }

        /// <summary>
        /// Gets the path to the virtual resource.
        /// </summary>
        public string Path => this.virtualPath;

        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>
        /// A read-only stream to the virtual file.
        /// </returns>
        public Stream Open()
        {
            string resourceName;

            // Get this assembly.
            Assembly assembly = typeof(ZoombracoEmbeddedResourceController).Assembly;
            Stream output = EmbeddedResourceHelper.GetResource(assembly, this.virtualPath, out resourceName);

            if (output == null)
            {
                // We need to loop through the loaded resources and check each one.
                Assembly localAssembly = assembly;
                IEnumerable<Type> externalResource = PluginManager.Current.ResolveTypes<IZoombracoEmbeddedResource>()
                                                                  .Where(t => t.Assembly != localAssembly);

                foreach (Type type in externalResource)
                {
                    string resource = EmbeddedResourceHelper.SanitizeResourceName(this.virtualPath);

                    assembly = type.Assembly;
                    resourceName = assembly.GetManifestResourceNames().FirstOrDefault(r => r.InvariantEndsWith(resource));

                    if (!string.IsNullOrWhiteSpace(resourceName))
                    {
                        return EmbeddedResourceHelper.GetResource(assembly, resource, out resourceName);
                    }
                }
            }

            return output;
        }
    }
}