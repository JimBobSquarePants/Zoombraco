// <copyright file="ImageExtensions.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    using System;
    using System.Collections.Specialized;
    using System.Net;
    using System.Web;

    using Umbraco.Core.Logging;

    using Zoombraco.Caching;

    /// <summary>
    /// Provides extension methods for the <see cref="Image"/> class.
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Gets the url to the image.
        /// </summary>
        /// <param name="image">The <see cref="Image"/> this method extends.</param>
        /// <returns>
        /// The <see cref="string"/> representing the url.
        /// </returns>
        public static string Url(this Image image)
        {
            return image.Crops.Src;
        }

        /// <summary>
        /// Gets the absolute url to the image.
        /// </summary>
        /// <param name="image">The <see cref="Image"/> this method extends.</param>
        /// <returns>
        /// The <see cref="string"/> representing the url.
        /// </returns>
        public static string UrlAbsolute(this Image image)
        {
            string root = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            return new Uri(new Uri(root, UriKind.Absolute), image.Crops.Src).ToString();
        }

        /// <summary>
        /// Gets the ImageProcessor Url by the crop alias (from the "umbracoFile" property alias)
        /// on the <see cref="Image"/> item.
        /// </summary>
        /// <param name="image">The <see cref="Image"/> this method extends.</param>
        /// <param name="alias">The crop alias <example>thumbnail</example>.</param>
        /// <param name="useCropDimensions">Whether to use the the crop dimensions to retrieve the url.</param>
        /// <param name="useFocalPoint">Whether to use the focal point.</param>
        /// <param name="quality">The quality of jpeg images.</param>
        /// <param name="parameters">Any additional querystring parameters we need to add or replace.</param>
        /// <returns>The <see cref="ImageProcessor.Web"/> Url. </returns>
        public static string GetCropUrl(this Image image, string alias, bool useCropDimensions = true, bool useFocalPoint = true, int quality = 85, NameValueCollection parameters = null)
        {
            string url = $"{image.Crops.Src}{image.Crops.GetCropUrl(alias, useCropDimensions, useFocalPoint) + "&quality=" + quality}";

            // Replace any parameters that we need to or add new.
            if (parameters != null)
            {
                string[] split = url.Split('?');
                url = split[0];
                NameValueCollection query = HttpUtility.ParseQueryString(split[1]);

                foreach (string key in parameters)
                {
                    query[key] = parameters[key];
                }

                url = $"{url}?{query}";
            }

            return url;
        }

        /// <summary>
        /// Gets the ImageProcessor Url from the CDN by the crop alias (from the "umbracoFile" property alias)
        /// on the <see cref="Image"/> item.
        /// </summary>
        /// <param name="image">The <see cref="Image"/> this method extends.</param>
        /// <param name="alias">The crop alias <example>thumbnail</example>.</param>
        /// <param name="useCropDimensions">Whether to use the the crop dimensions to retrieve the url.</param>
        /// <param name="useFocalPoint">Whether to use the focal point.</param>
        /// <param name="quality">The quality of jpeg images.</param>
        /// <param name="parameters">Any additional querystring parameters we need to add or replace.</param>
        /// <returns>The <see cref="ImageProcessor.Web"/> Url. </returns>
        public static string GetCdnCropUrl(this Image image, string alias, bool useCropDimensions = true, bool useFocalPoint = true, int quality = 85, NameValueCollection parameters = null)
        {
            if (HttpContext.Current == null)
            {
                return GetCropUrl(image, alias, useCropDimensions, useFocalPoint, quality, parameters);
            }

            // Piece together the full qualified url. We use this to make a head requests that ImageProcessor.Web will intercept and
            // route to the correct location. We'll cache that for subsequent requests.
            string domain = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            string cropUrl = GetCropUrl(image, alias, useCropDimensions, useFocalPoint, quality, parameters);
            string key = $"{domain}{cropUrl}";
            string result = cropUrl;
            int timeout = ZoombracoConfiguration.Instance.ImageCdnRequestTimeout;

            // We don't want timeouts or errors causing constant slowdown of the page rendering so we always cache either the CDN or non CDN result.
            // We do, however log extensively so that we can see why things are going wrong.
            return (string)ZoombracoApplicationCache.GetOrAddItem(
                key,
                () =>
                {
                    HttpWebResponse response = null;
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(key);
                        request.Method = "HEAD";
                        request.Timeout = timeout;

                        response = (HttpWebResponse)request.GetResponse();
                        HttpStatusCode responseCode = response.StatusCode;
                        if (responseCode == HttpStatusCode.OK)
                        {
                            result = response.ResponseUri.AbsoluteUri;
                        }
                    }
                    catch (WebException ex)
                    {
                        // If you are testing against localhost then this will happen.
                        if (ex.Response == null)
                        {
                            if (ex.Status == WebExceptionStatus.Timeout)
                            {
                                // Warn but about the timeout.
                                LogHelper.Warn<Image>($"CDN url request for {key} timed out. Consider adjusting the {ZoombracoConstants.Configuration.ImageCdnRequestTimeout} setting in the Web.Config.");
                            }

                            return GetCropUrl(image, alias, useCropDimensions, useFocalPoint, quality, parameters);
                        }

                        // ImageProcessor.Web will handle returning a response from the cache catering for different
                        // error codes from the cache providers. We'll just check for a url.
                        response = (HttpWebResponse)ex.Response;
                        if (response.ResponseUri != null)
                        {
                            result = response.ResponseUri.AbsoluteUri;
                        }
                        else
                        {
                            // Something is terribly wrong here.
                            LogHelper.Error<Image>($"Cannot get CDN url for {key}.", ex);
                        }
                    }
                    finally
                    {
                        response?.Dispose();
                    }

                    return result;
                });
        }
    }
}