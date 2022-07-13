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
                        required: true,
                        ma_thu_tuc_vaild: true,
                        tieng_viet_vaild: true
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

            jQuery.validator.addMethod("ma_thu_tuc_vaild", function (value, element) {
                if (/[ `!#$%^&*()_+\-=\[\]{};':"\\|,<>\/?~]/.test(value)) {
                    return false;
                } else {
                    return true;
                };
            }, "Mã không nhập các kí tự đặc biệt [ `!#$%^&*()_+\-=\[\]{};':\\|,<>\/?~]");

            jQuery.validator.addMethod("tieng_viet_vaild", function (value, element) {
                if ("ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ".includes(value)) {
                    return false;
                } else {
                    return true;
                };
            }, "Mã không nên nhập tiếng việt");
        }
    };
}();
$(function () { NguoiDung.init(); });


