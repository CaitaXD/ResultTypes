using SMData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UnitTest;
public class EitherTaskUnitTest
{
    [Test]
    public async Task ConstructorTest()
    {
        var result = await Either.OkAsync<string, int>(1);
        Assert.That(result.GetValueOrDefualt()!, Is.EqualTo(1));
    }

    [Test]
    public void Linq_interop()
    {
        Assert.That((from a in Either.OkAsync<string, int>(1)
                    from b in Either.OkAsync<string, int>(2)
                    from c in Either.OkAsync<string, int>(3)
                    select a + b + c).Result,
                    Is.EqualTo(Either.OkAsync<string, int>(6).Result));
    }
}
