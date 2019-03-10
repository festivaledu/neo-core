using System;

namespace Neo.Core.Config
{
    /// <summary>
    ///     Marks a property as editable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class EditablePropertyAttribute : Attribute
    {
        /// <summary>
        ///     The human-readable title of the property this <see cref="EditablePropertyAttribute"/> is attached to.
        /// </summary>
        public string Title { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EditablePropertyAttribute"/> class with the given title.
        /// </summary>
        /// <param name="title"></param>
        public EditablePropertyAttribute(string title) {
            this.Title = title;
        }
    }
}
