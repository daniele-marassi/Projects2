google.charts.load('current', { 'packages': ['gauge'] });
google.charts.load('current', { 'packages': ['table'] });

var chartData = [];

function IsBoolean(val) {
    switch (val.toLowerCase().trim()) {
        case "true": case "yes": /*case "1":*/ return true;
        case "false": case "no": /*case "0":*/ return true;
        default: return false;
    }
}

function CreateChartAndDetail(data) {
    var row = 0;
    var index = 0;
    var start = '';
    var end = '';
    var ack = '';
    var name = '';
    var tagName = '';
    var chartDivName = '';
    var tableDivName = '';
    var chartAndTableDivName = '';
    var chartAndTableButtonName = '';
    var chartAndTableLabelName = '';
    var containerName = 'ChartAndTableContainer';
    var fromRow = 0;
    for (row = 0; row < data.length; row++) {
        name = data[row]['eventName'];
        tagName = data[row]['tagName'];

        if (tagName.indexOf("TagStart") >= 0) start = data[row]['tagValue'];
        if (tagName.indexOf("TagEnd") >= 0) end = data[row]['tagValue'];
        if (tagName.indexOf("TagAck") >= 0) ack = data[row]['tagValue'];

        chartDivName = "Chart_div_" + index;
        tableDivName = "Table_div_" + index;
        chartAndTableDivName = "ChartAndTable_div_" + index;
        chartAndTableButtonName = "ChartAndTable_btn_" + index;
        chartAndTableLabelName = "ChartAndTable_lbl_" + index;

        if (document.getElementById(chartAndTableDivName) == null && ((row + 1) == (data.length) || data[row + 1]['eventName'] != name)) {

            var container = document.getElementById(containerName);
            var div = document.createElement('div');
            div.id = chartAndTableDivName;
            div.style.cssText = "text-align:left; margin-bottom:20px;";

            var subDiv = document.createElement('div');
            subDiv.style.cssText = "float:left; position:relative;";

            var button = document.createElement('input');
            button.type = "button"
            button.id = chartAndTableButtonName;
            button.addEventListener("click", function () { Details(this.id); });
            button.value = "+"
            button.classList.add("btn");
            button.classList.add("btn-primary");
            button.style.cssText = "margin-top:-2px; position:relative; width:60px;";

            var label = document.createElement('div');
            label.id = chartAndTableLabelName;
            label.innerHTML = name;
            label.style.cssText = "font-size:50px; color:#999; position:relative;";

            subDiv.appendChild(label);
            subDiv.appendChild(button);

            container.appendChild(subDiv);
            container.appendChild(div);

            google.charts.setOnLoadCallback(DrawChart(index, start, end, ack, name, chartAndTableDivName, chartDivName));
            index++;

            google.charts.setOnLoadCallback(DrawTable(data, fromRow, (row + 1), chartAndTableDivName, index, tableDivName));
            fromRow = (row + 1);

        } else if (document.getElementById(chartAndTableDivName) != null && ((row + 1) == (data.length) || data[row + 1]['eventName'] != name)) {

            DrawChart(index, start, end, ack, name, chartAndTableDivName, chartDivName);
            index++;
            var scrollLeft = 0;
            var scrollTop = 0;
            if (document.getElementById(tableDivName.replace("div", "subdiv")) != null) {
                scrollLeft = document.getElementById(tableDivName.replace("div", "subdiv")).scrollLeft;
                scrollTop = document.getElementById('mainContainer').scrollTop;
            }
            //if (document.getElementById(chartAndTableButtonName).innerHTML == '+' || document.getElementById(tableDivName.replace("div", "chk")) == null || document.getElementById(tableDivName.replace("div", "chk")).checked)
            if (document.getElementById(tableDivName.replace("div", "chk")) == null || document.getElementById(tableDivName.replace("div", "chk")).checked)
                google.charts.setOnLoadCallback(DrawTable(data, fromRow, (row + 1), chartAndTableDivName, index, tableDivName));

            if (document.getElementById(tableDivName.replace("div", "subdiv")) != null) {
                document.getElementById(tableDivName.replace("div", "subdiv")).scrollLeft = scrollLeft;
                document.getElementById('mainContainer').scrollTop = scrollTop;
                
            }
            fromRow = (row + 1);
        }
    }
}

function Details(buttonId) {
    document.activeElement.blur();
    var button = document.getElementById(buttonId);
    var div = document.getElementById(buttonId.replace("ChartAndTable_btn","Table_div"));

    if (button.value == '+') {
        button.value = '-';
        div.style.cssText = "height: auto; width: auto; overflow:hidden; display:block;";
    } else {
        button.value = '+';
        div.style.cssText = "height: 0px; width: 0px; overflow:hidden; display:none;";
    }
}

function DrawTable(data, fromRow, toRow, containerName, index, divName) {
    var tableData = new google.visualization.DataTable();
    var row = 0;
    var y = 0;
    var x = 0;
    var column = 0;
    var rows = new Array();

    for (row = 0; row == 0; row++) {

        for (var column in data[row]) {
            var value = data[row][column];
            if (Number.isInteger(value))
                tableData.addColumn('number', ConvertCamelCase(column));
            else if (IsBoolean(value))
                tableData.addColumn('boolean', ConvertCamelCase(column));
            else
                tableData.addColumn('string', ConvertCamelCase(column));
        }
    }

    for (row = fromRow; row < toRow; row++) {
        x = 0;
        for (var column in data[row]) {
            var value = data[row][column];
            if (x == 0) rows[y] = new Array();

            if (Number.isInteger(value))
                rows[y][x] = parseInt(value);
            else if (IsBoolean(value))
                rows[y][x] = Boolean(value);
            else
                rows[y][x] = value;

            x++;
        }
        y++;
    }

    tableData.addRows(rows);

    var subDivName = divName.replace("div","subdiv");

    if (document.getElementById(divName) == null) {
        var container = document.getElementById(containerName);

        var div = document.createElement('div');
        div.id = divName;
        div.style.cssText = "position:relative; height: 0px; width: 0px; overflow:hidden; display:none;";

        var chk = document.createElement('input');
        chk.type = "checkbox";
        chk.id = divName.replace("div", "chk");
        chk.checked = true;
        chk.style.cssText = "position:relative; cursor:pointer; float:left;";

        div.appendChild(chk);

        var label = document.createElement('label');
        label.innerHTML = 'Live Data';
        label.style.cssText = "position:relative; border:none; width:100px; margin-left:15px; color:#777;";

        div.appendChild(label);

        var subDiv = document.createElement('div');
        subDiv.id = subDivName;
        subDiv.style.cssText = "position:relative; height: auto; width: 600px; overflow:auto;";

        div.appendChild(subDiv);

        container.appendChild(div);

        var divEnd = document.createElement('div');
        divEnd.style.cssText = "clear:left; position:relative;";

        container.appendChild(divEnd);
    }

    var table = new google.visualization.Table(document.getElementById(subDivName));
    table.draw(tableData, { showRowNumber: true});

    var _div = document.getElementById(subDivName);
    var _table = _div.children[0];

    _table.style.cssText = "position:relative; height: auto; width: auto; overflow:hidden; color:#777;";
}

function DrawChart(index, start, end, ack, name, containerName, divName) {

    var chartOptions = {
        min: parseInt(start), max: parseInt(end),
        yellowFrom: (80 * parseInt(end) / 100), yellowTo: (90 * parseInt(end) / 100),
        redFrom: (90 * parseInt(end) / 100), redTo: parseInt(end),
        minorTicks: 1
    };

    if (document.getElementById(divName) == null) {

        chartData[index] = new google.visualization.DataTable();
        chartData[index].addColumn('number', '');
        chartData[index].addRows(1);
        chartData[index].setCell(0, 0, parseInt(ack));

        var container = document.getElementById(containerName);

        var div = document.createElement('div');
        div.id = divName;
        div.style.cssText = "position:relative; float:left; position:relative; text-align: left;";

        container.appendChild(div);

        var divEnd = document.createElement('div');
        divEnd.style.cssText = "clear:left; position:relative;";

        container.appendChild(divEnd);

        var chart = new google.visualization.Gauge(document.getElementById(divName));
        chart.draw(chartData[index], chartOptions);
    }
    else {
        var chart = new google.visualization.Gauge(document.getElementById(divName));
        chartData[index].setValue(0, 0, parseInt(ack));
        chart.draw(chartData[index], chartOptions);
    }

    var _div = document.getElementById(divName);
    var _chart = _div.children[0];

    _chart.style.cssText = "position:relative; text-align: left; margin-left:0px;";
}