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
       HoSoChoXL = 6,
       HoSoChuyenDaXL = 7,
       HoSoLienThong = 8,
       HoSoChoTiepNhanGD2 = 9,
    }
    public enum StatusTraKetQua
    {
        HoSoChoTraKQ = 10,
        HoSoChuaDenNhanKQ = 11,
        HoSoChoTraKS = 12,
        HoSoDaTraKQ = 13,
        HoSoNhanKQGD1 = 14,
        HoSoNhanKQQuaBC = 15,
        HoSoChuyenPhatTC = 16,
    }
}
