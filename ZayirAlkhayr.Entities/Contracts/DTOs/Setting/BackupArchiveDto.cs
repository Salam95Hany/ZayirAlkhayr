namespace ZayirAlkhayr.Entities.Contracts.DTOs.Setting
{
    public class BackupArchiveDto
    {
        public byte[] FileContents { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; } = "application/zip";
    }
}
