using Motive.Core.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ResourcePanelData<T>
{
    public ResourceActivationContext ActivationContext { get; set; }
    public T Resource { get; set; }

    public ResourcePanelData(ResourceActivationContext ac, T resource)
    {
        ActivationContext = ac;
        Resource = resource;
    }
}
