using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace HG.WebApp.Helper
{
    public class HelperString
    {
        public int PageStep = 0;
        public int CurrentPage = 0;
        public string LinkPage = "";
        public int TotalPage = 0;

        private static readonly string[] VietnameseSigns = new string[]
        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };
        public static string RemoveSign4VietnameseString(string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }
        /// <summary>
        /// Chuyển đổi chữ tiếng việt có dấu sang không dấu
        /// </summary>
        /// <param name="strUnicode">Chuỗi tiếng việt có dấu</param>
        /// <returns>Trả lại một chuỗi không dấu</returns>
        public static string UnsignCharacter(string text)
        {
            var pattern = new string[7];

            pattern[0] = "a|(á|ả|à|ạ|ã|ă|ắ|ẳ|ằ|ặ|ẵ|â|ấ|ẩ|ầ|ậ|ẫ)";

            pattern[1] = "o|(ó|ỏ|ò|ọ|õ|ô|ố|ổ|ồ|ộ|ỗ|ơ|ớ|ở|ờ|ợ|ỡ)";

            pattern[2] = "e|(é|è|ẻ|ẹ|ẽ|ê|ế|ề|ể|ệ|ễ)";

            pattern[3] = "u|(ú|ù|ủ|ụ|ũ|ư|ứ|ừ|ử|ự|ữ)";

            pattern[4] = "i|(í|ì|ỉ|ị|ĩ)";

            pattern[5] = "y|(ý|ỳ|ỷ|ỵ|ỹ)";

            pattern[6] = "d|đ";

            for (int i = 0; i < pattern.Length; i++)
            {

                // kí tự sẽ thay thế

                char replaceChar = pattern[i][0];

                MatchCollection matchs = Regex.Matches(text, pattern[i]);

                foreach (Match m in matchs)
                {

                    text = text.Replace(m.Value[0], replaceChar);

                }

            }

            return text;

        }

        /// <summary>
        /// Giới hạn số lượng từ trong một chuỗi
        /// </summary>
        /// <param name="fullText">Chuỗi truyền vào</param>
        /// <param name="maxLength">số lượng từ</param>
        /// <returns>Chuỗi đã giới hạn từ</returns>
        public static string TruncateByWord(string fullText, int maxLength)
        {
            if (fullText == null || fullText.Length < maxLength)
                return fullText;
            var iNextSpace = fullText.LastIndexOf(" ", maxLength, StringComparison.Ordinal);
            return string.Format("{0}...", fullText.Substring(0, (iNextSpace > 0) ? iNextSpace : maxLength).Trim());
        }

        /// <summary>
        /// Hàm lấy thông tin file ảnh và đường dẫn ảnh
        /// </summary>
        /// <param name="timespan"> Ngày tạo bài viết</param>
        /// <param name="imageFile">Tên file ảnh</param>
        /// <param name="imageSize">Kích thước file ảnh. Lấy thông tin từ hàng số trong Class </param>
        /// <param name="category"> Nhóm tin tức/khuyến mại</param>
        /// <returns> Đường dẫn file ảnh của tin bài</returns>
        //public static string GetImageUrl( int timespan, string imageFile,string imageSize,string category)
        //{
        //    const string startStr = "/Content/FrontEnd/_img_server/";
        //    string result = string.Empty;
        //    if (imageFile != null)
        //    {
        //        result = new StringBuilder(startStr + category + "/" + HelperDateTime.ConvertTimespan2DateTime(timespan).ToString("yyyy") + "/" +
        //                          HelperDateTime.ConvertTimespan2DateTime(timespan).ToString("MM") + "/" +
        //                          HelperDateTime.ConvertTimespan2DateTime(timespan).ToString("dd") + "/" + imageSize + "/" + imageFile).ToString();
        //    }
        //    return result;
        //}
        public static string convertToUnSign2(string? s)
        {
            if (s == null) return "";
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            var res = (sb.ToString().Normalize(NormalizationForm.FormD)).Replace(".", "");
            return res.Replace(" ", "-");
        }

        public string getHtmlPagingNews(string _LinkPage, int _PageStep, int _CurrentPage, int _RowPerPage, int _TotalRow)
        {
            this.PageStep = _PageStep;
            CurrentPage = _CurrentPage;
            LinkPage = _LinkPage;
            if (_RowPerPage == 0)
                _RowPerPage = 5;
            TotalPage = (_TotalRow % _RowPerPage == 0) ? _TotalRow / _RowPerPage : ((_TotalRow - (_TotalRow % _RowPerPage)) / _RowPerPage) + 1;
            return WriteHtmlPagingNews();
        }
        public string WriteHtmlPagingNews()
        {
            string strPageHTML = "<div class=\"dataTables_paginate paging_full_numbers\" id=\"dyntable_paginate\">";
            LinkPage = LinkPage.Split('?')[0];

            if (CurrentPage > PageStep + 1)
            {
                strPageHTML += "<a class=\"first paginate_button paginate_button_disabled\" href=\"" + LinkPage + "?page=1\">Đầu</a>";
                strPageHTML += "<a class=\"first paginate_button paginate_button_disabled\" href=\"" + LinkPage + "?page=" + (CurrentPage - 1) + "\">Trước</a>";
                //strPageHTML += "<li>...</li>";
            }
            int BeginFor = ((CurrentPage - PageStep) > 1) ? (CurrentPage - PageStep) : 1;
            int EndFor = ((CurrentPage + PageStep) > TotalPage) ? TotalPage : (CurrentPage + PageStep);
            for (int pNumber = BeginFor; pNumber <= EndFor; pNumber++)
            {
                strPageHTML += "<span>";
                if (pNumber == CurrentPage)
                    strPageHTML += "<a tabindex=\"0\" class=\"active paginate_active\" href=\"" + "" + "\">" + pNumber + "</a>";
                else
                    strPageHTML += "<a tabindex=\"0\" class=\"active paginate_button\" href=\"" + LinkPage + "?page=" + pNumber + "\">" + pNumber + "</a>";
                strPageHTML += "</span>";
            }
            if (CurrentPage < (TotalPage - PageStep))
            {
                //strPageHTML += "<li>...</li>";
                strPageHTML += "<a class= \"next paginate_button\" href=\"" + LinkPage + "?page=" + (CurrentPage + 1) + "\">Tiếp</a>";
                strPageHTML += "<a class= \"last paginate_button\" href=\"" + LinkPage + "?page=" + TotalPage + "\">Cuối</a>";
            }
            strPageHTML += "</div>";
            if (TotalPage > 1)
                return strPageHTML;
            return string.Empty;
        }
        public static List<ListPageRecords> ListPageRecords()
        {
            List<ListPageRecords> items = new List<ListPageRecords>
            {
               new ListPageRecords { key=5, page = 5 },
               new ListPageRecords { key=10, page = 10 },
               new ListPageRecords { key=15, page = 15 },
               new ListPageRecords { key=20, page = 20 },
               new ListPageRecords { key=25, page = 25 }
            };
            return items;
        }

        public static List<Tempalte> ListTemplate()
        {
            List<Tempalte> items = new List<Tempalte>
            {
               new Tempalte { TempFormat="{{Text#CoQuan#Tên đơn vị đề nghị}}", TempElement = "<input id=\"txtcoquan\" name=\"txtcoquan\"  />" },
               new Tempalte { TempFormat="{{Text#DiaBan#Tên địa bàn}}", TempElement = "<input id=\"txtdiaban\" name=\"txtdiaban\"  />" },
               new Tempalte { TempFormat="{{TextDateTime#Date#Ngày...Tháng..Năm...}}",TempElement =  "<input id=\"txtdiaban\" name=\"txtdiaban\"  />" },
               new Tempalte { TempFormat="{{Text#So#Số}}", TempElement =  "<input id=\"txtSo\" name=\"txtSo\"  placeholder=\"Nhập số\" />" },
               new Tempalte { TempFormat="{{Text#SoTB#Số}}", TempElement =  "<input id=\"txtSoTB\" name=\"txtSoTB\" placeholder=\"Nhập số\" />" },
               new Tempalte { TempFormat="{{Text#DiaBan#T&ecirc;n địa b&agrave;n}}", TempElement =  "<input id=\"txtDiaBan\" name=\"txtDiaBan\" placeholder=\"Nhập địa bàn\" />" },
               new Tempalte { TempFormat="{{TextDateTime#Date#Ng&agrave;y...Th&aacute;ng..Năm...}}", TempElement =  "<input id=\"TextDateTime\" name=\"TextDateTime\" placeholder=\"Nhập ngày tháng năm\"  />" },
               new Tempalte { TempFormat="{{Text#Ten#Họ t&ecirc;n}}", TempElement =  "<input id=\"txtCQCQ\" name=\"txtCQCQ\"  />" },
               new Tempalte { TempFormat="{{Text#HoTenN#Họ t&ecirc;n người thay}}", TempElement =  "<input id=\"txtCQCQ\" name=\"txtCQCQ\"  />" },
               new Tempalte { TempFormat="{{Text#TenCQTC#Tên cơ quan chủ quan}}", TempElement =  "<input id=\"txtCQCQ\" name=\"txtCQCQ\"  />" },

            };
            return items;
        }
    }
    public class ListPageRecords
    {
        public int key { get; set; }
        public int page { get; set; }
    }
    public class Tempalte
    {
        public string TempFormat { get; set; }
        public string TempElement { get; set; }
    }
}