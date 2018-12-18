using System;
using System.Linq;
using System.Collections.Generic;

namespace Suplex.Security.Principal
{
    public static class GroupUtilities
    {
        public static Dictionary<Guid, Group> ToUIdDictionary(this IEnumerable<Group> groups)
        {
            return groups.ToDictionary( group => group.UId, group => group );
            //Dictionary<Guid, Group> d = new Dictionary<Guid, Group>();
            //foreach( Group g in groups )
            //    d[g.UId] = g;

            //return d;
        }

        public static Dictionary<string, Group> ToNameDictionary(this IEnumerable<Group> groups, bool caseSensitive = false)
        {
            return groups.ToDictionary( group => group.Name, group => group, caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase );
            //Dictionary<string, Group> d = new Dictionary<string, Group>( caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase );
            //foreach( Group g in groups )
            //    d[g.Name] = g;

            //return d;
        }

        public static void ToDictionaries(this IEnumerable<Group> groups, out Dictionary<Guid, Group> uidDict, out Dictionary<string, Group> nameDict, bool caseSensitive = false)
        {
            uidDict = new Dictionary<Guid, Group>();
            nameDict =  new Dictionary<string, Group>( caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase );
            foreach( Group g in groups )
            {
                uidDict[g.UId] = g;
                nameDict[g.Name] = g;
            }
        }
    }
}