using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palladium.Security
{
    public interface IAccessControlList : System.Collections.IEnumerable
    {
        bool AllowInherit { get; set; }
    }

    public static class AccessControlListExtensions
    {
        public static bool ContainsRightType<T>(this IAccessControlList acl, T rightType) where T : struct, IConvertible
        {
            string type = rightType.GetRightType();

            bool found = false;

            foreach( IAccessControlEntry ace in acl )
                if( ace.GetRightType().Equals( type ) )
                {
                    found = true;
                    break;
                }

            return found;
        }
    }

    public class DiscretionaryAccessControlList : List<IAccessControlEntry>, IAccessControlList
    {
        public bool AllowInherit { get; set; }
    }

    public class SystemAccessControlList : List<IAccessControlEntryAudit>, IAccessControlList
    {
        public bool AllowInherit { get; set; }

        public AuditType AuditTypeFilter { get; set; } =
            AuditType.SuccessAudit | AuditType.FailureAudit | AuditType.Information | AuditType.Warning | AuditType.Error;
    }
}