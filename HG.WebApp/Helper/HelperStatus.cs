namespace HG.WebApp.Helper
{
    public class HelperStatus
    {
        public static List<ObjectStatus> GetListStatus()
        {
            List<ObjectStatus> lstStatus = new List<ObjectStatus>();
            lstStatus.Add(new ObjectStatus() { value = 1, name = "Hồ sơ đang tiếp nhận" });
            lstStatus.Add(new ObjectStatus() { value = 2, name = "Hồ sơ chuyển chưa xử lý" });
            lstStatus.Add(new ObjectStatus() { value = 3, name = "Hồ sơ đang xử lý" });
            lstStatus.Add(new ObjectStatus() { value = 4, name = "Hồ sơ bị thu hồi" });
            lstStatus.Add(new ObjectStatus() { value = 5, name = "Hồ sơ trực tuyến" });
            lstStatus.Add(new ObjectStatus() { value = 6, name = "Hồ sơ chờ bổ sung" });
            lstStatus.Add(new ObjectStatus() { value = 7, name = "Hồ sơ chờ xử lý" });
            lstStatus.Add(new ObjectStatus() { value = 8, name = "Hồ sơ chuyển đã xử lý" });
            lstStatus.Add(new ObjectStatus() { value = 9, name = "Hồ sơ liên thông" });
            lstStatus.Add(new ObjectStatus() { value = 10, name = "Hồ sơ tiếp nhận GĐ2" });
            lstStatus.Add(new ObjectStatus() { value = 11, name = "Hồ sơ tiếp nhận chưa chính thức" });
            lstStatus.Add(new ObjectStatus() { value = 12, name = "Hồ sơ đã trả kết quả" });
            lstStatus.Add(new ObjectStatus() { value = 13, name = "Hồ sơ đang chưa đến nhận kết quả" });
            lstStatus.Add(new ObjectStatus() { value = 14, name = "Hồ sơ chờ trả kết quả" });
            lstStatus.Add(new ObjectStatus() { value = 15, name = "Hồ sơ nhận kết quả GD1" });
            lstStatus.Add(new ObjectStatus() { value = 16, name = "Hồ sơ nhận kết quả qua báo cáo" });
            lstStatus.Add(new ObjectStatus() { value = 17, name = "Hồ sơ chuyển phát TC" });
            lstStatus.Add(new ObjectStatus() { value = 18, name = "Hồ sơ chờ xử lý" });
            lstStatus.Add(new ObjectStatus() { value = 19, name = "Hồ sơ đang xử lý" });
            lstStatus.Add(new ObjectStatus() { value = 20, name = "Hồ sơ phối hợp" });
            lstStatus.Add(new ObjectStatus() { value = 21, name = "Hồ sơ chưa bổ sung" });
            lstStatus.Add(new ObjectStatus() { value = 22, name = "Hồ sơ phối đã chuyển xử lý" });
            lstStatus.Add(new ObjectStatus() { value = 23, name = "Hồ sơ chờ ký" });
            lstStatus.Add(new ObjectStatus() { value = 24, name = "Hồ sơ đã ký" });
            lstStatus.Add(new ObjectStatus() { value = 25, name = "Hồ sơ chuyển một cửa" });
            lstStatus.Add(new ObjectStatus() { value = 26, name = "Hồ sơ đã phối hợp" });
            lstStatus.Add(new ObjectStatus() { value = 27, name = "Hồ sơ gần quá hạn" });
            lstStatus.Add(new ObjectStatus() { value = 28, name = "Hồ sơ theo dõi đôn đốc" });
            lstStatus.Add(new ObjectStatus() { value = 29, name = "Hồ sơ chuyển một cửa" });
            lstStatus.Add(new ObjectStatus() { value = 30, name = "Hồ sơ xử lý thay" });
            lstStatus.Add(new ObjectStatus() { value = 31, name = "Hồ sơ liên thông" });
            return lstStatus;
        }
    }
    public class ObjectStatus
    {
        public int value { get; set; }
        public string name { get; set; }
    }
}
