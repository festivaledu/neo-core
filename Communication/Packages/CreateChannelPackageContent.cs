using Neo.Core.Shared;

namespace Neo.Core.Communication.Packages
{
    public class CreateChannelPackageContent
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public int Limit { get; set; }
        public string Password { get; set; }
        public Lifespan Lifetime { get; set; }
    }
}
