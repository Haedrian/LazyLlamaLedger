var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
var chosenMonth = (new Date()).getMonth();
var chosenYear = (new Date()).getFullYear();

$(document).ready(function () {
    changeMonth();

    $('.datepicker').pickadate({
        selectMonths: false,
        selectYears: false 
    });

    $('.picker').appendTo('body');

    $('#slCategory').material_select();
    $('#slSubCategory').material_select();
});

function changeMonth()
{
    $("#lblMonth").html(months[chosenMonth] + " " + chosenYear);

    fetchData();
}

function readLedgerEntry()
{
    var entry = new Object();
    entry.Item = $("#txtItem").val();
    entry.IsExpense = $("#ckExpense").prop("checked");
    entry.Date = $("#txtDate").val();
    entry.Category = $("#slCategory").val();
    entry.SubCategory = $("#slSubCategory").val();
    entry.Money = $("#txtMoney").val();

    return entry;
}

function validateLedgerEntry(entry)
{
    if (entry.Item.length == 0)
    {
        Materialize.toast('Item Missing', 2000);
        return false;
    }

    if (entry.Date.length == 0)
    {
        Materialize.toast('Date Missing', 2000);
        return false;
    }

    if (Number(entry.Money) == NaN || Number(entry.Money) <= 0)
    {
        Materialize.toast('Money needs to be larger than 0', 2000);
        return false;
    }

    return true;
}

///Sorta clears the interface. Will retain the date and categories in case multiples want to be input
function clearEntryInterface()
{
    $("#txtItem").val("");
    $("#txtMoney").val("0");
}

function saveEntry(quit)
{
    var entry = readLedgerEntry();
    
    if (validateLedgerEntry(entry))
    {
        //Save
        $.post("http://localhost:7744/api/ledger/LedgerEntry", entry, function (data)
        {
            Materialize.toast('Entry Saved', 2000);

            fetchData();

            clearEntryInterface();

            if (quit)
            {
                closeModal();
            }
        });
    }
    else 
    {
        return;
    }
}

function closeModal()
{
    fetchData();
    $('#mdlPurchase').closeModal();
}

///Fetches the data. Considers filters and month and whatnot.
function fetchData()
{
    $.get("http://localhost:7744/api/ledger/ledgerentry?Month=" + (chosenMonth+1) + "&year="+chosenYear, function (data) {
        //Populate a table
        var html = "";
        var total = 0;

        $.each(data, function (index, val) {
            html += "<tr><td>" + val.Item + "</td><td>" + val.Date + "</td><td> " + val.Category + "</td><td>" + val.SubCategory + "</td><td style='text-align:right";

            if (val.IsExpense)
            {
                html += ";color:red";
                total -= val.Money;
            }
            else
            {
                html += ";color:green";
                total += val.Money;
            }

            html+= "'>" + val.Money + "</td><tr>";
        });

        $("#tblLedger tbody").html(html);

        $("#lblTotal").html(total);
        
        $("#lblTotal").css("color","green");

        if (total < 0)
        {
            $("#lblTotal").css("color","red");
        }
        else if (total == 0)
        {
            $("#lblTotal").css("color", "black");
        }

    }, "json");
}

//Marks the modal interface as being expense or income depending on the state of the lever
function setIE() {
    if ($("#ckExpense").prop("checked"))
    {
        $("#lblNewEntry").html("Add New Expense");
    }
    else {
        $("#lblNewEntry").html("Add New Income");
    }

    getCategories($("#ckExpense").prop("checked"));
}

function openAddElement(isExpense) {
    $("#ckExpense").prop("checked", isExpense);

    setIE();

    $('#mdlPurchase').openModal();
}

function getCategories(isExpense) {
    $.get("http://localhost:7744/api/ledger/category?activeonly=true&expense=" + isExpense , function (data) {
        var html = "";

        $.each(data, function (index, val) {
            html += "<option value='" + val.ID + "'>" + val.Name + "</option>";
        });

        $("#slCategory").html(html);
        $("#slCategory").material_select();

        getSubCategories($("#slCategory").val());
    });
}

function getSubCategories(id) {
    $.get("http://localhost:7744/api/ledger/subcategory?activeonly=true&category=" + id, function (data) {
        var html = "";

        $.each(data, function (index, val) {
            html += "<option value='" + val.ID + "'>" + val.Name + "</option>";
        });

        $("#slSubCategory").html(html);
        $("#slSubCategory").material_select();
    });
}
