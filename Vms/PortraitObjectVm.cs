using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace PortraitMaker2.Vms;
public abstract class PortraitObjectVm: ReactiveObject
{
    public string Path
    {
        get;
    }

    public Guid Id
    {
        get;
    } 

    protected PortraitObjectVm(string path, Guid id)
    {
        Path = path;
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
    }
}
