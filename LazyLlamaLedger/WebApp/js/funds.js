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

function openAddFund()
{
    $('#mdlFunds').openModal();
}

$(document).ready(function ()
{
    var colours =
        [
            { key: "#6d7889", value: "Bluegrey" },
            { key: "#dffffe", value: "Light Mint" },
            { key: "#fef0f4", value: "Light Pink" },
            { key: "#ea6459", value: "Papaya" },
            { key: "#1c5f5f", value: "Teal" },
            { key: "#3c0d22", value: "Plum" },
            { key: "#46784e", value: "Forest Green" },
            { key: "#482a43", value: "Royal Purple" },
            { key: "#FFA500", value: "Orange" }
        ];

    colours.forEach(function (element)
    {
        $('#ddlColour')
          .append($('<option>', { value: element.key })
          .text(element.value));
    });

    $("#ddlColour").material_select();
});