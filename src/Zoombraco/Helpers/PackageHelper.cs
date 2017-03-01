// <copyright file="PackageHelper.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using Umbraco.Core;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Services;

    /// <summary>
    /// Provides methods that allow the unpacking of packages within an Umbraco application.
    /// </summary>
    internal class PackageHelper
    {
        /// <summary>
        /// The packaging service
        /// </summary>
        private readonly IPackagingService packagingService;

        /// <summary>
        /// The file service
        /// </summary>
        private readonly IFileService fileService;

        /// <summary>
        /// The xml document
        /// </summary>
        private readonly XDocument xml;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageHelper"/> class.
        /// </summary>
        /// <param name="stream">The stream containing the package information.</param>
        public PackageHelper(Stream stream)
            : this(stream, ApplicationContext.Current)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageHelper"/> class.
        /// </summary>
        /// <param name="stream">The stream containing the package information.</param>
        /// <param name="applicationContext">The Umbraco application context.</param>
        public PackageHelper(Stream stream, ApplicationContext applicationContext)
            : this(stream, applicationContext.Services.PackagingService, applicationContext.Services.FileService)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageHelper"/> class.
        /// </summary>
        /// <param name="stream">The stream containing the package information.</param>
        /// <param name="packagingService">The packaging service.</param>
        /// <param name="fileService">The content type service.</param>
        public PackageHelper(Stream stream, IPackagingService packagingService, IFileService fileService)
        {
            Mandate.ParameterNotNull(stream, nameof(stream));
            Mandate.ParameterNotNull(packagingService, nameof(packagingService));
            Mandate.ParameterNotNull(fileService, nameof(fileService));

            this.packagingService = packagingService;
            this.fileService = fileService;
            this.xml = XDocument.Load(stream);

            if (this.xml.Root == null || this.xml.Root.Name != Constants.Packaging.UmbPackageNodeName)
            {
                LogHelper.Error<PackageHelper>("Stream contains an invalid package file.", new ArgumentException());
            }
        }

        /// <summary>
        /// Unpacks the given package.
        /// </summary>
        public void Unpack()
        {
            if (this.xml.Root == null)
            {
                return;
            }

            XElement info = this.xml.Root.Element(Constants.Packaging.InfoNodeName);
            if (info != null)
            {
                this.LogPackageInfo(info);
            }

            XElement element = this.xml.Root.Element(Constants.Packaging.DataTypesNodeName);
            if (element != null)
            {
                this.UnpackDataTypes(element.Elements(Constants.Packaging.DataTypeNodeName));
            }

            element = this.xml.Root.Element(Constants.Packaging.TemplatesNodeName);
            if (element != null)
            {
                this.UnpackTemplates(element.Elements(Constants.Packaging.TemplateNodeName));
            }

            element = this.xml.Root.Element(Constants.Packaging.DocumentTypesNodeName);
            if (element != null)
            {
                this.UnpackDocumentTypes(element.Elements(Constants.Packaging.DocumentTypeNodeName));
            }
        }

        /// <summary>
        /// Unpacks a file to the correct location.
        /// </summary>
        /// <param name="fileStream">The stream containing the file contents.</param>
        /// <param name="fileName">The file name.</param>
        public void UnpackFile(Stream fileStream, string fileName)
        {
            Mandate.ParameterNotNull(fileStream, nameof(fileStream));

            XElement element = this.xml.Root?.Element(Constants.Packaging.FilesNodeName);

            if (element == null)
            {
                return;
            }

            foreach (XElement file in element.Elements(Constants.Packaging.FileNodeName))
            {
                string name = (string)file.Element(Constants.Packaging.OrgNameNodeName);

                if (!name.InvariantEquals(fileName))
                {
                    continue;
                }

                this.Log($"Importing File '{name}'");
                string root = IOHelper.GetRootDirectorySafe();
                string origin = ((string)file.Element(Constants.Packaging.OrgPathNodeName)).TrimStart("/");
                string directory = Path.Combine(root, origin);
                string path = Path.Combine(directory, name);

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (FileStream outStream = File.Create(path))
                {
                    fileStream.CopyTo(outStream);
                }
            }
        }

        /// <summary>
        /// Unpacks the data types.
        /// </summary>
        /// <param name="elements">The collection of elements.</param>
        private void UnpackDataTypes(IEnumerable<XElement> elements)
        {
            foreach (XElement element in elements)
            {
                string name = (string)element.Attribute(Constants.Packaging.NameNodeName);
                this.Log($"Importing DataType '{name}'");
                this.packagingService.ImportDataTypeDefinitions(new XElement(Constants.Packaging.DataTypesNodeName, element));
            }
        }

        /// <summary>
        /// Unpacks the templates.
        /// </summary>
        /// <param name="elements">The collection of elements.</param>
        private void UnpackTemplates(IEnumerable<XElement> elements)
        {
            foreach (XElement element in elements)
            {
                string name = (string)element.Element(Constants.Packaging.NameNodeName);
                this.Log($"Importing Template '{name}'");
                this.fileService.SaveTemplate(this.packagingService.ImportTemplates(element));
            }
        }

        /// <summary>
        /// Unpacks the document types.
        /// </summary>
        /// <param name="elements">The collection of elements.</param>
        private void UnpackDocumentTypes(IEnumerable<XElement> elements)
        {
            foreach (XElement element in elements)
            {
                // ReSharper disable once PossibleNullReferenceException
                string name = (string)element.Element("Info").Element(Constants.Packaging.NameNodeName);
                this.Log($"Importing ContentType '{name}'");
                this.packagingService.ImportContentTypes(new XElement(Constants.Packaging.DocumentTypesNodeName, element));
            }
        }

        /// <summary>
        /// Logs the package information to the Umbraco log.
        /// </summary>
        /// <param name="info">The element containing the package info.</param>
        private void LogPackageInfo(XElement info)
        {
            XElement pkg = info.Element("package");

            if (pkg == null)
            {
                return;
            }

            string name = (string)pkg.Element("name");
            string version = (string)pkg.Element("version");
            this.Log($"Installing package {name} at v{version}");
        }

        /// <summary>
        /// Logs the given message to the Umbraco log.
        /// </summary>
        /// <param name="message">The message to log.</param>
        private void Log(string message)
        {
            LogHelper.Info<PackageHelper>(message);
        }
    }
}