namespace Suplex.Security.DataAccess
{
    public interface ISuplexDalHost
    {
        ISuplexDal Dal { get; }
        void Configure(object config);
    }
}