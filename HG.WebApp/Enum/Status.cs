using System;
using System.Collections.Generic;
using System.Text;

namespace HG.WebApp
{
    public enum Status
    {
        InActive,
        Active
    }
    public enum StatusTiepNhanHoSo
    {
       HoSoDangTiepNhan = 1,
       HoSoChuyenChuaXL = 2,
       HoSoDangXL = 3,
       HoSoBiThuHoi = 4,
       HoSoTrucTuyen = 5,
       HoSoChoBoSung = 6,
       HoSoChoXL = 7,
       HoSoChuyenDaXL = 8,
       HoSoLienThong = 9,
       HoSoChoTiepNhanGD2 = 10,
       HoSoChoTiepNhanChuaChinhThuc = 11,

    }
    public enum StatusTraKetQua
    {
        HoSoDaTraKQ = 12,
        HoSoChuaDenNhanKQ = 13,
        HoSoChoTraKS = 14,
        HoSoNhanKQGD1 = 15,
        HoSoNhanKQQuaBC = 16,
        HoSoChuyenPhatTC = 17,
    }
}
