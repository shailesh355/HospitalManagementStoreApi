namespace HospitalManagementStoreApi.Models.AppClass.BLayer
{
    public class BlCommon
    {
    }
    public class BlDocumentImagesModel
    {
        public BlDocumentImagesModel()
        {
            this.status = false;
        }
        public DocumentImageGroup documentImageGroup { get; set; }
        public int dptTableId { get; set; }
        public string? physcialPath { get; set; }
        public Int64 maxFileSizeAllowed { get; set; }
        public string? fileTypeAllowed { get; set; }
        public string[]? fileType { get; set; }
        public Int32 fileCount { get; set; }
        public bool status { get; set; }
        public bool addYear { get; set; }
        public bool createFolder { get; set; }
    }
    /// <summary>
    /// Refrenced as dpt_table_id in Tables
    /// </summary>
    public enum DocumentType
    {
        ProfilePic = 1,
        ProfileLogo = 2,
        ProfileDocument = 3,
        NACH = 4,
        License = 5,
        OtherDocument = 6,
        SDD = 7,
        DoctorProfilePic = 8,
        HospitalImages = 9,
        HospitalPAN = 10,
        DoctorHospitalImages = 11,
        DoctorWorkArea = 12,
        WebsiteBanner = 13,
        MobileBanner = 14,
    }
    public enum DocumentImageGroup
    {
        Hospital = 1,
        Doctor = 2,
        Website = 3,
        Mobile = 4,
    }
    public enum HostingEnvironment
    {
        Windows = 0,
        Linux = 1,
        ObjectStorage = 2,
    }
    public enum StorageType
    {
        ObjectStorage = 1,
        FileSystem = 2
    }
}
