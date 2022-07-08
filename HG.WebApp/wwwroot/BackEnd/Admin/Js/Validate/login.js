var login = function () {
    return {
        init: function () {
        },
        validateForm: function () {
            $("#formlogin").validate({
                rules:
            {
                    UserName:
                {
                    required: true
                },
                    PassWord:
                {
                        required: true,
                        pwcheck: true,
                        minlength: 1
                }
            },
                messages:
                {
                    UserName:
                    {
                        required: "Bạn chưa nhập tên đăng nhập"
                    },
                    PassWord:
                    {
                        required: "Bạn chưa nhập mật khẩu",
                        //pwcheck: "Mật khẩu cần một chữ hoa, chữ thường và một chữ số",
                        minlength: "Mật khẩu cần ít nhất 1 ký tự!"
                    }
                }
            });
            //$.validator.addMethod("pwcheck",
            //    function (value, element) {
            //        return /^[A-Za-z0-9\d=!\-@._*]+$/.test(value);
            //});
        }
    };
}();
$(function () { login.init(); });


