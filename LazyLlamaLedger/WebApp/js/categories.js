///Holds the current subcats so we can keep them in mind
var currentSubcats = [];

var selectedCat = null;

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
    $("#newSubcat").text("");

    //Add it to the list
    var subCat = { ID: null, Name: text, Active:true };

    currentSubcats.push(subCat);
}

function openNewSubcatInterface() {
    //Load it to a pristene stage
    $("#ulCat > li").not(':first').not(':last').remove();

    $("#txtCatName").text("...");
    $("#newSubcat").text("...");

    $("#mdlCats").openModal();

    $("#chkActive").prop("checked", "checked");

    currentSubcats = [];
    selectedCat = null;
}

function deleteSubcat(index) {
    //Just null the Nth one - then when we read them server-side we decide whether we need to actually delete some subcats
    currentSubcats[index] = null;

    //Delete the control
    $($("#sc" + index).parent().parent()).remove();
}

function loadCategories() {
    $.get("http://localhost:7744/api/cat/Categories", function (data) {
        //build up the html
        var html = "";
        $.each(data, function (index, val) {
            //TODO: ATTACH EVENT
            html += '<ul class="collection with-header">';

            if (val.Active) {
                html += '<li class="collection-header"><h5>' + val.Name;
            }
            else
            {
                html += '<li class="collection-header"><h5 style="text-decoration:line-through">' + val.Name;
            }

            if (val.IsExpense) {
                html += "<i class='material-icons' style='color:red;padding-left:10px'>attach_money</i>";
            }
            else {
                html += "<i class='material-icons' style='color:green;padding-left:10px'>attach_money</i>";
            }

            html += '<i style="float:right" class="material-icons">edit</i></h5></li>';

            $.each(val.Subcats, function (i, subcat) {
                if (subcat.Active) {
                    html += '<li class="collection-item">' + subcat.Name + '</li>';
                }
                else 
                {
                    html += '<li style="text-decoration:line-through" class="collection-item">' + subcat.Name + '</li>';
                }
            });

            html += '</ul>';
        });

        $("#cats").html(html);
    });
}

$(document).on('focusout', '[contenteditable=true]', function (e) {
    var element = $(this);
    if (!element.text().trim().length) {
        element.text("...");
    }
});

$(document).on('focusin', '[contenteditable=true]', function (e) {
    var element = $(this);
    if (element.text() == "...") {
        element.text("");
    }
});

$(document).on('keypress', '[contenteditable=true]', function (e) {
    return e.which != 13;
});

$(document).on('paste', '[contenteditable=true]', function (e) {
    e.preventDefault();
})

function closeCatModal() {
    $("#mdlCats").closeModal();
}