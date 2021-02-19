namespace ImportBL.Interfaces
{
    /// <summary>
    /// Saves library configs
    /// </summary>
    public interface IConfiguration
    {
        public string Url { get; set; }
        public string AppId { get; set; }
        public string ContactSchemaId { get; set; }
        public string SubjectSchemaId { get; set; }
        public string GiftSchemaId { get; set; }
        public string Token { get; set; }
        public string LogFilePath { get; set; }
    }
}
