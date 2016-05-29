var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
var chosenDate = new Date();

$(document).ready(function () {
    updateMonth();

    $('#txtDate').pickadate({
        selectMonths: false,
        selectYears: false
    });

    $("#txtCollectiveDateFrom").pickadate(
        {
            selectMonths: false,
            selectYears: false
        });

    $("#txtCollectiveDateTo").pickadate(
        {
            selectMonths: false,
            selectYears: false
        });

    //Set the collectives as from-to pickers
    var from = $('#txtCollectiveDateFrom').pickadate("picker");
    var to = $("#txtCollectiveDateTo").pickadate("picker");

    from.on('set', function (event) {
        if (event.select) {
            to.set('min', from.get('select'))
        } else if ('clear' in event) {
            to.set('min', false)
        }
    });

    to.on('set', function (event) {
        if (event.select) {
            from.set('max', to.get('select'))
        } else if ('clear' in event) {
            from.set('max', false)
        }
    });

    $('.picker').appendTo('body');

    $('#slCategory').material_select();
    $('#slSubCategory').material_select();

    $('#slCollectiveCategory').material_select();
    $('#slCollectiveSubCategory').material_select();
});

function drawChart() {

    $("#divExpenseChart").html(""); //Clear the old div. It's acting up sometimes, not sure why
    $("#divExpenseSubcatChart").html("");

    $("#divIncomeChart").html("");
    $("#divIncomeSubcatChart").html("");

    //Get the chart data

    $.get("http://localhost:7744/api/ledger/LedgerExpAggregate?Month=" + (chosenDate.getMonth() + 1) + "&year=" + chosenDate.getFullYear(), function (data) {
        var cData = [];

        $.each(data, function (index, val) {
            cData.push([val.Category, Number(val.Sum)]);
        });

        //and plot it
        jQuery.jqplot('divExpenseChart', [cData],
        {
            grid: { background: '#FFFFFF', borderWidth: 0, shadow: 0 },
            seriesDefaults: {
                renderer: jQuery.jqplot.PieRenderer,
                rendererOptions:
                    {
                        showDataLabels: true,
                        dataLabels: 'both',
                        // stroke the slices with a little thicker line.
                        lineWidth: 5,
                    }
            },
            legend: { show: false, location: 'e' }
        });
        $("#divExpenseChart").unbind("jqplotDataClick"); //Because otherwise it binds more than once
        $('#divExpenseChart').bind('jqplotDataClick',
    function (ev, seriesIndex, pointIndex, data) {
        drawSubcatExpenseChart(data[0], "divExpenseSubcatChart");
    });
    });

    //Income Graph
    $.get("http://localhost:7744/api/ledger/LedgerIncAggregate?Month=" + (chosenDate.getMonth() + 1) + "&year=" + chosenDate.getFullYear(), function (data) {
        var cData = [];

        $.each(data, function (index, val) {
            cData.push([val.Category, Number(val.Sum)]);
        });

        //and plot it
        jQuery.jqplot('divIncomeChart', [cData],
        {
            grid: { background: '#FFFFFF', borderWidth: 0, shadow: 0 },
            seriesDefaults: {
                renderer: jQuery.jqplot.PieRenderer,
                rendererOptions:
                    {
                        showDataLabels: true,
                        dataLabels: 'both',
                        // stroke the slices with a little thicker line.
                        lineWidth: 5,
                    }
            },
            legend: { show: false, location: 'e' }
        });
        $("#divIncomeChart").unbind("jqplotDataClick"); //Because otherwise it binds more than once
        $('#divIncomeChart').bind('jqplotDataClick',
    function (ev, seriesIndex, pointIndex, data) {
        drawSubcatExpenseChart(data[0], "divIncomeSubcatChart");
    });
    });

}

///This uses catName instead of ID. I know, not ideal - but there's nothing to do about it.
function drawSubcatExpenseChart(catName, chartSelector) {
    //Clear the old div. It acts up
    $("#" + chartSelector).html("");

    $.get("http://localhost:7744/api/ledger/LedgerSubcategoryAggregate?Month=" + (chosenDate.getMonth() + 1) + "&year=" + chosenDate.getFullYear() + "&catName=" + window.btoa(catName), //base 64 so we don't have issues 
        function (data) {
            var cData = [];

            $.each(data, function (index, val) {
                cData.push([val.Category, Number(val.Sum)]);
            });

            //and plot it
            jQuery.jqplot(chartSelector, [cData],
        {
            grid: { background: '#FFFFFF', borderWidth: 0, shadow: 0 }
                ,
            seriesDefaults: {
                renderer: jQuery.jqplot.PieRenderer,
                rendererOptions:
                    {
                        showDataLabels: true,
                        dataLabels: 'both',
                        // stroke the slices with a little thicker line.
                        lineWidth: 5,
                    }
            },
            legend: { show: false, location: 'e' }
        }
      );

        });
}

function updateMonth() {
    $("#lblMonth").html(months[chosenDate.getMonth()] + " " + chosenDate.getFullYear());

    fetchData();
}

///Changes the chosen date by a number of months and reloads everything
function changeDate(amount) {
    chosenDate.setMonth(chosenDate.getMonth() + amount);
    updateMonth();
}

function readLedgerEntry() {
    var entry = new Object();
    entry.Item = $("#txtItem").val();
    entry.IsExpense = $("#ckExpense").prop("checked");
    entry.Date = $("#txtDate").val();
    entry.Category = $("#slCategory").val();
    entry.SubCategory = $("#slSubCategory").val();
    entry.Money = $("#txtMoney").val();

    return entry;
}

function readCollectiveEntry() {
    var entry = new Object();
    entry.Item = $("#txtCollectiveItem").val();
    entry.IsExpense = $("#ckCollectiveExpense").prop("checked");
    entry.DateFrom = $("#txtCollectiveDateFrom").val();
    entry.DateTo = $("#txtCollectiveDateTo").val();
    entry.Category = $("#slCollectiveCategory").val();
    entry.SubCategory = $("#slCollectiveSubCategory").val();
    entry.Money = $("#txtCollectiveMoney").val();

    return entry;
}

function validateCollectiveEntry(entry) {
    if (entry.Item.length == 0) {
        Materialize.toast('Item Missing', 2000);
        return false;
    }


    if (Number(entry.Money) == NaN || Number(entry.Money) <= 0) {
        Materialize.toast('Money needs to be larger than 0', 2000);
        return false;
    }

    if (entry.DateFrom.length == 0 || entry.DateTo.length == 0) {
        Materialize.toast('Date Range is Missing', 2000);
        return false;
    }

    return true;

}

function validateLedgerEntry(entry) {
    if (entry.Item.length == 0) {
        Materialize.toast('Item Missing', 2000);
        return false;
    }

    if (entry.Date.length == 0) {
        Materialize.toast('Date Missing', 2000);
        return false;
    }

    if (Number(entry.Money) == NaN || Number(entry.Money) <= 0) {
        Materialize.toast('Money needs to be larger than 0', 2000);
        return false;
    }

    return true;
}

///Sorta clears the interface. Will retain the date and categories in case multiples want to be input
function clearEntryInterface() {
    $("#txtItem").val("");
    $("#txtMoney").val("0");
    $("#txtCollectiveItem").val("");
    $("#txtCollectiveMoney").val("0");
}

function saveCollectiveEntry() {
    var entry = readCollectiveEntry();

    if (validateCollectiveEntry(entry)) {
        //Save
        $.post("http://localhost:7744/api/ledger/CollectiveLedgerEntry", entry, function (data) {
            Materialize.toast("Entry Saved", 2000);

            fetchData();
            clearEntryInterface();
            closeModal();
        }).fail(function (xhr)
        {
            Materialize.toast("Error occured: " + JSON.parse(xhr.responseText).Message, 2000)
        });
    }
}

function saveEntry(quit) {
    var entry = readLedgerEntry();

    if (validateLedgerEntry(entry)) {
        //Save
        $.post("http://localhost:7744/api/ledger/LedgerEntry", entry, function (data) {
            Materialize.toast('Entry Saved', 2000);

            fetchData();

            clearEntryInterface();

            if (quit) {
                closeModal();
            }
        });
    }
    else {
        return;
    }
}

function closeModal() {
    fetchData();
    $('#mdlPurchase').closeModal();
    $("#mdlCollective").closeModal();
}

///Fetches the data. Considers filters and month and whatnot.
function fetchData() {
    $.get("http://localhost:7744/api/ledger/ledgerentry?Month=" + (chosenDate.getMonth() + 1) + "&year=" + chosenDate.getFullYear(), function (data) {
        //Populate two tables, one with collectives, the other without
        var html = "";
        var collectiveHtml = "";
        var total = 0;

        $.each(data, function (index, val) {
            var elementHtml = "";

            if (val.IsCollective)
            {
                elementHtml = "<tr><td>" + val.Item + "</td><td>Collective</td><td> " + val.Category + "</td><td>" + val.SubCategory + "</td><td style='text-align:right";
            }
            else
            {
                elementHtml = "<tr><td>" + val.Item + "</td><td>" + val.Date + "</td><td> " + val.Category + "</td><td>" + val.SubCategory + "</td><td style='text-align:right";
            }

            if (val.IsExpense)
            {
                elementHtml += ";color:red";
                total -= val.Money;
            }
            else
            {
                elementHtml += ";color:green";
                total += val.Money;
            }

            elementHtml += "'>" + val.Money + "</td><tr>";

            html += elementHtml;

        });

        $("#tblLedger tbody").html(html);

        $("#lblTotal").html(total.toFixed(2));

        $("#lblTotal").css("color", "green");

        if (total < 0) {
            $("#lblTotal").css("color", "red");
        }
        else if (total == 0) {
            $("#lblTotal").css("color", "black");
        }

    }, "json");

    drawChart();
}

//Marks the modal interface as being expense or income depending on the state of the lever
function setIE()
{
    if ($("#ckExpense").prop("checked")) {
        $("#lblNewEntry").html("Add New Expense");
    }
    else {
        $("#lblNewEntry").html("Add New Income");
    }

    getCategories($("#ckExpense").prop("checked"));
}

function setCollectiveIE() {
    if ($("#ckCollectiveExpense").prop("checked")) {
        $("#lblNewCollectiveEntry").html("Add new Collective Expense");
    }
    else {
        $("#lblNewCollectiveEntry").html("Add new Collective Income");
    }

    getCategories($("#ckCollectiveExpense").prop("checked"));
}

function openAddElement(isExpense) {
    $("#ckExpense").prop("checked", isExpense);

    setIE();

    $('#mdlPurchase').openModal();
}

function openCollectiveElement(isExpense) {
    $("#ckCollectiveExpense").prop("checked", isExpense);

    setCollectiveIE();

    $("#mdlCollective").openModal();
}

var categories = [];

function getCategories(isExpense) {
    $.get("http://localhost:7744/api/ledger/category?activeonly=true&expense=" + isExpense, function (data) {
        var html = "";

        $.each(data, function (index, val) {
            html += "<option value='" + val.ID + "'>" + val.Name + "</option>";
        });

        categories = data;

        $("#slCategory").html(html);
        $("#slCategory").material_select();

        $("#slCollectiveCategory").html(html);
        $("#slCollectiveCategory").material_select();

        getSubCategories($("#slCategory").val());
    });
}

function getSubCategories(id) {
    //Find the subcat by looking through the categories we have

    var cat = null;
    var html = "";

    for (var i = 0; i < categories.length; i++) {
        if (categories[i].ID == id) {
            //match found
            cat = categories[i];
            break;
        }
    }

    //Go through all the subcats


    $.each(cat.Subcats, function (index, val) {
        html += "<option value='" + val.ID + "'>" + val.Name + "</option>";
    });

    $("#slSubCategory").html(html);
    $("#slSubCategory").material_select();

    $("#slCollectiveSubCategory").html(html);
    $("#slCollectiveSubCategory").material_select();

}
