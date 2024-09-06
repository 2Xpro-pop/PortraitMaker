using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace PortraitMaker2.Vms;
public sealed class BodyVm : PortraitMoveableObject
{
    public BodyVm(string path, Guid id) : base(path, id)
    {
    }

}
