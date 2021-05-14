var focusOnMenu = false;
var intvCloseSubMenu;

function GetCookie(name) {
    var _name = name + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(_name) == 0) {
            return c.substring(_name.length, c.length);
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

function FillValue(id, value) {
    document.getElementById(id).value = value;
}

function GetValue(id) {
    var value = "";
    try {
        value = document.getElementById(id).value;
    } catch (e) {

    }
    return value;
}

function Enable(id) {
    document.getElementById(id).disabled = false;
}

function Disable(id) {
    document.getElementById(id).disabled = true;
}

function ResetValues(name) {
    document.getElementsByName(name)[0].value = "";
}

function OpenSubMenu(id) {
    var parent = document.getElementById(id).parentElement;
    focusOnMenu = true;
    var startingLevel = parseInt(document.getElementById(id).getAttribute("name").split("_")[1]);
    HiddenOrDisplayAllElements(startingLevel, 0, parent, "block");
}

function CloseSubMenu() {
    clearInterval(intvCloseSubMenu);
    if (focusOnMenu == true) return; 
    var parent = document.getElementById("menu");
    var startingLevel = parseInt(parent.getAttribute("name").split("_")[1]);
    HiddenOrDisplayAllElements(startingLevel, 0, parent, "none");
}

function HiddenOrDisplayAllElements(startingLevel, levelCount, node, value) {
    for (var i = 0; i < node.childNodes.length; i++) {
        var child = node.childNodes[i];
        var level = null;
        if (child != undefined && child != null && child.id != undefined && child.id != null) {
            if (child.getAttribute("name") != undefined && child.getAttribute("name") != null) {
                level = parseInt(child.getAttribute("name").split("_")[1]);
                levelCount++;
            }
        }
        HiddenOrDisplayAllElements(startingLevel, levelCount, child, value);

        if (child != undefined && child != null && child.id != undefined && child.id != null) {
            if ((value == "none" && level > 1 && level != null) || (value == "block" && levelCount <= 2) || (value == "block" && (level == 1 ))) {
                child.style.display = value;
            }
        }
    }
}

function LostFocusMenu() {
    focusOnMenu = false;
    intvCloseSubMenu = setInterval(CloseSubMenu, 2000);
}

function InsertHtmlInDiv(id, html) {
    document.getElementById(id).innerHTML = html;
}

function CreateMenu(id, html) {
    InsertHtmlInDiv(id, html);
    InitToolTip();
    focusOnMenu = false;
    CloseSubMenu();
}

function ShowToolTip(id) {
    focusOnMenu = true;
    var _id = id.replace("item", "toolTip");
    //var left = ( document.getElementById("menu").offsetWidth) - 50;
    document.getElementById(_id).style.marginLeft = "3px";
    document.getElementById(_id).style.visibility = "visible";
    document.getElementById(_id).style.marginTop = "-" + (document.getElementById(_id).offsetHeight + 20) + "px";
}

function HiddenToolTip(id) {
    var _id = id.replace("item", "toolTip");
    document.getElementById(_id).style.visibility = "hidden";
}

function InitToolTip() {
    var toolTips = document.getElementsByName("toolTip");
    for (var i = 0; i < toolTips.length; i++) {
        ShowToolTip(toolTips[i].id);
        HiddenToolTip(toolTips[i].id);
    }
}

function LoadPage(id) {
    var location = document.getElementById(id).getAttribute("href");
    this.document.location.href = location;
}