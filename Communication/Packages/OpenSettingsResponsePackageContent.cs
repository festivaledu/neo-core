using System.Collections.Generic;

namespace Neo.Core.Communication.Packages
{
    public class OpenSettingsResponsePackageContent
    {
        public dynamic Model { get; set; }
        public Dictionary<string, string> Titles { get; set; } = new Dictionary<string, string>();

        public OpenSettingsResponsePackageContent(dynamic model) {
            this.Model = model;
        }
    }
}
