using System;

namespace Neo.Core.Communication.Packages
{
    public class InputPackageContent
    {
        public string Input { get; set; }
        public Guid TargetChannel { get; set; }
    }
}
