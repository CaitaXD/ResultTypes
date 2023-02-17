using SMData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UnitTest;
public class OptionTaskTest
{
    
    [Test]
    public void Linq_interop()
    {
        Assert.That((from a in Option.SomeAsync(1)
                     from b in Option.SomeAsync(2)
                     from c in Option.SomeAsync(3)
                     select a + b + c).Result,
                    Is.EqualTo(Option.SomeAsync(6).Result));
    }
}
