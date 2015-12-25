$(document).ready(function () {
    getCurrentMonth();

    //$("#txtDate").datepicker();
    //$("#txtDate").datepicker("option", "dateFormat", "d M y");

    $('.datepicker').pickadate({
        selectMonths: true, // Creates a dropdown to control month
        selectYears: 2 // Creates a dropdown of 15 years to control year
    });

    $('.picker').appendTo('body');

    $('#slCategory').material_select();
    $('#slSubCategory').material_select();

    getCategories();
});

//Marks the modal interface as being expense or income depending on the state of the lever
function setIE() {
    if ($("#ckExpense").prop("checked"))
    {
        $("#lblNewEntry").html("Add New Expense");
    }
    else {
        $("#lblNewEntry").html("Add New Income");
    }
}

function openAddElement(isExpense) {
    $("#ckExpense").prop("checked", isExpense);

    setIE();

    $('#mdlPurchase').openModal();
}

function getCategories() {
    $.get("http://localhost:7744/api/ledger/category?activeonly=true", function (data) {
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

function getCurrentMonth() {
    //$.ajax({
    //    url: "/ledger/Get?Month=" + (new Date()).getMonth - 1,
    //    success: function (data)
    //    {
    //        alert(data);
    //    },

    //});

    $.get("http://localhost:7744/api/ledger/Get?Month=" + ((new Date()).getMonth() + 1), function (data) {
        //Populate a table
        var html = "";

        $.each(data, function (index, val) {
            html += "<tr><td>" + val.Item + "</td><td>" + val.Date + "</td><td> " + val.Category + "</td><td>" + val.SubCategory + "</td><td style='text-align:right'>" + val.Money + "</td><tr>";
        });

        $("#tblLedger tbody").html(html);

    }, "json");
}