# <img src="build/assets/logo/zoombraco-64.png" width="52" height="52"/> Zoombraco

This repository contains a lean boilerplate for rapidly developing fast, scalable, easy to maintain, strong-typed Umbraco websites. 

Imagine being able to write code like this to produce Umbraco sites...

**Demo Strong-typed Model**
```
/// <summary>
/// The generic page document type
/// </summary>
public class Generic : Page, IHeroPanel, INested
{
    /// <inheritdoc />
    public virtual IEnumerable<Image> HeroImages { get; set; }

    /// <inheritdoc />
    public virtual string HeroTitle { get; set; }

    /// <inheritdoc />
    public virtual RelatedLink HeroLink { get; set; }

    /// <inheritdoc />
    [UmbracoSearchMergedField]
    [NestedRichTextSearchResolver]
    public virtual IEnumerable<NestedComponent> NestedContent { get; set; }
}
```

**Demo Controller ActionResult**
```
/// <summary>
/// The Generic page controller
/// </summary>
[UmbracoOutputCache]
public class GenericController : ZoombracoController
{
    /// <inheritdoc />
    public override ActionResult Index(RenderModel model)
    {
        Generic generic = model.As<Generic>();
        RenderPage<Generic> viewModel = new RenderPage<Generic>(generic);

        return this.CurrentTemplate(viewModel);
    }
}
```

**Zoombraco** will let you do that and a lot more. The boilerplate is designed to be lean yet exceptionally powerful and flexible allowing you to create your own websites on a firm foundation.

## Features

 - Three strong-typed Base-Class-Types Page, Component, NestedComponent
 - Strong-typed ContentHelper for document traversal.
 - Base Controllers, Views
 - Automatic mapping of Nested Content
 - Metadata and automatic XML sitemap generation
 - Language aware Metadata
 - All properties can be made language aware and map automatically
 - Faceted, language aware search engine with fragment highlighting
 - Multitentant aware codebase

### Consuming the Solution

At the moment the codebase is there but I'm lacking a complete demo. I'll get round to it though as I am writing an accompanying set of articles.

Eventually there will be a Nuget package for consuming the library.

Having a play around is encouraged, I'm always looking for feedback.

The demo website login details are:
 - Username : zoombraco
 - Password : password
