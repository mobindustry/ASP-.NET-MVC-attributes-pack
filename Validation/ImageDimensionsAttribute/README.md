Description
===
Simple MVC attribute for validating an uploaded image for exact dimensions (width & height).

Use this attribute when the image is required to be exactly W x H pixels.


Usage
===
There are two ways to define the width and height for the attribute:

**1. Constant values**

Pass width and height values directly
```sh
public class UploadImageModel
{
	[ImageDimensions(100, 100, ErrorMessage = "The {0} dimensions must be {1} x {2}.")]
	public HttpPostedFileBase Image { get; set; }
}
```

**2. Using Web.config file**

Define your dimensions in the Web.config <appSettings> section
```sh
<configuration>
	<appSettings>
		<add key="Image.Width" value="100" />
		<add key="Image.Height" value="100" />
	</appSettings>
	...
</configuration>
```

and pass the <appSettings> keys to the attribute
```sh
public class UploadImageModel
{
	[ImageDimensions("Image.Width", "Image.Height", ErrorMessage = "The {0} dimensions must be {1} x {2}.")]
	public HttpPostedFileBase Image { get; set; }
}
```

