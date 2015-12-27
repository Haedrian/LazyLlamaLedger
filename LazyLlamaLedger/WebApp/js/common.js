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