// <copyright file="EmbeddedResourceWriter.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Resources
{
    using System;
    using System.IO;
    using System.Web;

    using ClientDependency.Core;
    using ClientDependency.Core.CompositeFiles;
    using ClientDependency.Core.CompositeFiles.Providers;

    /// <summary>
    /// The embedded resource writer.
    /// </summary>
    public sealed class EmbeddedResourceWriter : IVirtualFileWriter
    {
        /// <summary>
        /// Gets the file provider.
        /// </summary>
        public IVirtualFileProvider FileProvider => new EmbeddedResourceVirtualPathProvider();

        /// <inheritdoc/>
        public bool WriteToStream(
            BaseCompositeFileProcessingProvider provider,
            StreamWriter streamWriter,
            IVirtualFile virtualFile,
            ClientDependencyType type,
            string originalUrl,
            HttpContextBase context)
        {
            StreamReader streamReader = null;
            try
            {
                Stream readStream = virtualFile.Open();
                streamReader = new StreamReader(readStream);
                string output = streamReader.ReadToEnd();

                DefaultFileWriter.WriteContentToStream(provider, streamWriter, output, type, context, originalUrl);

                return true;
            }
            catch (Exception)
            {
                // The file must have failed to open
                return false;
            }
            finally
            {
                // readStream is disposed by the streamReader.
                streamReader?.Dispose();
            }
        }
    }
}
