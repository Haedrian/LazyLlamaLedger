function loadFunds()
{
    $.get("http://localhost:7744/api/funds", function(data)
    {
        var html = "";

        data.forEach(function (element)
        {
            html += "<tr><td style='color:" + element.Colour + "'>" + element.Name + "</td><td>" + element.Percentage + "</td><td>" + element.MinimumAmount + "</td><td>" + element.MaximumAmount + "</td><td>" + Total + "</td></tr>";
        });

        $("#tblFund tbody").html(html);

        $("#fundLoading").css("display", "none");
    });
}