  ______                     _
 |___  /                    | |
    / / ___   ___  _ __ ___ | |__  _ __ __ _  ___ ___
   / / / _ \ / _ \| '_ ` _ \| '_ \| '__/ _` |/ __/ _ \
  / /_| (_) | (_) | | | | | | |_) | | | (_| | (_| (_) |
 /_____\___/ \___/|_| |_| |_|_.__/|_|  \__,_|\___\___/

 ======================================================

 Dont forget to build!



 Zoombraco will install automatically on application start once Umbraco itself is installed.

 Once the package has been installed you will see the following application settings added to the web.config:


 <!--The currently installed Zoombraco version.-->
 <add key="Zoombraco:Version" value="0.5.0" />

 <!--The amound of time in seconds to cache the output for-->
 <add key="Zoombraco:OutputCacheDuration" value="0" />

 <!--The amount of time in milliseconds to wait to before requesting an ImageProcessor url from the CDN.-->
 <add key="Zoombraco:ImageCdnRequestTimeout" value="1000" />


 Additionally the existing value will be set to false to prevent compatibility issues with Ditto.
 
 <add key="Umbraco.ModelsBuilder.Enable" value="false" />



 To complete installation you now need to set your Global.asax file to inherit Zoombraco.ZoombracoGlobal as follows:
 
 <%@ Application Inherits="Zoombraco.ZoombracoGlobal" Language="C#" %>