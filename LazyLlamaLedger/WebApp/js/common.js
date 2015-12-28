var versionNumber = "0.1";

//This is to fix a bug where the datepicker pops up when you change tab
$(window).on("close", function ()
{
    $('.datepicker').blur();
})

///Closes all sections
function closeAll()
{
    $("#divCategories").css("display", "none");
    $("#divLedger").css("display", "none");
    $("li.active").removeClass("active");
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