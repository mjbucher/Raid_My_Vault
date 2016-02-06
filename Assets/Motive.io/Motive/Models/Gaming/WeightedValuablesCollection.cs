using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class WeightedValuablesCollection
{
    public double Weight { get; set; }
    public ValuablesCollection ValuablesCollection { get; set; }

    public WeightedValuablesCollection()
    {
        Weight = 1;
    }
}
