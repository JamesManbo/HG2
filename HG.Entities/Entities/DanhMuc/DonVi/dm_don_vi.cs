using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.DanhMuc.DonVi
{
    public class dm_don_vi : BaseEntities
    {
        public string ma_don_vi { get; set; }
        public string ten_don_vi { get; set; }
        public string? mo_ta { get; set; }
        public string ma_quoc_gia_don_vi { get; set; }
        public string ip_or_website { get; set; }
        public string ma_cap_co_quan { get; set; }
        public string? ma_dia_ban_cha { get; set; }
        public string? ma_dia_ban_con { get; set; }
        public string? ma_dia_ban_con_c1 { get; set; }
        public int la_dich_vu_cong { get; set; }
        public string? so_dien_thoai { get; set; }
        public string? email { get; set; }
        public string? fax { get; set; }
        public string? avatar { get; set; }
        public int? stt { get; set; }
        public int? la_kiem_duyet_kqhs { get; set; }
        public int? la_su_dung_phien_gd { get; set; }
        public string? token { get; set; }
        public string? lkvnpt_pay_baseurl { get; set; }
        public string? lkvnpt_pay_merchan_serviceid { get; set; }
        public string? lkvnpt_pay_api_key { get; set; }
        public string? lkvnpt_pay_secret_key { get; set; }
        public string? lkvnpt_qr_clientid_app { get; set; }
        public string? lkvnpt_qr_clientname_app { get; set; }
        public string? lkvnpt_qr_clientcode_app { get; set; }
        public string? lkvnpt_qr_terminalId_app_app { get; set; }
        public string? lkvnpt_qr_secret_key_app { get; set; }
        public string? lkvnpt_qr_base_url_app { get; set; }
        public string? lkvnpt_qr_Authorization { get; set; }
    }
}
