using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;

namespace PortraitMaker2.Vms;
public abstract class PortraitMoveableObject : PortraitObjectVm
{
    protected PortraitMoveableObject(string path, Guid id) : base(path, id)
    {
    }

    [Reactive]
    public double X
    {
        get; set;
    }

    [Reactive]
    public double Y
    {
        get; set;
    }

    [Reactive]
    public double Size
    {
        get; set;
    }
}
