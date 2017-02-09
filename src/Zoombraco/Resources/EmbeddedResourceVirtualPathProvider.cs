// <copyright file="EmbeddedResourceVirtualPathProvider.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Resources
{
    using ClientDependency.Core.CompositeFiles;

    using Umbraco.Core;

    /// <summary>
    /// The embedded resource virtual path provider.
    /// </summary>
    public sealed class EmbeddedResourceVirtualPathProvider : IVirtualFileProvider
    {
        /// <inheritdoc/>
        public bool FileExists(string virtualPath)
        {
            if (!virtualPath.InvariantEndsWith(ZoombracoConstants.EmbeddedResources.ResourceExtension))
            {
                return false;
            }

            return EmbeddedResourceHelper.ResourceExists(virtualPath);
        }

        /// <inheritdoc/>
        public IVirtualFile GetFile(string virtualPath)
        {
            if (!this.FileExists(virtualPath))
            {
                return null;
            }

            return new EmbeddedResourceVirtualFile(virtualPath);
        }
    }
}
