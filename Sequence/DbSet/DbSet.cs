
namespace Sequence.DbSet
{
    interface IDbSet
    {
        void CreateIfNotExists();
        void SetSequenceIfNotExists();
        int NextValue();
    }
}
