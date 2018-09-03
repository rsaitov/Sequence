using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.DbSet
{
    interface IDbSet
    {
        void CreateIfNotExists();
        void SetSequenceIfNotExists();
        int NextValue();
    }
}
