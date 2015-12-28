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

    if (text.length == 0)
    {
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
    var subCat = { ID: -1, Name: text, Active: true };

    currentSubcats.push(subCat);
}

function openNewSubcatInterface()
{

    clearCatInterface();
    $("#mdlCats").openModal();
}

function clearCatInterface() {
    //Load it to a pristene stage
    $("#ulCat > li").not(':first').not(':last').remove();

    $("#txtCatName").text("...");
    $("#newSubcat").text("...");

    currentSubcats = [];
    selectedCat = null;

    $("#chkActive").prop("checked", "checked");
    $("#ckCatExpense").prop("disabled", "");
}

///Reads and returns the category and all it's details
function readCategory()
{
    var cat = new Object();

    if (selectedCat!= null)
    {
        cat.ID = selectedCat.ID;
    }
    else 
    {
        cat.ID = -1;
    }

    //Read the details
    cat.Name = $("#txtCatName").text();
    cat.Active = $("#chkActive").prop("checked");
    cat.IsExpense = $("#chkCatExpense").prop("checked");

    cat.Subcats = currentSubcats;

    return cat;
}

function validateCategory(cat)
{
    if (cat.Name.length == 0 || cat.Name == "...")
    {
        Materialize.toast('Category Name Required', 2000);
        return false;
    }

    if (cat.Subcats.length == 0)
    {
        Materialize.toast('At least one subcategory is required', 2000);
        return false;
    }

    return true;
}

///Saves and closes
function SaveAndClose()
{
    var cat = readCategory();

    if (!validateCategory(cat))
    {
        return;
    }
    //Save!

    $.post("http://localhost:7744/api/cat/Categories", cat, function (data)
    {
        Materialize.toast('Entry Saved', 2000);

        loadCategories();

        closeCatModal();
    });
}


function loadCategoryForEditing(id)
{
    //Clear the cat interface
    clearCatInterface();

    //Now get the details
    $.get("http://localhost:7744/api/cat/Categories?ID=" + id, function (data)
    {
        selectedCat = data;
        currentSubcats = [];

        //Popualte the details
        $("#txtCatName").text(data.Name);
        $("#chkActive").prop("checked", data.Active);

        $("#ckCatExpense").prop("checked", data.IsExpense);
        $("#ckCatExpense").prop("disabled", "disabled");

        //Create subcats
        for(var i=0; i < data.Subcats.length; i++)
        {
            $("#newSubcat").text("..."); //This is a dirty hack so we get by the 'subcat needs to have a value' issue
            newSubcat();

            currentSubcats.pop(); //remove the duplicate valuie 'newsubcat' creates

            //Then populate the details
            $("#sc" + i).text(data.Subcats[i].Name);

            //Active?
            if (data.Subcats[i].Active)
            {
                $("#sc" + i).css("text-decoration", "");
            }
            else 
            {
                $("#sc" + i).css("text-decoration", "line-through");
            }
            currentSubcats.push(data.Subcats[i]);

        }
        $("#newSubcat").text("...");

        //OPen it up!
        $("#mdlCats").openModal();

    });
}

function deleteSubcat(index) {
    //Is the subcat one of those existing ones?
    var subCat = currentSubcats[index];

    if (subCat.ID != -1) {
        //Was it an inactive one already?
        if (subCat.Active) {
            //It's an existing one, we need to set the subcat to inactive instead
            $("#sc" + index).css("text-decoration", "line-through");
            subCat.Active = false;
        }
        else {
            //Toggle back on
            $("#sc" + index).css("text-decoration", "");
            subCat.Active = true;
        }

    }
    else {
        //Just null the Nth one - then when we read them server-side we decide whether we need to actually delete some subcats
        currentSubcats[index] = null;

        //Delete the control
        $($("#sc" + index).parent().parent()).remove();
    }
}

function loadCategories() {
    $.get("http://localhost:7744/api/cat/Categories", function (data) {
        //build up the html
        var html = "";
        $.each(data, function (index, val) {
            html += '<ul class="collection with-header" onclick="loadCategoryForEditing('+ val.ID +')">';

            if (val.Active) {
                html += '<li class="collection-header"><h5>' + val.Name;
            }
            else {
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
                else {
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