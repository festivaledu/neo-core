using System;

namespace Neo.Core.Config
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class EditablePropertyAttribute : Attribute
    {
        public string Title { get; }

        public EditablePropertyAttribute(string title) {
            this.Title = title;
        }
    }
}
