namespace Neo.Core.Config
{
    public class ConfigValues
    {
        public string Locale { get; set; } = "de";
        // ReSharper disable once InconsistentNaming
        public int RSAKeySize { get; set; } = 4096;
        public string ServerAddress { get; set; } = "0.0.0.0";
        public int ServerPort { get; set; } = 42420;
    }
}
