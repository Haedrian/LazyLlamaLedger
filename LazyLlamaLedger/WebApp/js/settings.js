function resetFile() {
    //We sure?
    if (confirm("Are you sure you want to reset file location?"))
    {
        $.post("http://localhost:7744/api/settings/ResetFileLocation", function (data)
        {
            Materialize.toast('File Location Reset. Please restart your server', 2000);
        });
    }
}

function settingsAddCatStarterPack()
{
    //Do it
    $.post("http://localhost:7744/api/cat/loadCategoryStarterPack", function (data)
    {
        //Close the popup
        Materialize.toast('Category Starter Pack Loaded', 2000);
    });
}