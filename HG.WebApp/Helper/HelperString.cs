﻿using System;
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

        public static string CheckMaKH(string makh)
        {
            if(makh.Length == 1)
            {
                return "0000000" + makh;
            }else if(makh.Length == 2)
            {
                return "000000" + makh;
            }
            else if (makh.Length == 3)
            {
                return "00000" + makh;
            }
            else if (makh.Length == 4)
            {
                return "0000" + makh;
            }
            else if (makh.Length == 5)
            {
                return "000" + makh;
            }
            else if (makh.Length == 6)
            {
                return "00" + makh;
            }
            else if (makh.Length == 7)
            {
                return "0" + makh;
            }
            return makh;
        }


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
        public static string RandomCodeVerify()
        {
            Random generator = new Random();
            int r = generator.Next(100000, 999999);
            return r.ToString();
        }
        public static string SerializeObject(object obj)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                serializer.Serialize(ms, obj);
                ms.Position = 0;
                xmlDoc.Load(ms);
                xmlDoc.RemoveChild(xmlDoc.FirstChild);
                return xmlDoc.InnerXml;
            }
        }

        public static string CreateCode(string code)
        {
            return String.Concat(RemoveSign4VietnameseString(code).ToUpper().Where(c => !Char.IsWhiteSpace(c)));
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
        public static List<Gender> ListGenders()
        {
            List<Gender> items = new List<Gender>
            {
               new Gender { key=1, name = "Nam" },
               new Gender { key=2, name = "Nữ" },
               new Gender { key=3, name = "Khác" },
              
            };
            return items;
        }

        public static List<ListCommonRecords> ListNguyenVongRecords()
        {
            List<ListCommonRecords> items = new List<ListCommonRecords>
            {
               new ListCommonRecords { key=1, noi_dung = "Nhóm trẻ 03 - 12 tháng tuổi" },
               new ListCommonRecords { key=2, noi_dung = "Nhóm trẻ 13 - 24 tháng tuổi" },
               new ListCommonRecords { key=3, noi_dung = "Nhóm trẻ 25 - 36 tháng tuổi" },
               new ListCommonRecords { key=4, noi_dung = "Lớp 3 tuổi" },
               new ListCommonRecords { key=5, noi_dung = "Lớp 4 tuổi" },
               new ListCommonRecords { key=6, noi_dung = "Lớp 5 tuổi" }
            };
            return items;
        }
        public static List<ListStrCommonRecords> ListNguyenVong2_THPTRecords()
        {
            List<ListStrCommonRecords> items = new List<ListStrCommonRecords>
            {
               new ListStrCommonRecords { ma="01", noi_dung = "PTDT Nội trú THCS&THPT Bắc Mê, Huyện Bắc Mê" },
               new ListStrCommonRecords { ma="02", noi_dung = "PTDT Nội trú THCS&THPT Hoàng Su Phì, Huyện Hoàng Su Phì" },
               new ListStrCommonRecords { ma="03", noi_dung = "PTDT Nội trú THCS&THPT Bắc Quang, Huyện Bắc Quang" },
               new ListStrCommonRecords { ma="04", noi_dung = "PTDT Nội trú  THCS&THPT Yên Minh, Thị trấn Yên Minh, Huyện Yên Minh" },
               new ListStrCommonRecords { ma="05", noi_dung = "PTDT Nội trú THCS&THPT Xín Mần, Huyện Xín Mần" },
               new ListStrCommonRecords { ma="06", noi_dung = "PTDT Nội trú THCS&THPT Đồng Văn, Huyện Đồng Văn" },
               new ListStrCommonRecords { ma="07", noi_dung = "PTDTNT THPT tỉnh Hà Giang, Thành phố Hà Giang" },

            };
            return items;
        }
        public static List<ListCommonRecords> ListTruongHocRecords()
        {
            List<ListCommonRecords> items = new List<ListCommonRecords>
            {
               new ListCommonRecords { key=1, noi_dung = "Nhóm trẻ Hoa Phượng Đỏ, Thành phố Hà Giang" },
               new ListCommonRecords { key=2, noi_dung = "Nhóm trẻ Mặt Trời Nhỏ, Thành phố Hà Giang" },
               new ListCommonRecords { key=3, noi_dung = "Nhóm trẻ Ánh Dương, Thành phố Hà Giang" },
               new ListCommonRecords { key=4, noi_dung = "Trường mầm non Hoa Sen, Thành phố Hà Giang" },
               new ListCommonRecords { key=5, noi_dung = "Trường mầm non Sao Mai, Thành phố Hà Giang" },
               new ListCommonRecords { key=6, noi_dung = "Trường mầm non Ngọc Đường, Thành phố Hà Giang" }
            };
            return items;
        }
        public static List<ListCommonRecords> ListGioiTinhRecords()
        {
            List<ListCommonRecords> items = new List<ListCommonRecords>
            {
               new ListCommonRecords { key=1, noi_dung = "Nam" },
               new ListCommonRecords { key=2, noi_dung = "Nữ" },
               new ListCommonRecords { key=3, noi_dung = "Khác" },
            };
            return items;
        }
        public static List<ListCommonRecords> ListTinhRecords()
        {
            List<ListCommonRecords> items = new List<ListCommonRecords>
            {
               new ListCommonRecords { key=1, noi_dung = "Hà Nội" },
               new ListCommonRecords { key=2, noi_dung = "Thái Bình" },
               new ListCommonRecords { key=3, noi_dung = "Nam Định" },
            };
            return items;
        }
        public static List<ListCommonRecords> ListHuyenRecords()
        {
            List<ListCommonRecords> items = new List<ListCommonRecords>
            {
               new ListCommonRecords { key=1, noi_dung = "Huyện Yên Minh" },
               new ListCommonRecords { key=2, noi_dung = "Huyện Quảng Bình" },
               new ListCommonRecords { key=3, noi_dung = "Huyện Đồng văn" },
            };
            return items;
        }
        public static List<ListCommonRecords> ListXaRecords()
        {
            List<ListCommonRecords> items = new List<ListCommonRecords>
            {
               new ListCommonRecords { key=1, noi_dung = "Xã Yên Thành" },
               new ListCommonRecords { key=2, noi_dung = "Xã Yên Hà" },
               new ListCommonRecords { key=3, noi_dung = "Xã Tân Nam" },
            };
            return items;
        }
        public static List<ListStrCommonRecords> ListKetQuaHocTapRecords()
        {
            List<ListStrCommonRecords> items = new List<ListStrCommonRecords>
            {
               new ListStrCommonRecords { ma="hoanthanh", noi_dung = "Hoàn Thành" },
               new ListStrCommonRecords { ma="hoanthanhtot", noi_dung = "Hoàn thành tốt" },
               new ListStrCommonRecords { ma="hoanthanhxuatsac", noi_dung = "Hoàn thành xuất sắc" },
            };
            return items;
        }
        public static List<ListStrCommonRecords> ListHanhKiemRecords()
        {
            List<ListStrCommonRecords> items = new List<ListStrCommonRecords>
            {
               new ListStrCommonRecords { ma="trungbinh", noi_dung = "Trung bình" },
               new ListStrCommonRecords { ma="kha", noi_dung = "Khá" },
               new ListStrCommonRecords { ma="tot", noi_dung = "Tốt" },
            };
            return items;
        }
        public static List<ListStrCommonRecords> ListUuTienRecords()
        {
            List<ListStrCommonRecords> items = new List<ListStrCommonRecords>
            {
               new ListStrCommonRecords { ma="uutien1", noi_dung = "Ưu tiên 1" },
               new ListStrCommonRecords { ma="uutien2", noi_dung = "Ưu tiên 2" },
               new ListStrCommonRecords { ma="uutien3", noi_dung = "Ưu tiên 3" },
            };
            return items;
        }
        public static List<ThanhPhanHoSoMoDel> ListThanhPhanHoSoRecords()
        {
            List<ThanhPhanHoSoMoDel> items = new List<ThanhPhanHoSoMoDel>
            {
                new ThanhPhanHoSoMoDel {
                   ten_thanh_phan="Đơn xin nhập học", file_dinh_kem = "" , 
                    bieu_mau = "" , ban_chinh = 1, ban_sao = 0, ban_photo = 0, stt = 1},
                new ThanhPhanHoSoMoDel {
                   ten_thanh_phan="Giấy khai sinh", file_dinh_kem = "" , 
                   bieu_mau = "" , ban_chinh = 0, ban_sao = 1, ban_photo = 0,stt = 2},
            };
            return items;
        }
        public static List<ThanhPhanHoSoMoDel> ListThanhPhanHoSoCapTieuHocRecords()
        {
            List<ThanhPhanHoSoMoDel> items = new List<ThanhPhanHoSoMoDel>
            {
                new ThanhPhanHoSoMoDel {
                   ten_thanh_phan="Đơn xin tuyển sinh vào lớp 1", file_dinh_kem = "" ,
                    bieu_mau = "" , ban_chinh = 1, ban_sao = 0, ban_photo = 0, stt = 1},
                new ThanhPhanHoSoMoDel {
                   ten_thanh_phan="Giấy khai sinh hợp lệ", file_dinh_kem = "" ,
                   bieu_mau = "" , ban_chinh = 0, ban_sao = 1, ban_photo = 0,stt = 2},
            };
            return items;
        }

        public static List<ThanhPhanHoSoMoDel> ListThanhPhanHoSoCapTHCSRecords()
        {
            List<ThanhPhanHoSoMoDel> items = new List<ThanhPhanHoSoMoDel>
            {
                new ThanhPhanHoSoMoDel {
                   ten_thanh_phan="Giấy khai sinh hợp lệ", file_dinh_kem = "" , is_bat_buoc = true,
                    bieu_mau = "" , ban_chinh = 0, ban_sao = 1, ban_photo = 0, stt = 1},
                new ThanhPhanHoSoMoDel {
                   ten_thanh_phan="Học bạ cấp tiểu học hoặc các hồ sơ khác có giá trị thay thế học bạ",
                    file_dinh_kem = "" , is_bat_buoc = true,
                   bieu_mau = "" , ban_chinh = 1, ban_sao = 0, ban_photo = 0,stt = 2},
                new ThanhPhanHoSoMoDel {
                   ten_thanh_phan="Sổ hộ khẩu", file_dinh_kem = "" , is_bat_buoc = false,
                   bieu_mau = "" , ban_chinh = 0, ban_sao = 1, ban_photo = 0,stt = 3},
                new ThanhPhanHoSoMoDel {
                   ten_thanh_phan="Giấy xác nhận chế độ ưu tiên, do cơ quan có thẩm quyền cấp (nếu có)", 
                   file_dinh_kem = "" , is_bat_buoc = false,
                   bieu_mau = "" , ban_chinh = 0, ban_sao = 1, ban_photo = 0,stt = 4},
            };
            return items;
        }
        public static List<ThanhPhanHoSoMoDel> ListThanhPhanHoSoCapTHPTRecords()
        {
            List<ThanhPhanHoSoMoDel> items = new List<ThanhPhanHoSoMoDel>
            {
                new ThanhPhanHoSoMoDel {
                   ten_thanh_phan="Giấy khai sinh hợp lệ", file_dinh_kem = "" , is_bat_buoc = true,
                    bieu_mau = "" , ban_chinh = 0, ban_sao = 1, ban_photo = 0, stt = 1},
                new ThanhPhanHoSoMoDel {
                   ten_thanh_phan="	Sổ hộ khẩu (bản sao hợp lệ) hoặc văn bản xác nhận thông tin " +
                   "về cư trú của cơ quan có thẩm quyền " +
                   "(Bắt buộc đối với tuyển sinh vào trường PTDT Nội trú)",
                    file_dinh_kem = "" , is_bat_buoc = false,
                   bieu_mau = "" , ban_chinh = 1, ban_sao = 1, ban_photo = 0,stt = 2},
                new ThanhPhanHoSoMoDel {
                   ten_thanh_phan="Học bạ cấp trung học cơ sở", file_dinh_kem = "" , is_bat_buoc = true,
                   bieu_mau = "" , ban_chinh = 1, ban_sao = 0, ban_photo = 0,stt = 3},
                new ThanhPhanHoSoMoDel {
                   ten_thanh_phan="Giấy xác nhận chế độ ưu tiên, do cơ quan có thẩm quyền cấp",
                   file_dinh_kem = "" , is_bat_buoc = true,
                   bieu_mau = "" , ban_chinh = 1, ban_sao = 0, ban_photo = 0,stt = 4}, 
                new ThanhPhanHoSoMoDel {
                   ten_thanh_phan="Giấy xác nhận do Ủy ban nhân dân xã, phường, thị trấn cấp (đối với người học đã tốt nghiệp trung học cơ sở từ những năm học trước) " +
                   "không trong thời gian thi hành án phạt tù; cải tạo không giam giữ hoặc vi phạm pháp luật	",
                   file_dinh_kem = "" , is_bat_buoc = false,
                   bieu_mau = "" , ban_chinh = 1, ban_sao = 0, ban_photo = 0,stt = 5}, 
                new ThanhPhanHoSoMoDel {
                   ten_thanh_phan="Bằng tốt nghiệp trung học cơ sở hoặc giấy chứng nhận tốt nghiệp trung học cơ sở tạm thời",
                   file_dinh_kem = "" , is_bat_buoc = true,
                   bieu_mau = "" , ban_chinh = 1, ban_sao = 0, ban_photo = 0,stt = 6},
            };
            return items;
        }
        public static string GetListSchool()
        {
            return "";
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

    public class Gender
    {
        public int key { get; set; }
        public string name { get; set; }
    }
    public class ListCommonRecords
    {
        public int key { get; set; }
        public string noi_dung { get; set; }
    }
    public class ListStrCommonRecords
    {
        public string ma { get; set; }
        public string noi_dung { get; set; }
    }
    public class Tempalte
    {
        public string TempFormat { get; set; }
        public string TempElement { get; set; }
    }
    public class ThanhPhanHoSoMoDel
    {
        public string? ten_thanh_phan { get; set; }
        public string? file_dinh_kem { get; set; }
        public string? bieu_mau { get; set; }
        public int? ban_chinh { get; set; }
        public int? ban_sao { get; set; }
        public int? ban_photo { get; set; }
        public int? id_ghs_tuyen_sinh_cap_mam_non { get; set; }
        public int? stt { get; set; }
        public bool? is_bat_buoc { get; set; }
    }
}