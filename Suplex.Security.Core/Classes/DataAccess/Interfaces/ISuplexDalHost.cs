namespace Suplex.Security.AclModel.DataAccess
{
    public interface ISuplexDalHost
    {
        IDataAccessLayer Dal { get; }
        void Configure(object config);
    }
}