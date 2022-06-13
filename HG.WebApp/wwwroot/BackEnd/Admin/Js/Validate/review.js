var review = function () {
    return {
        init: function () {
        },
        validateForm: function () {
            $("#review__linting").validate({
                rules:
            {
                    Title:
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
                    Title:
                    {
                        required: "Bạn chưa nhập tên chủ đề"
                    },
                    Description:
                    {
                        required: "Bạn chưa nhập mô tả"
                    }
                   
                }
            });
        }
    };
}();
$(function () { review.init(); });


