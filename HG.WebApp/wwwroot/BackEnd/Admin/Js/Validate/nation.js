var nation = function () {
    return {
        init: function () {
        },
        validateForm: function () {
            $("#formnation").validate({
                rules:
            {
                Name:
                {
                    required: true
                },
                    NameA:
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
                    Name:
                    {
                        required: "Bạn chưa nhập tên quốc gia"
                    },
                    NameA:
                    {
                        required: "Bạn chưa nhập tên viết tắt quốc gia"
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
$(function () { nation.init(); });


