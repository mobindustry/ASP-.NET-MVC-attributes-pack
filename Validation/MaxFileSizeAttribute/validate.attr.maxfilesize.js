
$.validator.unobtrusive.adapters.add(
	'maxfilesize', ['value'], function(options) {
		options.rules['maxfilesize'] = options.params;
		if (options.message) {
			options.messages['maxfilesize'] = options.message;
		}
	}
);

$.validator.addMethod('maxfilesize', function (value, element, params) {
	try {
		if (element.files && element.files.length < 1) {
			// No files selected
			return true;
		}

		if (!element.files || !element.files[0].size) {
			// This browser doesn't support the HTML5 API
			return true;
		}
		return element.files[0].size < params.value;

	} catch (e) {
		return true;
	}
}, '');