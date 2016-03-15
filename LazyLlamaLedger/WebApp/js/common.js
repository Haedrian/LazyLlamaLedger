var versionNumber = "3.0";

//This is to fix a bug where the datepicker pops up when you change tab
$(window).on("close", function ()
{
    $('.datepicker').blur();
});

$(document).ready(function ()
{
    isFirstTime();
});

function isFirstTime()
{
    $.get("http://localhost:7744/api/cat/FirstTime", function (data)
    {
        if (!data)
        {
            //Carry on
        }
        else 
        {
            //First time, pop it up!
            $("#mdlFirstRun").openModal();
        }
    });
}

function loadCategoryStarterPack()
{
    $.post("http://localhost:7744/api/cat/loadCategoryStarterPack", function (data)
    {
        //Close the popup
        Materialize.toast('Category Starter Pack Loaded', 2000);
        $("#mdlFirstRun").closeModal();
    });
}

///Closes all sections
function closeAll()
{
    $("#divCategories").css("display", "none");
    $("#divLedger").css("display", "none");
    $("#divReports").css("display", "none");
    $("li.active").removeClass("active");
}

function openReports()
{
    closeAll();
    $("#divReports").css("display", "block");
    $("liReports").addClass("active");
}

function openLedger()
{
    closeAll();
    $("#divLedger").css("display", "block");
    $("#liLedger").addClass("active");
}

function openCats()
{
    closeAll();
    loadCategories();

    $("#divCategories").css("display", "block");
    $("#liCategories").addClass("active");
}

function openAbout()
{
    $("#txtVersion").text("Version " + versionNumber);
    $("#mdlAbout").openModal();
}