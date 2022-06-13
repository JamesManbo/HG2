var category = function () {
    return {
        init: function () {
        },
        validateForm: function () {
            $("#formcategory").validate({
                rules:
            {
                CName:
                {
                    required: true
                },
                CDes:
                {
                    required: true
                },
                Status:
                {
                    required: true
                }
            },
                messages:
                {
                    CName:
                    {
                        required: "Bạn chưa nhập tên danh mục"
                    },
                    CDes:
                    {
                        required: "Bạn chưa nhập mô tả danh mục"
                    },
                    Status:
                    {
                        required: "Bạn chưa chọn trạng thái danh mục"
                    }
                }
            });
        }
    };
}();
$(function () { category.init(); });


