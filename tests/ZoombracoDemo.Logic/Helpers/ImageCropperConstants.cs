// <copyright file="ImageCropperConstants.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Helpers
{
    /// <summary>
    /// The image cropper constants containing crop aliases.
    /// Alias' signify the default "mobile-first" viewport size. We default to 2X image dimensions with low quality output
    /// to take advatage of browser scaling on Hi-Res devices.
    /// Each alias has an M suffix equivalent in the CMS to designate medium+ viewports.
    /// </summary>
    public static class ImageCropperConstants
    {
        /// <summary>
        /// 960x270, 1920x540
        /// </summary>
        public const string HeroPanel = "HeroPanel";

        /// <summary>
        /// Add the suffix <value>M</value> to the given string.
        /// </summary>
        /// <param name="alias">The image crop alias.</param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToM(string alias)
        {
            return $"{alias}M";
        }
    }
}
