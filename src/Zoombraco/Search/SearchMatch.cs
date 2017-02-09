// <copyright file="SearchMatch.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Search
{
    using System;
    using System.Web;
    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Web;
    using Umbraco.Web.Models;
    using Zoombraco.Extensions;

    /// <summary>
    /// Represents a single search match.
    /// </summary>
    public class SearchMatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchMatch"/> class.
        /// </summary>
        /// <param name="id">The id of the content this result represents.</param>
        /// <param name="helper">The Umbraco helper for querying content.</param>
        /// <param name="highlightText">The highlight text displaying search matches.</param>
        /// <param name="categories">The categories within which the search match falls.</param>
        internal SearchMatch(int id, string highlightText, UmbracoHelper helper, string[] categories)
        {
            this.Id = id;
            this.HighlightText = new HtmlString(highlightText);
            this.Categories = categories;
            this.Node = this.GetNode(id, helper);

            if (this.Node != null)
            {
                this.Url = this.GetUrl(this.Node);
                this.UrlAbsolute = this.GetUrlAbsolute(this.Url);
            }
        }

        /// <summary>
        /// Gets the id for this result.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the highlight text for this result.
        /// </summary>
        public HtmlString HighlightText { get; private set; }

        /// <summary>
        /// Gets the name for this result.
        /// </summary>
        public string Name => this.Node != null ? this.Node.Name : string.Empty;

        /// <summary>
        /// Gets or sets the categories within which the search match falls.
        /// </summary>
        public string[] Categories { get; set; }

        /// <summary>
        /// Gets the <see cref="IPublishedContent"/> for the search match.
        /// </summary>
        public IPublishedContent Node { get; }

        /// <summary>
        /// Gets the url for this result.
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Gets the absolute url for this result.
        /// </summary>
        public string UrlAbsolute { get; }

        /// <summary>
        /// Gets the raw published content.
        /// </summary>
        /// <param name="id">The id of the node to get.</param>
        /// <param name="helper">The Umbraco helper for querying content.</param>
        /// <returns>The <see cref="IPublishedContent"/>.</returns>
        private IPublishedContent GetNode(int id, UmbracoHelper helper)
        {
            UmbracoObjectTypes objectType = UmbracoObjectTypes.Unknown;

            IPublishedContent content = this.GetPublishedContent(id, ref objectType, UmbracoObjectTypes.Document, helper.TypedContent)
                 ?? this.GetPublishedContent(id, ref objectType, UmbracoObjectTypes.Media, helper.TypedMedia)
                 ?? this.GetPublishedContent(id, ref objectType, UmbracoObjectTypes.Member, helper.TypedMember);

            return content;
        }

        /// <summary>
        /// Attempt to get an <see cref="IPublishedContent"/> instance based on id and object type.
        /// </summary>
        /// <param name="nodeId">The content node ID</param>
        /// <param name="actual">The type of content being requested</param>
        /// <param name="expected">The type of content expected/supported by <paramref name="typedMethod"/></param>
        /// <param name="typedMethod">A function to fetch content of type <paramref name="expected"/></param>
        /// <returns>
        /// The requested content, or null if either it does not exist or <paramref name="actual"/> does not
        /// match <paramref name="expected"/>
        /// </returns>
        private IPublishedContent GetPublishedContent(int nodeId, ref UmbracoObjectTypes actual, UmbracoObjectTypes expected, Func<int, IPublishedContent> typedMethod)
        {
            // Is the given type supported by the typed method.
            if (actual != UmbracoObjectTypes.Unknown && actual != expected)
            {
                return null;
            }

            // Attempt to get the content
            IPublishedContent content = typedMethod(nodeId);
            if (content != null)
            {
                // If we find the content, assign the expected type to the actual type so we don't have to
                // keep looking for other types of content.
                actual = expected;
            }

            return content;
        }

        /// <summary>
        /// Gets the url for the given node.
        /// </summary>
        /// <param name="content">The cached content.</param>
        /// <returns>The <see cref="string"/></returns>
        private string GetUrl(IPublishedContent content)
        {
            if (content.HasProperty(Constants.Conventions.Media.File))
            {
                ImageCropDataSet crops = content.GetPropertyValue<ImageCropDataSet>(Constants.Conventions.Media.File);

                if (crops != null)
                {
                    return crops.Src;
                }
            }

            return content.Url;
        }

        /// <summary>
        /// Gets the absolute url for the given url.
        /// </summary>
        /// <param name="url">The url to return the absolute url for.</param>
        /// <returns>The <see cref="string"/></returns>
        private string GetUrlAbsolute(string url)
        {
            string uri = url;

            if (string.IsNullOrWhiteSpace(uri))
            {
                return uri;
            }

            // Hackathon!
            // Certain virtual pages such as Articulate blog pages are only returning a relative url.
            if (!uri.IsAbsoluteUrl())
            {
                string root = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                uri = new Uri(new Uri(root, UriKind.Absolute), uri).ToString();
            }

            return uri;
        }
    }
}