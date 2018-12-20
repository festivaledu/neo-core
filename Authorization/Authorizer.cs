using System.Collections.Generic;
using System.Linq;

namespace Neo.Core.Authorization
{
    public static class Authorizer
    {
        public static Dictionary<string, Permission> UnionPermissions(Dictionary<string, Permission> lowerContext, Dictionary<string, Permission> higherContext) {
            // Take all values from the lower context as the basis
            var union = new Dictionary<string, Permission>(lowerContext);

            foreach (var key in higherContext.Keys) {

                /*
                 * The following if statement is used to overwrite subkeys with the value from a new master key
                 * For example:
                 *
                 * LowerContext     HigherContext       Union
                 * -neo.read        +neo.*              +neo.read
                 * ~neo.write                           +neo.write
                 * +neo.edit                            +neo.edit
                 */

                // If the current key from the higher context overwrites multiple values at once
                if (key.Contains('*') && higherContext[key] != Permission.Inherit) {

                    // Take all existing keys from the basis that are subkeys from the current key
                    foreach (var subkey in union.Keys.ToList().FindAll(k => k != key && k.StartsWith(key.Replace("*", "")))) {

                        // Replace the old value from the lower context with new one from the higher context
                        union[subkey] = higherContext[key];
                    }
                }

                if (!union.ContainsKey(key)) {

                    // If the union doesn't contain the key, add it
                    union.Add(key, higherContext[key]);

                } else if (higherContext[key] != Permission.Inherit) {

                    // If the higher context overwrites the old value, overwrite it
                    union[key] = higherContext[key];

                } else if (!key.Contains('*') && higherContext[key] == Permission.Inherit && union[key] != lowerContext[key]) {

                    /*
                     * If the higher context inherites the old value, inherit it
                     * This has to take the value from the lower context, because the value inside the base may have been overwrite already
                     * For example:
                     *
                     * The higher context might override all rights in neo but it want's to keep neo.write
                     * So when the neo.* is parsed, neo.write might be already overwriten in the union by it
                     * Therefore we have to take the unaltered value from the lower context on Permission.Inherit
                     */
                    union[key] = lowerContext[key];
                }
            }

            return union;
        }
    }
}
