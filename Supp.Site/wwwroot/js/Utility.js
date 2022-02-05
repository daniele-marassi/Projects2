function GetCookie(cookieName) {
    try {
        var name = cookieName + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
    } catch (e) {

    }

    return null;
}

function SetCookie(name, value, expireDateString, path) {
    if (path == undefined || path == "") {
        path = "/";
    }
    var expires = "expires=" + expireDateString;
    document.cookie = name + "=" + value + "; " + expires + "; path=" + path;
}

function DeleteCookie(name, path) {
    if (path == undefined || path == "") {
        path = "/";
    }
    var expires = "expires=" + "Thu, 01 Jan 1970 00:00:00 GMT";
    document.cookie = name + "=" + "" + "; " + expires + "; path=" + path;
}

function ConvertCamelCase(str) {
    return InsertSpaces(str.replace(/(^| )(\w)/g, function (x) {
        return x.toUpperCase();
    }));
}

function InsertSpaces(string) {
    string = string.replace(/([a-z])([A-Z])/g, '$1 $2');
    string = string.replace(/([A-Z])([A-Z][a-z])/g, '$1 $2')
    return string;
}

function Details(buttonId) {
    document.activeElement.blur();
    var button = document.getElementById(buttonId);
    var div = document.getElementById(buttonId.replace("btn", "div"));

    if (button.value == '+') {
        button.value = '-';
        div.style.cssText = "height: auto; width: 600px; overflow:auto; display:block;";
    } else {
        button.value = '+';
        div.style.cssText = "height: 0px; width: 0px; overflow:hidden; display:none;";
    }
}