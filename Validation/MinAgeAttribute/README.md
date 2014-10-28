Description
===
Simple MVC attribute for validating a user birth date for minimum age limitation.

Use this attribute when the user enters his birthday and his age must be more than some value.


Usage
===
Server Validation
---
```sh
public class UserModel
{
	[MinAge(18, ErrorMessage = "Users under {1} are not allowed")]
	public DateTime Birthday { get; set; }
}
```

Client Validation
---
The MinAgeAttribute implements the *IClientValidatable* interface so it supports the javascript validation.
To use it reference the validate.attr.minage.js file in your view.

**Please note**: it depends on [jQuery Unobtrusive Validation]

[jQuery Unobtrusive Validation]: https://www.nuget.org/packages/Microsoft.jQuery.Unobtrusive.Validation/
