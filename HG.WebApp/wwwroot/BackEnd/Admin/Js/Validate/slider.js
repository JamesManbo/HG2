var slider = function () {
    return {
        init: function () {
        },
        validateForm: function () {
            $("#formslider").validate({
                rules:
            {
                SliderName:
                {
                    required: true
                },
                SliderUrl:
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
                    SliderName:
                    {
                        required: "Bạn chưa nhập tên slider"
                    },
                    SliderUrl:
                    {
                        required: "Bạn chưa nhập ảnh"
                    },
                    Status:
                    {
                        required: "Bạn chưa chọn trạng thái"
                    }
                }
            });
        }
    };
}();
var slider2 = function () {
    return {
        init: function () {
        },
        validateForm: function () {
            $("#formEditslider").validate({
                rules:
                {
                    ESliderName:
                    {
                        required: true
                    },
                    ESliderUrl:
                    {
                        required: true
                    },
                    EStatus:
                    {
                        required: true
                    }
                },
                messages:
                {
                    ESliderName:
                    {
                        required: "Bạn chưa nhập tên slider"
                    },
                    ESliderUrl:
                    {
                        required: "Bạn chưa nhập ảnh"
                    },
                    EStatus:
                    {
                        required: "Bạn chưa chọn trạng thái"
                    }
                }
            });
        }
    };
}();

$(function () { slider.init(); });
$(function () { slider2.init(); });


