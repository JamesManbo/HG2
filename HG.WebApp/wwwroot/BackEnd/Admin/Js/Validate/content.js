var content = function () {
    return {
        init: function () {
        },
        validateForm: function () {
            $("#formcontent").validate({
                rules:
            {
                    title:
                    {
                        required: true
                    },
                    title_Bold:
                    {
                        required: true
                    },
                    Des1_Bold:
                    {
                    required: true
                    },
                    Des1_Normal:
                    {
                        required: true
                    },
                    Des2_Bold:
                    {
                        required: true
                    },
                    Des2_Normal:
                    {
                        required: true
                    },
                    Des3_Bold:
                    {
                        required: true
                    },
                    Des3_Normal:
                    {
                        required: true
                    },
                    info__footer1:
                    {
                        required: true
                    },
                    info__footer2:
                    {
                        required: true
                    },
                    info__footer3:
                    {
                        required: true
                    },
                    info__footer4:
                    {
                        required: true
                    },
                    info__footer5:
                    {
                        required: true
                    },
                    info__footer7:
                    {
                        required: true
                    },
                    info__footer8:
                    {
                        required: true
                    }
            },
                messages:
                {
                    title:
                    {
                        required: "Bạn chưa tiêu đề"
                    },
                    title_Bold:
                    {
                        required: "Bạn chưa tiêu đề"
                    },
                    Des1_Bold:
                    {
                        required: "Bạn chưa nhập mô tả"
                    },
                    Des1_Normal:
                    {
                        required: "Bạn chưa nhập mô tả"
                    },
                    Des2_Bold:
                    {
                        required: "bạn chưa nhập mô tả"
                    },
                    Des2_Normal:
                    {
                        required: "bạn chưa nhập mô tả"
                    },
                    Des3_Normal:
                    {
                        required: "bạn chưa nhập mô tả"
                    },
                    Des3_Bold:
                    {
                        required: "bạn chưa nhập mô tả"
                    },
                    info__footer1:
                    {
                        required: "Bạn chưa client & partners"
                    },
                    info__footer2:
                    {
                        required: "Bạn chưa nhập footer"
                    },
                    info__footer3:
                    {
                        required: "Bạn chưa nhập footer"
                    },
                    info__footer4:
                    {
                        required: "Bạn chưa nhập footer"
                    },
                    info__footer5:
                    {
                        required: "Bạn chưa nhập footer"
                    },
                    info__footer6:
                    {
                        required: "Bạn chưa nhập footer"
                    },
                    info__footer7:
                    {
                        required: "Bạn chưa nhập footer"
                    },
                    info__footer8:
                    {
                        required: "Bạn chưa nhập footer"
                    }
                }
            });
        }
    };
}();
$(function () { content.init(); });


