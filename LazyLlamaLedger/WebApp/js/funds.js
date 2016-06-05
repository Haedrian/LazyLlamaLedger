var loadedFund;

function loadFunds()
{
    $.get("http://localhost:7744/api/funds", function(data)
    {
        var html = "";

            data.forEach(function (element)
            {
                var total = "";

                if (total < 0)
                {
                    total = "<td style='text-align:right;color:red'>" + element.Total.toFixed(2) + "</td>"
                }
                else
                {
                    total = "<td style='text-align:right;color:green'>" + element.Total.toFixed(2) + "</td>"
                }

                html += "<tr><td style='width:20px'> <div style='border: 1px groove black;width:20px;height:20px;background-color:" + element.Colour + "';></td><td>" + element.Name + "</td><td>" + (element.IsActive ? "Active" : "Inactive") + "</td><td>" + element.Percentage + "</td><td>" + element.MinimumAmount + "</td><td>" + element.MaximumAmount + "</td>" + total + "<td><i style='cursor:pointer' onclick='openEditFund(\"" + element.Name + "\")' class='material-icons'>settings</i></td><td><i style='cursor:pointer' onclick='openAdjustFund(\"" + element.Name + "\")' class='material-icons'>compare_arrows</i></td></tr>";
        });

        $("#tblFund tbody").html(html);

        $("#fundLoading").css("display", "none");

        //Remove colours which we have already
        var colours =
            [
                { key: "#6d7889", value: "Bluegrey" },
                { key: "#ea6459", value: "Papaya" },
                { key: "#1c5f5f", value: "Teal" },
                { key: "#3c0d22", value: "Plum" },
                { key: "#46784e", value: "Forest Green" },
                { key: "#482a43", value: "Royal Purple" },
                { key: "#FFA500", value: "Orange" },
                { key: "#673AB7", value: "Deep Purple" },
                { key: "#5C6BC0", value: "Indigo" },
                { key: "#2196F3", value: "Sea Blue" },
                { key: "#43A047", value: "Sea Green" },
                { key: "#CDDC39", value: "Lime" },
                { key: "#6D4C41", value: "Leather" },
                { key: "#37474F", value: "Nightsky" }
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

function openAdjustFund(fundName)
{
    //Load the details for that fund
    $.get("http://localhost:7744/api/funds?fundName=" + fundName, function (fund)
    {
        loadedFund = fund;
        $("#txtTransferAdjustment").val("");

        $("#lblTransferFundName").html(fund.Name);
        $("#divTransferFundColour").css("background-color", fund.Colour);
        $("#h5FundCurrentAmount").html(fund.Total.toFixed(2));
        $("#h5FundNewAmount").html(fund.Total.toFixed(2));

        $("#mdlTransferFunds").openModal();
    });
}

function determineTotal()
{
    var prev = loadedFund.Total;
    var adjustment = Number($("#txtTransferAdjustment").val());

    if (isNaN(adjustment))
    {
        $("#h5FundNewAmount").html("---");
    }
    else
    {
        $("#h5FundNewAmount").html((prev + adjustment).toFixed(2));
    }
}

function makeAdjustment()
{
    var prev = loadedFund.Total;
    var adjustment = Number($("#txtTransferAdjustment").val());

    if (isNaN(adjustment))
    {
        Materialize.toast("Adjustment amount is not a valid number", 2000);
    }
    else 
    {
        var newTotal = prev + adjustment;

        //Do it
        $.ajax(
        {
            url: "http://localhost:7744/api/funds?fundName=" + loadedFund.Name + "&newTotal=" + newTotal,
            method: "PATCH"
        })
    .done(function (data)
    {
        //Done 
        $('#mdlTransferFunds').closeModal();
        //reload the funds
        loadFunds();

    }).fail(function (data)
    {
        Materialize.toast(data.responseJSON.Message, 2000);
    });
    }
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

function openEditFund(fundName)
{
    //Load the details for that fund
    $.get("http://localhost:7744/api/funds?fundName=" + fundName, function (fund)
    {
        if (fund)
        {
            loadedFund = fund;

            //Load it up
            var titleHtml = fund.Name + " : " + fund.Total.toFixed(2);

            $("#mdlEditFund #lblFundName").html(titleHtml);
            $("#mdlEditFund #divEditFundColour").css("background-color", fund.Colour);

            $("#mdlEditFund #txtEditMinAmount").val(fund.MinimumAmount);
            $("#mdlEditFund #txtEditPercentage").val(fund.Percentage);
            $("#mdlEditFund #txtEditMaxAmount").val(fund.MaximumAmount);

            $("#mdlEditFund #cbEditOnNegative").prop("checked", fund.MinimumIfNegative);
            $("#mdlEditFund #cbEditFundActive").prop("checked", fund.IsActive);

            $("#mdlEditFund").openModal();
        }
        else 
        {
            Materialize.toast("Couldn't find fund");
        }
    });
        
 
}

function updateFund()
{
    var fund = readUpdateFund();

    //Validate it
    if (fund.MinimumAmount.length > 0) {
        if (isNaN(parseFloat(fund.MinimumAmount))) {
            Materialize.toast("Minimum amount is not a valid number", 2000);
            return;
        }
    }

    if (fund.MaximumAmount.length > 0) {
        if (isNaN(parseFloat(fund.MaximumAmount))) {
            Materialize.toast("Maximum amount is not a valid number", 2000);
            return;
        }
    }

    if (fund.Percentage.length > 0) {
        if (isNaN(parseFloat(fund.Percentage))) {
            Materialize.toast("Percentage is not a valid number", 2000);
            return;
        }
    }

    //Okay it's fine

    $.ajax(
        {
            url: "http://localhost:7744/api/funds?fundName="+fund.Name,
            method: "PUT",
            data: fund
        })
    .done(function (data)
    {
        //Done 

        $('#mdlEditFund').closeModal();
        //reload the funds
        loadFunds();

    }).fail(function (data) {
        Materialize.toast(data.responseJSON.Message, 2000);
    });
}

function readUpdateFund()
{
    var fund = new Object();

    fund.Name = loadedFund.Name;
    fund.MinimumAmount = $("#mdlEditFund #txtEditMinAmount").val();
    fund.Percentage = $("#mdlEditFund #txtEditPercentage").val();
    fund.MaximumAmount = $("#mdlEditFund #txtEditMaxAmount").val();
    fund.MinimumIfNegative = $("#mdlEditFund #cbEditOnNegative").prop("checked");
    fund.IsActive = $("#mdlEditFund #cbEditFundActive").prop("checked");

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