$(document).ready(function () {
    getCurrentMonth();

    $("#txtDate").datepicker();
    $("#txtDate").datepicker("option", "dateFormat", "d M y");
});


function getCurrentMonth()
{
    //$.ajax({
    //    url: "/ledger/Get?Month=" + (new Date()).getMonth - 1,
    //    success: function (data)
    //    {
    //        alert(data);
    //    },
        
    //});

    $.get("http://localhost:7744/api/ledger/Get?Month=" + ((new Date()).getMonth() + 1), function (data)
    {
        //Populate a table
        var html = "";

        $.each(data,function (index, val)
        {
            html += "<tr><td>" + val.Item + "</td><td>" + val.Date + "</td><td> " + val.Category + "</td><td>" + val.SubCategory + "</td><td style='text-align:right'>" + val.Money + "</td><tr>";
        });

        $("#tblLedger tbody").html(html);

    },"json");
}