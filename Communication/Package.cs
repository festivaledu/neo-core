using Newtonsoft.Json;

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

        public TOut GetContentTypesafe<TOut>() {
            return JsonConvert.DeserializeObject<TOut>(JsonConvert.SerializeObject(Content));
        }
    }

    public enum PackageType
    {
        Debug,
        RSA,
        AES,
    }
}
