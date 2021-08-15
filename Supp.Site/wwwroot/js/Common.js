function DecodeUnicode(str) {
    str = decodeURIComponent(str);
    str = str.replace(new RegExp('/u0022', 'g'), '"');

    str = str.replace(new RegExp('/U00E0', 'g'), "à");
    str = str.replace(new RegExp('/U00E8', 'g'), "è");
    str = str.replace(new RegExp('/U00F2', 'g'), "ò");
    str = str.replace(new RegExp('/U00F9', 'g'), "ù");
    str = str.replace(new RegExp('/U00EC', 'g'), "ì");

    str = str.replace(new RegExp('/U00C0', 'g'), "à");
    str = str.replace(new RegExp('/U00C8', 'g'), "è");
    str = str.replace(new RegExp('/U00D2', 'g'), "ò");
    str = str.replace(new RegExp('/U00D9', 'g'), "ù");
    str = str.replace(new RegExp('/U00CC', 'g'), "ì");

    str = str.replace(new RegExp('/U0026', 'g'), "&");
    str = str.replace(new RegExp('/u0026', 'g'), "&");

    str = str.replace(new RegExp('/U0027', 'g'), "'");

    str = str.replace(new RegExp('/U00D', 'g'), " ");
    str = str.replace(new RegExp('/U00A', 'g'), " ");

    str = str.replace(new RegExp('/u00E0', 'g'), "à");
    str = str.replace(new RegExp('/u00E8', 'g'), "è");
    str = str.replace(new RegExp('/u00F2', 'g'), "ò");
    str = str.replace(new RegExp('/u00F9', 'g'), "ù");
    str = str.replace(new RegExp('/u00EC', 'g'), "ì");

    str = str.replace(new RegExp('/u00C0', 'g'), "à");
    str = str.replace(new RegExp('/u00C8', 'g'), "è");
    str = str.replace(new RegExp('/u00D2', 'g'), "ò");
    str = str.replace(new RegExp('/u00D9', 'g'), "ù");
    str = str.replace(new RegExp('/u00CC', 'g'), "ì");

    str = str.replace(new RegExp('/u0027', 'g'), "'");

    str = str.replace(new RegExp('/u00D', 'g'), " ");
    str = str.replace(new RegExp('/u00A', 'g'), " ");

    str = str.replace(new RegExp('/u002B', 'g'), "+");

    str = str.replace(new RegExp('/u00F1', 'g'), "ñ");

    return str;
}

function DecodeUnicodeExtended(str) {

    str = DecodeUnicode(str);

    str = str.replace(new RegExp('null', 'g'), "");

    return str;
}

function GetBoolean(value) {
    var result = false;
    if (value == '1' || value == 1 || value == "True" || value == "true" || value == true) result = true;
    return result;
}

function SendHttpRequest(method, url, json, funcName) {
    var xhr = new XMLHttpRequest();
    xhr.open(method, url, true);
    xhr.onload = function (e) {
        code = xhr.response;
        if (funcName != undefined && funcName != null && funcName != '') ExecuteFunction(funcName, code);
    };
    xhr.onerror = function () {
        console.error("** An error occurred during the XMLHttpRequest");
    };
    xhr.setRequestHeader("Content-type", "application/json");
    if (json != null && json != '') xhr.send(json);
    else xhr.send();
}

function IsMobile() {
    var result = false;
    if (/iPhone|iPad|iPod|Android|webOS|BlackBerry|Windows Phone/i.test(navigator.userAgent) || screen.availWidth <= 480) {
        result = true;
    }

    return result;
}

function ClearUrl() {
    var _url = window.location.toString();
    var splitUrl = _url.split('?');
    url = splitUrl[0];
    history.pushState({}, null, url);
}

function ExecuteFunction(funcName, par) {
    var funcCall = funcName;
    if (par != undefined && par != null && par != '') funcCall += "('" + par + "');";
    else funcCall += "();";

    var ret = eval(funcCall);
}

function Linebreak(s) {
    return s.replace(two_line, '<p></p>').replace(one_line, '<br>');
}

function Capitalize(s) {
    return s.replace(first_char, function (m) { return m.toUpperCase(); });
}

function FindInArrayOfObject(obj, fieldName, value) {
    var countLayer = obj.length;
    for (var x = 0; x < countLayer; x++) {
        if (obj[x][fieldName] === value) {
            return obj[x];
        }
    }

    return null;
}

function HexDecode(str) {
    str = str.replace(/&aacute;/g, "à");
    str = str.replace(/&eacute;/g, "è");
    str = str.replace(/&oacute;/g, "ò");
    str = str.replace(/&uacute;/g, "ù");
    str = str.replace(/&iacute;/g, "ì");

    str = str.replace(/&Aacute;/g, "à");
    str = str.replace(/&Eacute;/g, "è");
    str = str.replace(/&Oacute;/g, "ò");
    str = str.replace(/&Uacute;/g, "ù");
    str = str.replace(/&Iacute;/g, "ì");

    str = str.replace(/&#xE0;/g, "à");
    str = str.replace(/&#xE8;/g, "è");
    str = str.replace(/&#xF2;/g, "ò");
    str = str.replace(/&#xF9;/g, "ù");
    str = str.replace(/&#xEC;/g, "ì");

    str = str.replace(/&#xC0;/g, "à");
    str = str.replace(/&#xC8;/g, "è");
    str = str.replace(/&#xD2;/g, "ò");
    str = str.replace(/&#xD9;/g, "ù");
    str = str.replace(/&#xCC;/g, "ì");

    str = str.replace(/&#x27;/g, "'");

    str = str.replace(/&#xD;/g, " ");
    str = str.replace(/&#xA;/g, " ");

    return str;
}

function GenerateGuid() {
    var result, i, j;
    result = '';
    for (j = 0; j < 32; j++) {
        if (j == 8 || j == 12 || j == 16 || j == 20)
            result = result + '-';
        i = Math.floor(Math.random() * 16).toString(16).toUpperCase();
        result = result + i;
    }

    return result;
}