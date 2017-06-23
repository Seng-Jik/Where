using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Where.Renderer.Renderer3D.AfterEffect
{
    interface IAfterEffect
    {
        void SetDrawState();
        void ResetDrawState();
        int DownSample {get;}
        int Live { get; }
    }
}
