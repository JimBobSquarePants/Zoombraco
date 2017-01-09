// <copyright file="OpenGraphType.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    using System.ComponentModel.DataAnnotations;

    using Zoombraco.ComponentModel.Processors;

    /// <summary>
    /// Represent the type of document that is to be shared using the Open Graph protocol.
    /// <see href="http://ogp.me/"/>
    /// <remarks>
    /// To correctly display the enumeration as expected by the protocol in a document meta tag use
    /// the <see cref="M:Enum.ToDisplay"/> method.
    /// </remarks>
    /// </summary>
    [NuPickerEnum]
    public enum OpenGraphType
    {
        /// <summary>
        /// The document represents a website page.
        /// </summary>
        [Display(Name = "website")]
        Website,

        /// <summary>
        /// The document represents a news article.
        /// </summary>
        [Display(Name = "article")]
        Article,

        /// <summary>
        /// The document represents a book.
        /// </summary>
        [Display(Name = "book")]
        Book,

        /// <summary>
        /// The document represents a profile.
        /// </summary>
        [Display(Name = "profile")]
        Profile,

        /// <summary>
        /// The document represents a musical song.
        /// </summary>
        [Display(Name = "music.song")]
        MusicSong,

        /// <summary>
        /// The document represents a musical album.
        /// </summary>
        [Display(Name = "music.album")]
        MusicAlbum,

        /// <summary>
        /// The document represents a musical playlist.
        /// </summary>
        [Display(Name = "music.playlist")]
        MusicPlaylist,

        /// <summary>
        /// The document represents a musical radio station.
        /// </summary>
        [Display(Name = "music.radio_station")]
        MusicRadioStation,

        /// <summary>
        /// The document represents a video movie.
        /// </summary>
        [Display(Name = "video.movie")]
        VideoMovie,

        /// <summary>
        /// The document represents a multi-episode TV show.
        /// </summary>
        [Display(Name = "video.episode")]
        VideoEpisode,

        /// <summary>
        /// The document represents a video that doesn't belong in any other category.
        /// </summary>
        [Display(Name = "video.other")]
        VideoOther
    }
}