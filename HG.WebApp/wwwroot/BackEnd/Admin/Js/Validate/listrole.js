var listting = function () {
    return {
        init: function () {
        },
        validateForm: function () {
            $("#formrole").validate({
                rules:
            {
                Name:
                {
                    required: true
                },
                Description:
                {
                    required: true
                }
            },
                messages:
                {
                    Name:
                    {
                        required: "Bạn chưa nhập tên nhóm quyền"
                    },
                    Description:
                    {
                        required: "Bạn chưa nhập tên miêu tả"
                    }
                }
            });
        }
    };
}();
$(function () { listrole.init(); });


