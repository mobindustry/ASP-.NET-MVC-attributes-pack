
$.validator.unobtrusive.adapters.add(
    'minage', ['value'], function(options) {
        options.rules['minage'] = options.params;
        if (options.message) {
            options.messages['minage'] = options.message;
        }
    }
);

$.validator.addMethod('minage', function (value, element, params) {
    try {
        var actualDate = new Date($(element).val());
        actualDate.setHours(0, 0, 0, 0);

        var maxDate = new Date();
        maxDate.setFullYear(maxDate.getFullYear() - params.value);
        maxDate.setHours(0, 0, 0, 0);

        if (maxDate < actualDate) {
            return false;
        }

        return true;

    } catch (e) {
        return true;
    }
}, '');