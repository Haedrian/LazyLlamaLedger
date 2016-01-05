
$(document).ready(function () {
    $('#txtDateFrom').pickadate(
    {
        selectMonths: true,
        selectYears: true,
        format: 2,
        format: 'yyyy-mm'
    });

    $('#txtDateTo').pickadate(
  {
      selectMonths: true,
      selectYears: true,
      format: 2,
      format: 'yyyy-mm',
  });

});


function getAndCreateExpenseTable()
{
    //Do we have a date from and a date to?
    if ($("#txtDateFrom").val().length == 0 || $("#txtDateTo").val().length == 0)
    {
        Materialize.toast('Start Date and End Date are required', 2000);
        return;
    }

    $.get("http://localhost:7744/api/report/ExpenseTable?StartDate="+$("#txtDateFrom").val()+"-01"+"&EndDate=" + $("#txtDateTo").val() + "-01", function (data) {
        var headerHtml = "<tr>";
        //Start with the headings

        headerHtml += "<td>Category</td>";

        for (var i = 0; i < data.Headers.length; i++) {
            headerHtml += "<td style='text-align:right'>" + data.Headers[i] + "</td>";
        }

        headerHtml += "</tr>";

        var bodyHtml = "";

        //Now do the rows
        for (var i = 0; i < data.Rows.length; i++) {
            bodyHtml += "<tr>";

            bodyHtml += "<td style='font-weight:bold'>" + data.Rows[i].CategoryName + "</td>";

            //And the values
            for (var j = 0; j < data.Rows[i].Values.length; j++) {
                bodyHtml += "<td style='text-align:right'>" + data.Rows[i].Values[j] + "</td>";
            }

            bodyHtml += "</td>";
        }

        //Done :) Let's create the table

        $("#tblExpenseReport thead").html(headerHtml);
        $("#tblExpenseReport tbody").html(bodyHtml);

        $("#divActualReport").css("display", "block");

    });

    $.get("http://localhost:7744/api/report/IncomeTable?StartDate=" + $("#txtDateFrom").val() + "-01" + "&EndDate=" + $("#txtDateTo").val() + "-01", function (data) {
        var headerHtml = "<tr>";
        //Start with the headings

        headerHtml += "<td>Category</td>";

        for (var i = 0; i < data.Headers.length; i++) {
            headerHtml += "<td style='text-align:right'>" + data.Headers[i] + "</td>";
        }

        headerHtml += "</tr>";

        var bodyHtml = "";

        //Now do the rows
        for (var i = 0; i < data.Rows.length; i++) {
            bodyHtml += "<tr>";

            bodyHtml += "<td style='font-weight:bold'>" + data.Rows[i].CategoryName + "</td>";

            //And the values
            for (var j = 0; j < data.Rows[i].Values.length; j++) {
                bodyHtml += "<td style='text-align:right'>" + data.Rows[i].Values[j] + "</td>";
            }

            bodyHtml += "</td>";
        }

        //Done :) Let's create the table

        $("#tblIncomeReport thead").html(headerHtml);
        $("#tblIncomeReport tbody").html(bodyHtml);

    });

}