Cloudfiles Encoding Demo
========================

An ASP.NET MVC Demo to demonstrate [Encoding.com][] Video Processing integration with [Rackspace Cloud Files][].

### Getting Started

The project can be built with .NET 3.5 and ASP.NET MVC. The appSettings.config file must be updated for your specific account info.

Start the project and browse the following pages:

- Network Upload
	- Requires Silverlight for uploader
- Cloud Upload
	- Requires Silverlight for uploader
- Files
	- Will provide link to move network file to cloud
	- Trigger encoding process
	- Track encoding progress (requires browser localStorage (FF 3.6+))

### Included Components

A few libraries and components are included in the demo:

- [Com.Mosso.CloudFiles][] (Rackspace C# CloudFiles API)
- [Log4Net][]
- [Newtonsoft.Json.Net35][]
- CreateThe.MediaBlog.UI.SilverLight.xap (Fork of [Silverlight File Uploader][])

[Encoding.com]: http://encoding.com
[Rackspace Cloud Files]:http://www.rackspacecloud.com/cloud_hosting_products/files/
[Com.Mosso.CloudFiles]: https://github.com/rackspace/csharp-cloudfiles
[Log4Net]: http://logging.apache.org/log4net/
[Newtonsoft.Json.Net35]: http://james.newtonking.com/pages/json-net.aspx
[Silverlight File Uploader]: http://silverlightuploader.codeplex.com
