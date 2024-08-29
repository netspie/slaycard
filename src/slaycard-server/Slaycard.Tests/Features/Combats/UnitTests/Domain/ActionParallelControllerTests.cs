using Slaycard.Api.Core.Domain;
using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Tests.Features.Combats.UnitTests.Domain;

public class ActionParallelControllerTests
{
    [Test]
    public void Run()
    {
        var controller = new ActionParallelController<PlayerId>([new PlayerId("player-1")]);

        bool allGood = false;
        Assert.DoesNotThrow(() => controller.Run(new PlayerId("player-1"), () => allGood = true));

        Assert.IsTrue(allGood);
    }

    [Test]
    public void Run_WhenNotExistingId_Throws()
    {
        var controller = new ActionParallelController<PlayerId>([new PlayerId("player-1")]);

        bool allGood = false;
        Assert.Throws<DomainException>(() => controller.Run(new PlayerId("player-2"), () => allGood = true));

        Assert.IsFalse(allGood);
    }

    [Test]
    public void Run_DoCycle()
    {
        var id1 = new PlayerId("player-1");
        var id2 = new PlayerId("player-2");

        var controller = new ActionParallelController<PlayerId>([id1, id2]);

        Assert.DoesNotThrow(() => controller.Run(id1, () => { }));
        Assert.Throws<DomainException>(() => controller.Run(id1, () => { }));
        Assert.DoesNotThrow(() => controller.Run(id2, () => { }));
        Assert.Throws<DomainException>(() => controller.Run(id2, () => { }));
        Assert.Throws<DomainException>(() => controller.Run(id1, () => { }));
    }
}
