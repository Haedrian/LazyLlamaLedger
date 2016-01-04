function getAndCreateExpenseTable() {
    $.get("http://localhost:7744/api/report/ExpenseTable?StartDate=2015-12-01&EndDate=2016-01-01", function (data)
    {
        var headerHtml = "<tr>";
        //Start with the headings

        headerHtml += "<td>Category</td>";

        for (var i = 0; i < data.Headers.length; i++)
        {
            headerHtml += "<td>" + data.Headers[i] + "</td>";
        }

        headerHtml += "</tr>";

        var bodyHtml = "";

        //Now do the rows
        for(var i=0; i < data.Rows.length; i++)
        {
            bodyHtml += "<tr>";

            bodyHtml += "<td style='font-weight:bold'>" + data.Rows[i].CategoryName + "</td>";

            //And the values
            for (var j = 0; j < data.Rows[i].Values.length; j++)
            {
                bodyHtml += "<td>" + data.Rows[i].Values[j] + "</td>";
            }

            bodyHtml += "</td>";
        }

        //Done :) Let's create the table

        $("#tblExpenseReport thead").html(headerHtml);
        $("#tblExpenseReport tbody").html(bodyHtml);

    });
}