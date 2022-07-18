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
        HoSoChoTraKQ = 1,
        HoSoChuaDenNhanKQ = 2,
        HoSoChoTraKS = 3,
        HoSoDaTraKQ = 4,
        HoSoNhanKQGD1 = 5,
        HoSoNhanKQQuaBC = 6,
        HoSoChuyenPhatTC = 7,
    }
}
