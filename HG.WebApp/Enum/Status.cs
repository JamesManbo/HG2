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

    public enum StatusXuLyHoSo
    {
        HoSoChoXuLy = 18,
        HoSoDangXuLy = 19,
        HoSoPhoiHop = 20,
        HoSoChuaBoSung = 21,
        HoSoDaChuyenXuLy = 22,
        HoSoChoKy = 23,
        HoSoDaKy = 24,
        HoSoChuyenMotCua = 25,
        HoSoDaPhoiHop = 26,
        HoSoGanQuaHan = 27,
        HoSoTheoDoiDonDoc = 28,
        HoSoChoChuyenMotCua = 29,
        HoSoXuLyThay = 30
    }

    public enum StatusChuyenXuLyHS
    {
        DangXuLy = 1,
        DaChuyenXuLy = 2,
        KhongDat = 3,
        ChoBoSung = 4,
        TraLai = 5,
        TraKetQua = 6,
        DaKy = 7,
        KetThucXuLy = 8,
        ThongBaoChoCD = 9,
        Chuyen1Cua = 10,
        GiaHan = 11

    }
}
