$(document).on('keypress', '[contenteditable=true]', function (e) {
    return e.which != 13;
});

$(document).on('paste','[contenteditable=true]',function (e)
{
    e.preventDefault();
})