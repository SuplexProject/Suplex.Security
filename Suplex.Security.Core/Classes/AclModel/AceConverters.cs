using System.Collections.ObjectModel;

namespace Suplex.Security.AclModel
{
    public class AceConverters : ObservableCollection<IAccessControlEntryConverter>, IAceConverters
    { }
}