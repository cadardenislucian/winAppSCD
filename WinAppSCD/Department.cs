namespace WinAppSCD
{
    internal class Department
    {
        public int departmentID { get; set; }
        public string description { get; set; }
        public int? parentID { get; set; }
        public int? managerID { get; set; }
    }
}
