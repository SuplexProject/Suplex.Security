using System.Collections.ObjectModel;

namespace Suplex.Security.AclModel
{
    public class AceConverters : ObservableCollection<IAccessControlEntryConverter>, IAceConverters
    {
        public override string ToString()
        {
            return $"DaclConverters: {Count}";
        }
    }
}