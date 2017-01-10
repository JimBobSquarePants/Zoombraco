// <copyright file="RelatedLinksPropertyConverter.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.ComponentModel.PropertyValueConverters
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    using Umbraco.Core;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web;

    using Zoombraco.Models;

    /// <summary>
    /// The related links property value converter.
    /// <see href="https://github.com/Jeavon/Umbraco-Core-Property-Value-Converters"/>
    /// </summary>
    [PropertyValueType(typeof(IEnumerable<RelatedLink>))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.ContentCache)]
    public class RelatedLinksPropertyConverter : PropertyValueConverterBase
    {
        /// <inheritdoc/>
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.PropertyEditorAlias.Equals(Constants.PropertyEditors.RelatedLinksAlias);
        }

        /// <inheritdoc/>
        public override object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview)
        {
            if (source == null)
            {
                return null;
            }

            string sourceString = source.ToString();

            IEnumerable<RelatedLinkData> relatedLinksData = JsonConvert.DeserializeObject<IEnumerable<RelatedLinkData>>(sourceString);
            List<RelatedLink> relatedLinks = new List<RelatedLink>();

            foreach (RelatedLinkData linkData in relatedLinksData)
            {
                RelatedLink relatedLink = new RelatedLink
                {
                    Caption = linkData.Caption,
                    NewWindow = linkData.NewWindow,
                    IsInternal = linkData.IsInternal,
                    Type = linkData.Type,
                    Id = linkData.Internal,
                    Link = linkData.Link
                };

                relatedLink = this.CreateLink(relatedLink);

                if (!relatedLink.IsDeleted)
                {
                    relatedLinks.Add(relatedLink);
                }
                else
                {
                    LogHelper.Warn<RelatedLinksPropertyConverter>(
                        $"Related Links value converter skipped a link as the node has been unpublished/deleted (Internal Link NodeId: {relatedLink.Link}, Link Caption: \"{relatedLink.Caption}\")");
                }
            }

            return relatedLinks;
        }

        /// <summary>
        /// Augments a related link to provide additiona link data.
        /// </summary>
        /// <param name="link">The related linkj</param>
        /// <returns>The <see cref="RelatedLink"/></returns>
        private RelatedLink CreateLink(RelatedLink link)
        {
            if (link.IsInternal && link.Id != null)
            {
                if (UmbracoContext.Current == null)
                {
                    return null;
                }

                link.Link = UmbracoContext.Current.UrlProvider.GetUrl((int)link.Id);
                if (link.Link.Equals("#"))
                {
                    link.IsDeleted = true;
                    link.Link = link.Id.ToString();
                }
                else
                {
                    link.IsDeleted = false;
                }
            }

            return link;
        }
    }
}