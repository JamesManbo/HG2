var listting = function () {
    return {
        init: function () {
        },
        validateForm: function () {
            $("#formnews").validate({
                rules:
            {
                    Title:
                {
                    required: true
                },
                    TitleSeo:
                {
                    required: true
                },
                    Phone:
                {
                    required: true
                }
            },
                messages:
                {
                    Title:
                    {
                        required: "Bạn chưa nhập tên listting"
                    },
                    TitleSeo:
                    {
                        required: "Bạn chưa nhập tên url listting"
                    },
                    Phone:
                    {
                        required: "Bạn chưa nhập số điện thoại listting"
                    }
                }
            });
        }
    };
}();
$(function () { listting.init(); });


