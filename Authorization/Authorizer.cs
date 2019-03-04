using System.Collections.Generic;
using System.Linq;
using Neo.Core.Shared;

namespace Neo.Core.Authorization
{
    public static class Authorizer
    {
        /// <summary>
        ///     Combines two sets of permissions.
        /// </summary>
        /// <param name="lowerContext">The permission set with lower valence.</param>
        /// <param name="higherContext">The permission set with higher valence.</param>
        /// <returns>Returns a new set of permissions created from unioning two sets.</returns>
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

        /// <summary>
        ///     Checks whether the permission sets grant a given permission.
        /// </summary>
        /// <param name="permission">The permission to validate.</param>
        /// <param name="permissionSets">The permission sets to check.</param>
        /// <returns>Returns <c>true</c> when the sets grant the given permission.</returns>
        public static bool IsAuthorized(string permission, params Dictionary<string, Permission>[] permissionSets) {
            var permissions = new Dictionary<string, Permission>();

            // Create the final set of permissions by unioning all sets together
            permissions = permissionSets.Aggregate(permissions, UnionPermissions);

            /*
             * Assemble all valid permissions by splitting the searched one and putting it back together one part a time
             * For example:
             *
             * permission               validPermissions
             * neo.server.edit.name     *
             *                          neo.*
             *                          neo.server.*
             *                          neo.server.edit.*
             *                          neo.server.edit.name
             */
            var permissionLayers = permission.Split('.');
            var validPermissions = new List<string> {
                "*"
            };

            var valid = "";
            for (var l = 0; l < permissionLayers.Length - 1; l++) {
                var layer = permissionLayers[l];
                valid += $"{layer}.";
                validPermissions.Add($"{valid}*");
            }
            validPermissions.Add(permission);

            // Define the default permission if none of the sets contained any of the valid permissions
            var p = Permission.Inherit;

            // Loop through the valid permissions and overwrite p everytime a new value other than Permission.Inherit was found.
            // This needs to be done, because explicit permissions are stronger than implicit ones. So neo.server.edit.name counts higher than neo.server.edit.*.
            foreach (var validPermission in validPermissions) {
                if (!permissions.ContainsKey(validPermission) || permissions[validPermission] == Permission.Inherit) {
                    continue;
                }

                p = permissions[validPermission];
            }

            return p == Permission.Allow;
        }

        /// <summary>
        ///     Checks whether this <see cref="IAuthorizable"/> is granted the given permission.
        /// </summary>
        /// <param name="authorizable">The <see cref="IAuthorizable"/> to authorize.</param>
        /// <param name="permission">The permission to check.</param>
        /// <returns>Returns <c>true</c> when the sets grant the given permission.</returns>
        public static bool IsAuthorized(this IAuthorizable authorizable, string permission) {
            var permissionsSets = new List<Dictionary<string, Permission>>();

            switch (authorizable) {
            case Guest guest:
                permissionsSets.Add(Pool.Server.Groups[0].Permissions);
                if (guest.ActiveChannel != null) {
                    permissionsSets.Add(guest.ActiveChannel.MemberPermissions[guest.InternalId]);
                }
                permissionsSets.Add(guest.Permissions);
                break;
            case Member member:
                foreach (var group in member.Groups) {
                    permissionsSets.Add(group.Permissions);
                }
                if (member.ActiveChannel != null) {
                    permissionsSets.Add(member.ActiveChannel.MemberPermissions[member.InternalId]);
                }
                permissionsSets.Add(member.Permissions);
                break;
            case Group group:
                permissionsSets.Add(group.Permissions);
                break;
            default:
                return false;
            }

            return IsAuthorized(permission, permissionsSets.ToArray());
        }
    }
}
