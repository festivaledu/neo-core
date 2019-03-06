namespace Neo.Core.Communication.Packages
{
    public class RegisterPackageContent
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
    }
}
