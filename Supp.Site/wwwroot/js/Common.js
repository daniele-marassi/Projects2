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

function SendHttpRequest(method, url, json, funcName, _sslCertificateExpirationDate) {

    if (_sslCertificateExpirationDate != undefined && _sslCertificateExpirationDate != null && _sslCertificateExpirationDate != "") {

        var expiration = new Date(_sslCertificateExpirationDate);
        var now = new Date();
        if (now > expiration) {
            alert("Ssl certificate expirated!");
            location.reload();
            return;
        }
    }

    var xhr = new XMLHttpRequest();
    xhr.open(method, url, true);
    xhr.onload = function (e) {
        code = xhr.response;
        if (funcName == "ResultSendPhrase") ResultSendPhrase(code);
        else if (funcName == "ResultRecognitionPhrase") ResultRecognitionPhrase(code);
        else if (funcName == "ResultRecognitionEhi") ResultRecognitionEhi(code);
        else if (funcName == "ResultActionShortcut") ResultActionShortcut(code);
        else if (funcName != undefined && funcName != null && funcName != '') ExecuteFunction(funcName, code);
    };
    xhr.onerror = function () {
        var _err = "SendHttpRequest - ** An error occurred" + ", response: " + xhr.response + ", responseText: " + xhr.responseText + ", status: " + xhr.status + ", method: " + method + ", url: " + url + ", json: " + json + ", funcName: " + funcName;
        console.error(_err);
        alert("Probably the Ssl certificate is expirated!" + " - " + _err);
        location.reload();
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
        if (obj[x][fieldName].trim().toLowerCase() === value.trim().toLowerCase()) {
            return obj[x];
        }
    }

    return null;
}

function HexDecode(str) {
    //str = str.replace(/&aacute;/g, "à");
    //str = str.replace(/&eacute;/g, "è");
    //str = str.replace(/&oacute;/g, "ò");
    //str = str.replace(/&uacute;/g, "ù");
    //str = str.replace(/&iacute;/g, "ì");
    //
    //str = str.replace(/&Aacute;/g, "à");
    //str = str.replace(/&Eacute;/g, "è");
    //str = str.replace(/&Oacute;/g, "ò");
    //str = str.replace(/&Uacute;/g, "ù");
    //str = str.replace(/&Iacute;/g, "ì");
    //
    //str = str.replace(/&#xE0;/g, "à");
    //str = str.replace(/&#xE8;/g, "è");
    //str = str.replace(/&#xF2;/g, "ò");
    //str = str.replace(/&#xF9;/g, "ù");
    //str = str.replace(/&#xEC;/g, "ì");
    //
    //str = str.replace(/&#xC0;/g, "à");
    //str = str.replace(/&#xC8;/g, "è");
    //str = str.replace(/&#xD2;/g, "ò");
    //str = str.replace(/&#xD9;/g, "ù");
    //str = str.replace(/&#xCC;/g, "ì");
    //
    //str = str.replace(/&#x27;/g, "'");
    //
    //str = str.replace(/&#xD;/g, " ");
    //str = str.replace(/&#xA;/g, " ");
    //
    //str = str.replace(/&#xEC;/g, "ì");
    //
    //str = str.replace(/&gt;/g, ">");
    //str = str.replace(/&lt;/g, "<");
    //
    //str = str.replace(/&#x22;/g, '"');
    //str = str.replace(/&#x26;/g, "&");
    //str = str.replace(/&#x3C;/g, "<");
    //str = str.replace(/&#x3E;/g, ">");
    //str = str.replace(/&#x152;/g, "Œ");
    //str = str.replace(/&#x153;/g, "œ");
    //str = str.replace(/&#x160;/g, "Š");
    //str = str.replace(/&#x161;/g, "š");
    //str = str.replace(/&#x178;/g, "Ÿ");
    //str = str.replace(/&#x2C6;/g, "ˆ");
    //str = str.replace(/&#x2DC;/g, "˜");
    //str = str.replace(/&#x2013;/g, "–");
    //str = str.replace(/&#x2014;/g, "—");
    //str = str.replace(/&#x2018;/g, "‘");
    //str = str.replace(/&#x2019;/g, "’");
    //str = str.replace(/&#x201A;/g, "‚");
    //str = str.replace(/&#x201C;/g, "“");
    //str = str.replace(/&#x201D;/g, "”");
    //str = str.replace(/&#x201E;/g, "„");
    //str = str.replace(/&#x2020;/g, "†");
    //str = str.replace(/&#x2021;/g, "‡");
    //str = str.replace(/&#x2030;/g, "‰");
    //str = str.replace(/&#x2039;/g, "‹");
    //str = str.replace(/&#x203A;/g, "›");
    //str = str.replace(/&#x20AC;/g, "€");

    var txt = document.createElement("textarea");
    txt.innerHTML = str;
    str = txt.value;

    str = str.normalize();

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

function MatchPhrase(_phrase, webSpeechlist, identification) {
    var _data = null;
    var result = { Data: null, WebSpeechKeysMatched: null };
    result.Data = null;
    result.WebSpeechKeysMatched = null;
    var minMatch = 0;
    var countMatch = 0;
    var phraseMatch = "";
    var previousCountMatch = 0;
    var founded = false;

    if (webSpeechlist != undefined && webSpeechlist != null && webSpeechlist != "") {
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
    }

    if (_data != null) {
        result.Data = GetAnswer(_data, identification);
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

function GetAnswer(data, identification) {
    data.Answer = SuppUtility.GetAnswer(data.Answer, identification);

    return data;
}

//example in https://loading.io/background/ //remove Width="xx", Height="xx" and in style remove background-color:xxx;, add width: 100%;, in g change scale(-1,1) in scale(-1.1,1.1), boxAnimatedSpeech opacity="0.9", other variables opacity="0.5" 
var boxAnimatedListening = '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" style="margin:auto;z-index:1;position:relative; width: 100%;" preserveAspectRatio="xMidYMid" viewBox="0 0 3000 953">  <g transform="translate(1500,476.5) scale(-1.1,1.1) translate(-1500,-476.5)">  <linearGradient id="lg-0.12295177618329411" x1="0" x2="1" y1="0" y2="0">    <stop stop-color="#ff2f2f" offset="0"></stop>    <stop stop-color="#ff0000" offset="1"></stop>  </linearGradient>  <linearGradient id="lg-0.12295177618329412" x1="0" x2="1" y1="0" y2="0">    <stop stop-color="#ff2f2f" offset="0"></stop>    <stop stop-color="#da2a2a" offset="1"></stop>  </linearGradient>  <path d="" fill="url(#lg-0.12295177618329411)" opacity="0.5">    <animate attributeName="d" dur="9.090909090909092s" repeatCount="indefinite" keyTimes="0;0.333;0.667;1" calcmod="spline" keySplines="0.2 0 0.2 1;0.2 0 0.2 1;0.2 0 0.2 1" begin="0s" values="M0 0M 0 650.9012373856431Q 375 528.5290192278325 750 520.0691314046794T 1500 569.4947686959802T 2250 540.8236593567924T 3000 637.5726809659013L 3000 386.76674516133886Q 2625 319.8012797025959 2250 314.8707898922493T 1500 404.3623671193542T 750 292.85766608151425T 0 348.92776790575493Z;M0 0M 0 558.4690557575133Q 375 618.3820302235039 750 610.95543958593T 1500 562.7905861367346T 2250 557.1754373195541T 3000 665.8646192426828L 3000 293.5208920797761Q 2625 298.7659030843393 2250 291.84608659840217T 1500 435.7651843291168T 750 317.0971599338627T 0 374.1747463473898Z;M0 0M 0 529.5390666292412Q 375 539.5699990268544 750 539.227364167668T 1500 640.691288924297T 2250 562.0170928753315T 3000 647.4329475988617L 3000 385.5068863541555Q 2625 372.0740720679168 2250 364.5114090817394T 1500 340.65122170971546T 750 441.3025642628007T 0 416.6403877684558Z;M0 0M 0 650.9012373856431Q 375 528.5290192278325 750 520.0691314046794T 1500 569.4947686959802T 2250 540.8236593567924T 3000 637.5726809659013L 3000 386.76674516133886Q 2625 319.8012797025959 2250 314.8707898922493T 1500 404.3623671193542T 750 292.85766608151425T 0 348.92776790575493Z"></animate>  </path><path d="" fill="url(#lg-0.12295177618329412)" opacity="0.5">    <animate attributeName="d" dur="9.090909090909092s" repeatCount="indefinite" keyTimes="0;0.333;0.667;1" calcmod="spline" keySplines="0.2 0 0.2 1;0.2 0 0.2 1;0.2 0 0.2 1" begin="-4.545454545454546s" values="M0 0M 0 648.8757091362452Q 375 629.4912141356333 750 620.777093490925T 1500 564.5282037095584T 2250 541.6964086631235T 3000 658.9273866362248L 3000 290.07810935104925Q 2625 439.2812986113886 2250 437.88895007163535T 1500 392.8401430724568T 750 400.76075560297846T 0 335.31407532607335Z;M0 0M 0 583.0550763129477Q 375 543.1395333514719 750 536.2039328108966T 1500 571.4568294850293T 2250 584.2746018869279T 3000 561.2101017050868L 3000 370.53841441371475Q 2625 415.38475483223954 2250 415.11343004316984T 1500 375.87098420121276T 750 396.8761914002128T 0 345.3161353262193Z;M0 0M 0 652.176077426726Q 375 620.7213602233345 750 611.2651889169598T 1500 600.6939199085961T 2250 505.8739718183975T 3000 578.6502902714343L 3000 430.37097090159335Q 2625 369.5448940733296 2250 365.0478944042838T 1500 362.0914720221535T 750 391.8788640460124T 0 383.7324542660948Z;M0 0M 0 648.8757091362452Q 375 629.4912141356333 750 620.777093490925T 1500 564.5282037095584T 2250 541.6964086631235T 3000 658.9273866362248L 3000 290.07810935104925Q 2625 439.2812986113886 2250 437.88895007163535T 1500 392.8401430724568T 750 400.76075560297846T 0 335.31407532607335Z"></animate>  </path></g>  </svg>';
var boxAnimatedSpeech = '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" style="margin:auto;z-index:1;position:relative; width: 100%;" preserveAspectRatio="xMidYMid" viewBox="0 0 3000 953">  <g transform="translate(1500,476.5) scale(-1.1,1.1) translate(-1500,-476.5)">  <linearGradient id="lg-0.12295177618329411" x1="0" x2="1" y1="0" y2="0">    <stop stop-color="#ff2f2f" offset="0"></stop>    <stop stop-color="#ff0000" offset="1"></stop>  </linearGradient>  <linearGradient id="lg-0.12295177618329412" x1="0" x2="1" y1="0" y2="0">    <stop stop-color="#ff2f2f" offset="0"></stop>    <stop stop-color="#da2a2a" offset="1"></stop>  </linearGradient>  <path d="" fill="url(#lg-0.12295177618329411)" opacity="0.9">    <animate attributeName="d" dur="1.9464480874316939s" repeatCount="indefinite" keyTimes="0;0.333;0.667;1" calcmod="spline" keySplines="0.2 0 0.2 1;0.2 0 0.2 1;0.2 0 0.2 1" begin="0s" values="M0 0M 0 605.6410396107964Q 250 519.2868720076112 500 513.175423790126T 1000 645.8760877434809T 1500 621.3997552061398T 2000 626.3559911249106T 2500 638.6317031467672T 3000 553.731020568717L 3000 437.27285350558697Q 2750 350.8647336304113 2500 350.10090797098087T 2000 443.25670351268064T 1500 297.1446982840303T 1000 395.3026513193123T 500 384.1586503357802T 0 405.51084976024373Z;M0 0M 0 605.3951955990162Q 250 575.7683382746486 500 573.0446591248411T 1000 648.5676206958766T 1500 649.1473669965658T 2000 645.0785224733099T 2500 540.12798936574T 3000 586.5316680105504L 3000 423.28428539319793Q 2750 414.65019645193644 2500 413.09802255812974T 2000 319.10753385794925T 1500 302.6012515651614T 1000 414.3589885086766T 500 351.3692283859299T 0 441.18805879686164Z;M0 0M 0 635.1210568358051Q 250 531.503375701163 500 528.1386784595074T 1000 633.1225029836544T 1500 515.9881063263404T 2000 623.0215293081786T 2500 631.471050625095T 3000 654.7002564315612L 3000 368.6991823952591Q 2750 323.5571774116108 2500 318.52403269593515T 2000 413.5152487232341T 1500 441.694740792256T 1000 302.1785174418187T 500 309.7610818566693T 0 321.9141693183017Z;M0 0M 0 605.6410396107964Q 250 519.2868720076112 500 513.175423790126T 1000 645.8760877434809T 1500 621.3997552061398T 2000 626.3559911249106T 2500 638.6317031467672T 3000 553.731020568717L 3000 437.27285350558697Q 2750 350.8647336304113 2500 350.10090797098087T 2000 443.25670351268064T 1500 297.1446982840303T 1000 395.3026513193123T 500 384.1586503357802T 0 405.51084976024373Z"></animate>  </path><path d="" fill="url(#lg-0.12295177618329412)" opacity="0.9">    <animate attributeName="d" dur="1.9464480874316939s" repeatCount="indefinite" keyTimes="0;0.333;0.667;1" calcmod="spline" keySplines="0.2 0 0.2 1;0.2 0 0.2 1;0.2 0 0.2 1" begin="-0.27322404371584696s" values="M0 0M 0 561.9746122536249Q 250 601.3542954814417 500 600.4057034416958T 1000 514.6925201338015T 1500 628.1629618843197T 2000 606.1775729205151T 2500 605.2674213492642T 3000 643.294830571026L 3000 299.289511913654Q 2750 368.7290344254009 2500 365.1704869660533T 2000 439.8719551647539T 1500 337.4736534329669T 1000 364.667040151865T 500 327.63920295565737T 0 342.07733654931616Z;M0 0M 0 626.9391796387656Q 250 531.7334678736349 500 526.4190080009921T 1000 548.5078940125705T 1500 564.3676215313758T 2000 546.8212055238365T 2500 513.2012231698272T 3000 608.2836894968841L 3000 357.7526252394241Q 2750 318.5551877700771 2500 317.49813704189717T 2000 317.94572455371497T 1500 434.30683728906934T 1000 421.33905591073T 500 331.17121837706486T 0 321.9701391441795Z;M0 0M 0 593.9943224759505Q 250 530.3769767549544 500 523.5318953717235T 1000 583.0103793967929T 1500 655.3652034441898T 2000 581.3750232756856T 2500 628.5802261817767T 3000 640.7370052667992L 3000 444.2609859857702Q 2750 310.65845284005445 2500 303.0641825144603T 2000 407.4032200702527T 1500 419.4161128604768T 1000 372.26020490974656T 500 311.00906273022923T 0 390.3699177872379Z;M0 0M 0 561.9746122536249Q 250 601.3542954814417 500 600.4057034416958T 1000 514.6925201338015T 1500 628.1629618843197T 2000 606.1775729205151T 2500 605.2674213492642T 3000 643.294830571026L 3000 299.289511913654Q 2750 368.7290344254009 2500 365.1704869660533T 2000 439.8719551647539T 1500 337.4736534329669T 1000 364.667040151865T 500 327.63920295565737T 0 342.07733654931616Z"></animate>  </path></g>  </svg>';
var boxAnimatedStopped = '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" style="margin:auto;z-index:1;position:relative; width: 100%;" preserveAspectRatio="xMidYMid" viewBox="0 0 3000 953">  <g transform="translate(1500,476.5) scale(-1.1,1.1) translate(-1500,-476.5)">  <linearGradient id="lg-0.12295177618329411" x1="0" x2="1" y1="0" y2="0">    <stop stop-color="#ff2f2f" offset="0"></stop>    <stop stop-color="#ff0000" offset="1"></stop>  </linearGradient>  <linearGradient id="lg-0.12295177618329412" x1="0" x2="1" y1="0" y2="0">    <stop stop-color="#ff2f2f" offset="0"></stop>    <stop stop-color="#da2a2a" offset="1"></stop>  </linearGradient>  <path d="" fill="url(#lg-0.12295177618329411)" opacity="0.5">    <animate attributeName="d" dur="0s" repeatCount="indefinite" keyTimes="0;0.333;0.667;1" calcmod="spline" keySplines="0.2 0 0.2 1;0.2 0 0.2 1;0.2 0 0.2 1" begin="0s" values="M0 0M 0 650.9012373856431Q 375 528.5290192278325 750 520.0691314046794T 1500 569.4947686959802T 2250 540.8236593567924T 3000 637.5726809659013L 3000 386.76674516133886Q 2625 319.8012797025959 2250 314.8707898922493T 1500 404.3623671193542T 750 292.85766608151425T 0 348.92776790575493Z;M0 0M 0 558.4690557575133Q 375 618.3820302235039 750 610.95543958593T 1500 562.7905861367346T 2250 557.1754373195541T 3000 665.8646192426828L 3000 293.5208920797761Q 2625 298.7659030843393 2250 291.84608659840217T 1500 435.7651843291168T 750 317.0971599338627T 0 374.1747463473898Z;M0 0M 0 529.5390666292412Q 375 539.5699990268544 750 539.227364167668T 1500 640.691288924297T 2250 562.0170928753315T 3000 647.4329475988617L 3000 385.5068863541555Q 2625 372.0740720679168 2250 364.5114090817394T 1500 340.65122170971546T 750 441.3025642628007T 0 416.6403877684558Z;M0 0M 0 650.9012373856431Q 375 528.5290192278325 750 520.0691314046794T 1500 569.4947686959802T 2250 540.8236593567924T 3000 637.5726809659013L 3000 386.76674516133886Q 2625 319.8012797025959 2250 314.8707898922493T 1500 404.3623671193542T 750 292.85766608151425T 0 348.92776790575493Z"></animate>  </path><path d="" fill="url(#lg-0.12295177618329412)" opacity="0.5">    <animate attributeName="d" dur="0s" repeatCount="indefinite" keyTimes="0;0.333;0.667;1" calcmod="spline" keySplines="0.2 0 0.2 1;0.2 0 0.2 1;0.2 0 0.2 1" begin="0s" values="M0 0M 0 648.8757091362452Q 375 629.4912141356333 750 620.777093490925T 1500 564.5282037095584T 2250 541.6964086631235T 3000 658.9273866362248L 3000 290.07810935104925Q 2625 439.2812986113886 2250 437.88895007163535T 1500 392.8401430724568T 750 400.76075560297846T 0 335.31407532607335Z;M0 0M 0 583.0550763129477Q 375 543.1395333514719 750 536.2039328108966T 1500 571.4568294850293T 2250 584.2746018869279T 3000 561.2101017050868L 3000 370.53841441371475Q 2625 415.38475483223954 2250 415.11343004316984T 1500 375.87098420121276T 750 396.8761914002128T 0 345.3161353262193Z;M0 0M 0 652.176077426726Q 375 620.7213602233345 750 611.2651889169598T 1500 600.6939199085961T 2250 505.8739718183975T 3000 578.6502902714343L 3000 430.37097090159335Q 2625 369.5448940733296 2250 365.0478944042838T 1500 362.0914720221535T 750 391.8788640460124T 0 383.7324542660948Z;M0 0M 0 648.8757091362452Q 375 629.4912141356333 750 620.777093490925T 1500 564.5282037095584T 2250 541.6964086631235T 3000 658.9273866362248L 3000 290.07810935104925Q 2625 439.2812986113886 2250 437.88895007163535T 1500 392.8401430724568T 750 400.76075560297846T 0 335.31407532607335Z"></animate>  </path></g>  </svg>';
