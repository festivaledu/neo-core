namespace Neo.Core.Communication
{
    public class Package
    {
        public dynamic Content { get; set; }
        public PackageType Type { get; set; }

        public Package() { }

        public Package(PackageType type, dynamic content) {
            this.Type = type;
            this.Content = content;
        }
    }

    public enum PackageType
    {
        Debug,
        Aes,
    }
}
