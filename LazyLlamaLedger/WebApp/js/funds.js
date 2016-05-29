function loadFunds()
{
    $.get("http://localhost:7744/api/funds", function(data)
    {
        var html = "";

            data.forEach(function (element)
        {
            html += "<tr><td style='width:30px'> <div style='width:30px;height:30px;background-color:"+element.Colour+"';></td><td>" + element.Name + "</td><td>" + element.Percentage + "</td><td>" + element.MinimumAmount + "</td><td>" + element.MaximumAmount + "</td><td style='text-align:right'>" + element.Total + "</td></tr>";
        });

        $("#tblFund tbody").html(html);

        $("#fundLoading").css("display", "none");

        //Remove colours which we have already
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

        for (var i = 0; i < colours.length; i++)
        {
            for (var j=0 ; j < data.length; j++)
            {
                if (colours[i].key == data[j].Colour)
                {
                    //Remove
                    colours.splice(i, 1);
                }
            }
        }

        $("#ddlColour").html(""); //Clear it

        colours.forEach(function (element)
        {
            $('#ddlColour')
              .append($('<option>', { value: element.key })
              .text(element.value));
        });

        $("#ddlColour").material_select();
    });
}

function openAddFund()
{
    $('#mdlFunds').openModal();
}

function readFund()
{
    var fund = new Object();

    fund.Name = $("#mdlFunds #txtFundName").val();
    fund.Colour = $("#mdlFunds #ddlColour").val();
    fund.MinimumAmount = $("#mdlFunds #txtMinAmount").val();
    fund.Percentage = $("#mdlFunds #txtPercentage").val();
    fund.MaximumAmount = $("#mdlFunds #txtMaxAmount").val();
    fund.MinimumIfNegative = $("#mdlFunds #cbOnNegative").prop("checked");
    fund.Total = $("#mdlFunds #txtFundSeed").val();

    return fund;
}

function saveFund()
{
    var fund = readFund();

    //Validate it
    if (fund.Name.length == 0)
    {
        Materialize.toast('Name is missing', 2000);
        return;
    }

    if (fund.MinimumAmount.length > 0)
    {
        if (isNaN(parseFloat(fund.MinimumAmount)))
        {
            Materialize.toast("Minimum amount is not a valid number", 2000);
            return;
        }
    }

    if (fund.MaximumAmount.length > 0)
    {
        if (isNaN(parseFloat(fund.MaximumAmount)))
        {
            Materialize.toast("Maximum amount is not a valid number", 2000);
            return;
        }
    }

    if (fund.Percentage.length > 0)
    {
        if (isNaN(parseFloat(fund.Percentage)))
        {
            Materialize.toast("Percentage is not a valid number", 2000);
            return;
        }
    }

    if (fund.Total.length > 0)
    {
        if (isNaN(parseFloat(fund.Total)))
        {
            Materialize.toast("Total is not a valid number", 2000);
            return;
        }
    }

    if (fund.MinimumAmount.length + fund.MaximumAmount.length + fund.Percentage.length == 0)
    {
        Materialize.toast("Minimum Amount, Maximum Amount or Percentage must be supplied");
        return;
    }

    //Okay, we should be fine - post it
    $.ajax(
        {
            url: "http://localhost:7744/api/funds",
            method: "POST",
            data: fund
        })
    .done(function (data)
    {
        //Done - clear the fields
        $("#mdlFunds #txtFundName").val("");
        $("#mdlFunds #txtMinAmount").val("");
        $("#mdlFunds #txtPercentage").val("");
        $("#mdlFunds #txtMaxAmount").val("");
        $("#mdlFunds #cbOnNegative").prop("checked",false);
        $("#mdlFunds #txtFundSeed").val("");

        $('#mdlFunds').closeModal();
        //reload the funds
        loadFunds();
        
    }).fail(function (data)
    {
        Materialize.toast(data.responseJSON.Message, 2000);
    });

}