var modalForm = function () {
    return {
        init: function () {
        },
        validateForm: function () {
            $("#addTagModalForm").validate({
                rules:
            {
                tagName:
                {
                    required: true
                },
               
            },
                messages:
                {
                    tagName:
                    {
                        required: "Bạn chưa nhập tag !"
                    }
                }
            });
        }
    };
}();
$(function () { modalForm.init(); });


