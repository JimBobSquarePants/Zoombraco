# <img src="build/assets/logo/zoombraco-64.png" width="52" height="52"/> Zoombraco

This repository contains a lean boilerplate for rapidly developing fast, scalable, easy to maintain, strong-typed Umbraco websites. 

At the moment the codebase is there but I'm lacking a proper demo. I'll get round to it though as I am writing an accompanying set of articles.

Eventually there will be a Nuget package for consuming the library.

### Playing with the Solution

To enable package restore to work for Nuget you will be required to add the following lines to your Nuget.config file which you should add to the root of your solution. See this [blog post](http://blog.davidebbo.com/2014/01/the-right-way-to-restore-nuget-packages.html) for more information. 

	<?xml version="1.0" encoding="utf-8"?>
	<configuration>
	  <packageSources>
		<add key="nuget.org" value="https://www.nuget.org/api/v2/" />
		<add key="myget.ditto" value="https://www.myget.org/F/umbraco-ditto/" />
	  </packageSources>
	</configuration>

The demo website login details are:
 - Username : zoombraco
 - Password : password