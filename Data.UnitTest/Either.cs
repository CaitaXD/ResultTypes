using SMData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest;
internal class TestEither
{
    [SetUp]
    public void Setup()
    {
        var test = true
            ? Either.Ok<string,int>(1)
            : Either.Err<string,int>("Error");
    }

    [Test]
    public void None_should_equal_Either_none()
    {
        Assert.Multiple(() =>
        {
            Assert.That((IEither<string, int>)Either.Err<string, int>("A"), Is.EqualTo(new Either<string, int>("A")));
            Assert.That((IEither<string, int>)new Either<string, int>(1), Is.EqualTo(Either.Ok<string, int>(1)));
        });
    }
    [Test]
    public void TryGetValue_should_return_correclty()
    {
        if (Either.Ok<string,int>(1).TryGetValue(out var value, out var err))
            Assert.That(value, Is.EqualTo(1));
        else
            Assert.Fail();

        if (new Either<string,int>("Error").TryGetValue(out value, out err))
            Assert.Fail();
        else
            Assert.That(err, Is.EqualTo("Error"));
    }
    [Test]
    public void GetValueOr_should_return_correclty()
    {
        Assert.Multiple(() =>
        {
            Assert.That(Either.Ok<string,int>(1).GetValueOrDefualt(-1), Is.EqualTo(1));
            Assert.That(new Either<string,int>().GetValueOrDefualt(-1), Is.EqualTo(-1));
        });
    }
    [Test]
    public void List_patterns_and_foreach_should_work()
    {
        int A = 0;
        foreach (var a in Either.Ok<string,int>(1))
        {
            A = a;
        }
        if (Either.Ok<string,int>(1) is [1] b)
        {
            A += b.Unwrap();
        }
        if (new Either<string,int>() is [] c)
        {
            A += 1;
        }
        Assert.That(A, Is.EqualTo(3));
    }
    [Test]
    public void Linq_interop()
    {
        Assert.That(from a in Either.Ok<string,int>(1)
                    from b in Either.Ok<string,int>(2)
                    from c in Either.Ok<string,int>(3)
                    select a + b + c,
                    Is.EqualTo(Either.Ok<string,int>(6)));
    }
}