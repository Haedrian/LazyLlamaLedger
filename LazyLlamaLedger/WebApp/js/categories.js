///Holds the current subcats so we can keep them in mind
var currentSubcats = [];

$(document).ready(function () {
    $("#newSubcat").on("keydown", function (e) {
        var charCode = e.which || e.keyCode;

        if (charCode == 9) {
            //newsubcat!
            newSubcat();

            return false;
        }

    });
});

function newSubcat() {
    var text = $("#newSubcat").text();

    if (text.length == 0) {
        Materialize.toast('Subcategory Name Required', 2000);
        return;
    }

    //TODO: Check for duplicates

    //Determine the id we want for the next subcat
    var id = currentSubcats.length;
    id = "sc" + id;

    var html = '<li style="height:73.5px" class="collection-item"><p><span id="' + id + '" contenteditable="true">' + text + '</span> <i onclick="deleteSubcat(' + currentSubcats.length + ');" style="float:right" class="material-icons">delete</i></p></li>'

    //Shove this before the editable thingy
    $("#liNewSubcat").before(html);

    //And clear the newsubcat
    $("#newSubcat").text("...");

    //Add it to the list
    var subCat = { ID: null, Name: text };

    currentSubcats.push(subCat);
}

function deleteSubcat(index)
{
    //Just null the Nth one - then when we read them server-side we decide whether we need to actually delete some subcats
    currentSubcats[index] = null;

    //Delete the control
    $($("#sc" + index).parent().parent()).remove();
}

$(document).on('focusout', '[contenteditable=true]', function (e) {
    var element = $(this);
    if (!element.text().trim().length) {
        element.text("...");
    }
});

$(document).on('keypress', '[contenteditable=true]', function (e) {
    return e.which != 13;
});

$(document).on('paste', '[contenteditable=true]', function (e) {
    e.preventDefault();
})