using SMData;
namespace UnitTest;
public class TestsOption
{
    [SetUp]
    public void Setup()
    {
        var test = true 
            ? Option.Some(1)
            : Option.None;
    }

    [Test]
    public void None_should_equal_Option_none()
    {
        Assert.Multiple(() =>
        {
            Assert.That(Option.None, Is.EqualTo(new Option<int>()));
            Assert.That(Option.None, Is.EqualTo(new Option<string>()));
            Assert.That(Option.None, Is.EqualTo(new Option<Option<string>>()));
            Assert.That(Option.None, Is.Not.EqualTo(Option.Some(1)));
            Assert.That(new Option<int>(2), Is.Not.EqualTo(Option.Some(1)));
            Assert.That(new Option<int>(1), Is.EqualTo(Option.Some(1)));
            
            Assert.That((IOption<int>)(Option<int>)Option.None, Is.EqualTo(new Option<int>()));
            Assert.That((IOption<int>)new Option<int>(1), Is.EqualTo(Option.Some(1)));
        });
    }
    [Test]
    public void TryGetValue_should_return_correclty()
    {
        if (Option.Some(1).TryGetValue(out var value))
            Assert.That(value, Is.EqualTo(1));
        else
            Assert.Fail();

        if (new Option<int>().TryGetValue(out value))
            Assert.Fail();
        else
            Assert.That(value, Is.EqualTo(default(int)));
    }
    [Test]
    public void GetValueOr_should_return_correclty()
    {
        Assert.Multiple(() =>
        {
            Assert.That(Option.Some(1).GetValueOrDefualt(-1), Is.EqualTo(1));
            Assert.That(new Option<int>().GetValueOrDefualt(-1), Is.EqualTo(-1));
        });
    }
    [Test]
    public void List_patterns_and_foreach_should_work()
    {
        int A = 0;
        foreach (var a in Option.Some(1))
        {
            A = a;
        }
        if (Option.Some(1) is [1] b)
        {
            A += b.Unwrap();
        }
        if (new Option<int>() is [] c)
        {
            A += 1;
        }
        Assert.That(A, Is.EqualTo(3));
    }
    [Test]
    public void Linq_interop()
    {
        Assert.That(from a in Option.Some(1)
                    from b in Option.Some(2)
                    from c in Option.Some(3)
                    select a + b + c, 
                    Is.EqualTo(Option.Some(6)));
    }
}