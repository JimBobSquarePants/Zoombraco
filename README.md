# <img src="build/assets/logo/zoombraco-64.png" width="52" height="52"/> Zoombraco

This repository contains a lean boilerplate for rapidly developing fast, scalable, easy to maintain, strong-typed Umbraco websites. 



**Zoombraco** is designed to be lean yet exceptionally powerful and flexible allowing you to create your own websites on a firm foundation. **Zoombraco** lets you concentrate on the fun features of your site while enabling you to maintain a strong MVC architecture.

## Features

 - Three strong-typed Base-Class-Types `Page`, `Component`, `NestedComponent`
 - Base Controllers, Views
 - Strong-typed `ContentHelper` for document traversal.
 - Automatic mapping of Nested Content
 - Automatic XML sitemap generation
 - Language aware Metadata
 - All properties can be made language aware and map automatically
 - Faceted, language aware search engine with fragment highlighting
 - Multitentant aware codebase
 - Image CDN url generation.

## Documentation

Documentation can be found via the [Wiki](https://github.com/JimBobSquarePants/Zoombraco/wiki) and is open for collaboration.

### Consuming the Solution

At the moment the codebase is there but I'm lacking a complete web demo. I'll get round to it though as I am writing documentation and an accompanying set of articles.

Nightlies are available from [MyGet](https://www.myget.org/gallery/zoombraco)

A sample Nuget.config file is as follows:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://www.nuget.org/api/v2/" />
    <add key="myget.ditto" value="https://www.myget.org/F/umbraco-ditto/" />
    <add key="myget.zoombraco" value="https://www.myget.org/F/zoombraco/api/v3/index.json"/>
  </packageSources>
</configuration>
```

Eventually there will be a Nuget package for consuming the library once the Ditto dependency is released.

Having a play around is encouraged, I'm always looking for feedback.

The demo website login details are:
 - Username : zoombraco
 - Password : password
