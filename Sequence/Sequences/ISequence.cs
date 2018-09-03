using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence
{
    public interface ISequence
    {
        object NextValue();
        object Current();
    }
}
