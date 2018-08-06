$(function () {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function (e) {
        $("#MyModalContent").load(this.href, function () {
            $("#MyModal").modal({
                //backdrop: 'static',
                keyboard: false
            }, 'show');
            bindForm(this);
        });
        return false;
    });
});

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        //event.preventDefault();
        var formData = new FormData($(this)[0]);
        var val = $("button[type=submit][clicked=true]").val();
        $("#btnSubmit").hide();
        formData.append("submit", val);
        
        $.ajax({
            url: this.action,
            type: this.method,
            data: formData,// $(this).serialize(),//data: formData,$(this).serializeArray()
            processData: false,
            contentType: false,
            success: function (result) {
                if (result.success) {
                    $("#MyModal").modal('hide');
                    toastr.success(result.message);
                    tableDraw();
                    //location.reload();
                }
                else {
                    $("#MyModalContent").html(result);
                    //$("button[type=submit]").show();
                    $("#MyModal").modal('show');
                   
                    toastr.error(result.ErrorMessage);
                    bindForm(dialog);
                }
            },
            error: function (xml, message, text) {
                $("#btnSubmit").show();

                toastr.error("Msg: " + message + ", Text: " + text);
            }
        });
        return false;
    });
}