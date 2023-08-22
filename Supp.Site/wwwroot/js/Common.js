function DecodeUnicode(_value) {
    _value = decodeURIComponent(_value);
    _value = _value.replace(new RegExp('/u0022', 'g'), '"');

    _value = _value.replace(new RegExp('/U00E0', 'g'), "à");
    _value = _value.replace(new RegExp('/U00E8', 'g'), "è");
    _value = _value.replace(new RegExp('/U00F2', 'g'), "ò");
    _value = _value.replace(new RegExp('/U00F9', 'g'), "ù");
    _value = _value.replace(new RegExp('/U00EC', 'g'), "ì");

    _value = _value.replace(new RegExp('/U00C0', 'g'), "à");
    _value = _value.replace(new RegExp('/U00C8', 'g'), "è");
    _value = _value.replace(new RegExp('/U00D2', 'g'), "ò");
    _value = _value.replace(new RegExp('/U00D9', 'g'), "ù");
    _value = _value.replace(new RegExp('/U00CC', 'g'), "ì");

    _value = _value.replace(new RegExp('/U0026', 'g'), "&");
    _value = _value.replace(new RegExp('/u0026', 'g'), "&");

    _value = _value.replace(new RegExp('/U0027', 'g'), "'");

    _value = _value.replace(new RegExp('/U00D', 'g'), " ");
    _value = _value.replace(new RegExp('/U00A', 'g'), " ");

    _value = _value.replace(new RegExp('/u00E0', 'g'), "à");
    _value = _value.replace(new RegExp('/u00E8', 'g'), "è");
    _value = _value.replace(new RegExp('/u00F2', 'g'), "ò");
    _value = _value.replace(new RegExp('/u00F9', 'g'), "ù");
    _value = _value.replace(new RegExp('/u00EC', 'g'), "ì");

    _value = _value.replace(new RegExp('/u00C0', 'g'), "à");
    _value = _value.replace(new RegExp('/u00C8', 'g'), "è");
    _value = _value.replace(new RegExp('/u00D2', 'g'), "ò");
    _value = _value.replace(new RegExp('/u00D9', 'g'), "ù");
    _value = _value.replace(new RegExp('/u00CC', 'g'), "ì");

    _value = _value.replace(new RegExp('/u0027', 'g'), "'");

    _value = _value.replace(new RegExp('/u00D', 'g'), " ");
    _value = _value.replace(new RegExp('/u00A', 'g'), " ");

    _value = _value.replace(new RegExp('/u002B', 'g'), "+");

    _value = _value.replace(new RegExp('/u00F1', 'g'), "ñ");

    return _value;
}

function DecodeUnicodeExtended(_value) {

    _value = DecodeUnicode(_value);

    _value = _value.replace(new RegExp('null', 'g'), "");

    return _value;
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

function MatchPhrase(_phrase, webSpeechlist, _claims) {
    var _data = null;
    var result = { Data: null, WebSpeechKeysMatched: null };
    result.Data = null;
    result.WebSpeechKeysMatched = null;
    var minMatch = 0;
    var countMatch = 0;
    var phraseMatch = "";
    var previousCountMatch = 0;
    var founded = false;

    webSpeechlist.forEach(
        function (item) {
            try {
                var keysInObjectList = JSON.parse(item.Phrase);

                minMatch = keysInObjectList.length;
                countMatch = 0;
                phraseMatch = "";

                keysInObjectList.forEach(
                    function (keysInObject) {
                        var keysArray = [];

                        if (Array.isArray(keysInObject)) {
                            keysArray = keysInObject;
                        }
                        else {
                            keysArray.push(keysInObject);
                        }

                        founded = false;
                        keysArray.forEach(
                            function (key) {
                                var keys = key.toString().trim().toLowerCase().split(' ');
                                if (_phrase == null) _phrase = "";

                                if (keys.length == 1) {
                                    var words = _phrase.trim().toLowerCase().split(' ');

                                    if (!founded && words.includes(key.toString().trim().toLowerCase())) {
                                        founded = true;
                                        countMatch++;
                                        if (phraseMatch != "") phraseMatch += " ";
                                        phraseMatch += key.toString();
                                    }
                                }
                                else {
                                    if (!founded) {
                                        var matchPrhareNoKeysResult = MatchPrhareNoKeys(key.toString().trim().toLowerCase(), _phrase, minMatch, countMatch, phraseMatch, previousCountMatch);
                                        minMatch = matchPrhareNoKeysResult.MinMatch;
                                        countMatch = matchPrhareNoKeysResult.CountMatch;
                                        phraseMatch = matchPrhareNoKeysResult.PhraseMatch;
                                        founded = matchPrhareNoKeysResult.Founded;
                                        previousCountMatch = matchPrhareNoKeysResult.PreviousCountMatch;
                                    }
                                }
                            }
                        );
                    }
                );
            } catch (error) {
                var matchPrhareNoKeysResult = MatchPrhareNoKeys(item.Phrase, _phrase, minMatch, countMatch, phraseMatch, previousCountMatch);
                minMatch = matchPrhareNoKeysResult.MinMatch;
                countMatch = matchPrhareNoKeysResult.CountMatch;
                phraseMatch = matchPrhareNoKeysResult.PhraseMatch;
                previousCountMatch = matchPrhareNoKeysResult.PreviousCountMatch;
            }

            if (countMatch >= minMatch && countMatch > previousCountMatch) {
                previousCountMatch = countMatch;
                _data = item;
                result.WebSpeechKeysMatched = phraseMatch;
                countMatch = 0;
            }
        }
    );

    if (_data != null) {
        result.Data = GetAnswer(_data, _claims);
    }

    return result;
}

function MatchPrhareNoKeys(itemPhrase, _phrase, minMatch, countMatch, phraseMatch, previousCountMatch) {
    var result = { MinMatch: 0, CountMatch: 0, PhraseMatch: null, Founded: false, PreviousCountMatch: 0 };
    result.MinMatch = minMatch;
    result.CountMatch = countMatch;
    result.PhraseMatch = phraseMatch;
    result.Founded = false;
    result.PreviousCountMatch = previousCountMatch;

    var _countMatch = 0;
    var _phraseMatch = "";
    var _minMatch = 0;
    var founded = false;

    try {
        var keysArray = itemPhrase.split(' ');
        _minMatch = keysArray.length;
        if (_phrase == null) _phrase = "";

        keysArray.forEach(
            function (key) {
                if (_phrase.trim().toLowerCase().includes(key.toString().trim().toLowerCase())) {
                    founded = true;
                    _countMatch++;
                    if (_phraseMatch != "") _phraseMatch += " ";
                    _phraseMatch += key.toString();
                }
            }
        );
    }
    catch (error) {
    }

    if (_countMatch >= result.MinMatch && _countMatch > result.PreviousCountMatch) {
        previousCountMatch = countMatch;
        result.MinMatch = _minMatch;
        result.CountMatch += _countMatch;
        result.PhraseMatch += _phraseMatch;
        result.Founded = founded;
        result.PreviousCountMatch = previousCountMatch;
    }

    return result;
}

function GetAnswer(data, _claims) {
    data.Answer = SuppUtility.GetAnswer(data.Answer, _claims);

    return data;
}	