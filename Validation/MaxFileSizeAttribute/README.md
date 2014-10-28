Description
===
Simple MVC attribute for validating a file for some size limitation.

Use this attribute when the file is required to be not more than some value.


Usage
===
Server Validation
---
The maximum file size must be defined in bytes. There are two ways to set this value:

**1. Constant value**

Pass the max file size value directly
```sh
public class UploadFileModel
{
	[MaxFileSize(1024, ErrorMessage = "The {0} file size is limited to {1}.")]
	public HttpPostedFileBase File { get; set; }
}
```

**2. Using Web.config file**

Define the max file size in the Web.config <appSettings> section
```sh
<configuration>
	<appSettings>
		<add key="File.MaxSize" value="1024" />
	</appSettings>
	...
</configuration>
```

and pass the <appSettings> key to the attribute
```sh
public class UploadFileModel
{
	[MaxFileSize("File.MaxSize", ErrorMessage = "The {0} file size is limited to {1}.")]
	public HttpPostedFileBase File { get; set; }
}
```

**Format Units**

To display the user friendly file size limitation in the error message, you can specify the FormatUnit parameter.
The FormatUnit is detected automatically by default.

For instance this code:
```sh
[MaxFileSize(2621440, ErrorMessage = "The file size is limited to {1}.")]
public HttpPostedFileBase File { get; set; }
```
displays the error message: "The file size is limited to 2.5 Mb."

But you can set the FormatUnit, and it will be used for the formatting: 
```sh
[MaxFileSize(2097152, FormatUnit = Units.Kilobyte, ErrorMessage = "The file size is limited to {1}.")]
public HttpPostedFileBase File { get; set; }
```
Error message: "The file size is limited to 2048 Kb." (instead of 2 Mb)

Client Validation
---
The MaxFileSizeAttribute implements the *IClientValidatable* interface so it supports the javascript validation.
To use it reference a validate.attr.maxfilesize.js file in your view.

**Please note**: it depends on [jQuery Unobtrusive Validation]

[jQuery Unobtrusive Validation]: https://www.nuget.org/packages/Microsoft.jQuery.Unobtrusive.Validation/
