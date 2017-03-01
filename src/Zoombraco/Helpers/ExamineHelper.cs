// <copyright file="ExamineHelper.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Helpers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Umbraco.Core;
    using Umbraco.Core.Logging;

    /// <summary>
    /// Provides methods that allow updating the examine indexers.
    /// </summary>
    internal class ExamineHelper
    {
        /// <summary>
        /// The full type declaration for the anaylzer.
        /// </summary>
        private static readonly string Analyzer = "Zoombraco.Search.ZoombracoMultiLingualAnalyzer, Zoombraco";

        /// <summary>
        /// The xml document
        /// </summary>
        private readonly XDocument xml;

        /// <summary>
        /// The fileastream
        /// </summary>
        private readonly Stream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamineHelper"/> class.
        /// </summary>
        /// <param name="stream">The stream containing the package information.</param>
        public ExamineHelper(Stream stream)
        {
            Mandate.ParameterNotNull(stream, nameof(stream));

            this.xml = XDocument.Load(stream);
            this.stream = stream;

            if (this.xml.Root == null || this.xml.Root.Name != "Examine")
            {
                LogHelper.Error<ExamineHelper>("Stream contains an invalid examine settings file.", new ArgumentException());
            }
        }

        /// <summary>
        /// Updates the External indexer with our provider
        /// </summary>
        public void UpdateExamineAnalyzer()
        {
            if (this.xml.Root == null)
            {
                return;
            }

            try
            {
                XElement element = this.xml.Root.Element("ExamineIndexProviders");
                if (element != null)
                {
                    this.UpdateSectionAnalyzer(element.Element("providers"), Constants.Examine.ExternalIndexer);
                }

                element = this.xml.Root.Element("ExamineSearchProviders");
                if (element != null)
                {
                    this.UpdateSectionAnalyzer(element.Element("providers"), Constants.Examine.ExternalSearcher);
                }

                // Clear out the contents of the file and overwrite.
                this.stream.SetLength(0);
                this.stream.Flush();
                this.xml.Save(this.stream);
            }
            catch (Exception e)
            {
                LogHelper.Error<ExamineHelper>("Unable to update External index analyzer.", e);
            }
        }

        /// <summary>
        /// Updates the provider analyzer
        /// </summary>
        /// <param name="element">The element containing the provider info.</param>
        /// <param name="name">The name of the provider to update.</param>
        private void UpdateSectionAnalyzer(XElement element, string name)
        {
            XElement external = element.Elements().First(e =>
            {
                XAttribute attribute = e.Attribute("name");
                return attribute != null && attribute.Value == name;
            });

            external.SetAttributeValue("analyzer", Analyzer);
        }
    }
}