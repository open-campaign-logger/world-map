jQuery.validator.addMethod("mapfilesize",
    function (value, element, param) {

        return element.files[0].size < 10485760;

    });

jQuery.validator.unobtrusive.adapters.addBool("mapfilesize");

jQuery.validator.addMethod("mapfiletype",
    function (value, element, param) {

        var extensions = ['png', 'jpg', 'jpeg', 'gif', 'bmp'];
        var validExtension = false;
        extensions.forEach(function (extension) {
            if (value.toLowerCase().endsWith(extension)) {
                validExtension = true;
            }
        });
        return validExtension;

    });

jQuery.validator.unobtrusive.adapters.addBool("mapfiletype");

