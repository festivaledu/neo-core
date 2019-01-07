// ReSharper disable InconsistentNaming

namespace Neo.Core.Config
{
    public class ConfigValues
    {
        public int RSAKeySize { get; set; } = 4096;
        public string ServerAddress { get; set; } = "0.0.0.0";
        public int ServerPort { get; set; } = 42420;
    }
}
