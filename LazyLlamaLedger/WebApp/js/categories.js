///Holds the current subcats so we can keep them in mind
var currentSubcats = [];

function newSubcat()
{
    var text = $("#newSubcat").text();

    if (text.length == 0)
    {
        Materialize.toast('Subcategory Name Required', 2000);
        return;
    }

    //TODO: Check for duplicates

    //Determine the id we want for the next subcat
    var id = currentSubcats.length;
    id = "sc" + id;

    var html = '<li style="height:73.5px" class="collection-item"><p><span id="'+id+'" contenteditable="true">'+text+'</span> <i style="float:right" class="material-icons">delete</i></p></li>'

    //Shove this before the editable thingy
    $("#liNewSubcat").before(html);

    //And clear the newsubcat
    $("#newSubcat").text("...");
}

//jQuery(function ($) {
//    $("[contenteditable]").focusout(function () {
//        var element = $(this);
//        if (!element.text().trim().length) {
//            element.text("...");
//        }
//    });
   
//});

$(document).on('focusout', '[contenteditable=true]', function (e) {
    var element = $(this);
    if (!element.text().trim().length) {
        element.text("...");
    }
});

$(document).on('keypress', '[contenteditable=true]', function (e) {
    return e.which != 13;
});

$(document).on('paste','[contenteditable=true]',function (e)
{
    e.preventDefault();
})