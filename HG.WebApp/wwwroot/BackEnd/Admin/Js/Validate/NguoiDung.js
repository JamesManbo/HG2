var NguoiDung = function () {
    return {
        init: function () {
        },
        validateForm: function () {
            $("#FNguoiDung").validate({
                rules:
            {
                    UserName:
                    {
                         required: true
                     },
                    ho_dem:
                    {
                         required: true
                    },
                    ten:
                    {
                         required: true
                    }
            },
                messages:
                {
                    UserName:
                    {
                        required: "Bạn cần nhập tên đăng nhập!"
                    },
                    ho_dem:
                    {
                        required: "Bạn cần nhập họ đệm!"
                    },
                    ten:
                    {
                        required: "Tên không được trống!"
                    }

                }
            });
            $.validator.addMethod("pwcheck",
                function (value, element) {
                    return /^[A-Za-z0-9\d=!\-@._*]+$/.test(value);
                });
        }
    };
}();
$(function () { NguoiDung.init(); });


