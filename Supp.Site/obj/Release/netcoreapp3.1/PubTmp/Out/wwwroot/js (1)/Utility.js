function GetCookie(cookieName) {
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
    return "";
}

function SetCookie(name, value, expireDateString, path) {
    if (path == undefined || path == "") {
        path = "/";
    }
    var expires = "expires=" + expireDateString;
    document.cookie = name + "=" + value + "; " + expires + "; path=" + path;
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