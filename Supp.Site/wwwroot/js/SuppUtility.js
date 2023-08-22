//namespace SuppUtility
var SuppUtility =
{
    GetAnswer: function (value, _claims) {
        var list = [];

        try {
            if (Array.isArray(value)) {
                list = value;
            } else {
                list = JSON.parse(value);
            }
        } catch (error) {
            list.push(value);
        }

        if (list == null) list = [""];

        var x = Math.floor(Math.random() * list.length);

        value = list[x];

        if (value == null) value = "";

        value = value.replace("SURNAME", _claims.Surname);
        value = value.replace("NAME", _claims.Name);

        return value;
    }
}