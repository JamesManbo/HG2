﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HG.WebApp
{
    public enum StatusAction
    {
        None,
        Add,
        Edit,
        View,
        Role,
        SaveAndTranfer,
        TraKQ,
        InPhieuHen,
        InPhieuChuyen,
        ChuyenXuLy,
        Send
    }

    public enum ActionSave
    {
        Add,
        AddNew
    }

    public enum ActionThuTuc
    {
        ThuTuc,
        ThanhPhan,
        VanBanLQ,
        NhomTP,
        TPKQXL
    }
    public enum TableCheck
    {
        LuongXuLy         
    }
}
