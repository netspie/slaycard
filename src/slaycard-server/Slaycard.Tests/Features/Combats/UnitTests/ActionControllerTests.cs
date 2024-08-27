using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Tests.Features.Combats.UnitTests;

public class ActionControllerTests
{
    private void DoSomething() { }
    private void DoSomethingElse() { }
    private void DoSomethingElseElse() { }

    [Test]
    public void CanDoAction_IsTrue_IfConstructedWithIt()
    {
        var controller = new ActionController<PlayerId>(DoSomething) as IActionController<PlayerId>;
        Assert.IsTrue(controller.CanMakeAction(DoSomething));
    }

    [Test]
    public void CanDoAction_IsFalse_IfNotConstructedWithIt()
    {
        var controller = new ActionController<PlayerId>(DoSomethingElse) as IActionController<PlayerId>;
        Assert.IsFalse(controller.CanMakeAction(DoSomething));
    }


    [Test]
    public void SetActionExpectedNext_IsSuccess_IfSetPreviousActionDone()
    {
        var controller = new ActionController<PlayerId>(DoSomething);
        Assert.IsTrue(controller
            .SetActionDone(nameof(DoSomething))
            .SetActionExpectedNext(DoSomethingElse)
            .IsSuccess
        );
    }

    [Test]
    public void SetActionDone_IsNotSuccess_IfNotUserAction_ButUserProvided()
    {
        var controller = new ActionController<PlayerId>(DoSomething);
        Assert.IsFalse(controller.SetActionDone(nameof(DoSomething), new PlayerId("")).IsSuccess);
    }

    [Test]
    public void SetActionDone_IsNotSuccess_IfUserAction_ButUserNotProvided()
    {
        var controller = new ActionController<PlayerId>();
        Assert.IsTrue(controller
            .SetActionExpectedNext(nameof(DoSomething))
            .By([new PlayerId("id-1")])
            .IsSuccess);

        Assert.IsFalse(controller.SetActionDone(nameof(DoSomething)).IsSuccess);
    }

    [Test]
    public void SetActionDone_IsNotSuccess_IfUserAction_AndNonExpectedUserIdProvided()
    {
        var controller = new ActionController<PlayerId>() as ActionController<PlayerId>;
        Assert.IsTrue(controller
            .SetActionExpectedNext(nameof(DoSomething))
            .By([new PlayerId("id-1")])
            .IsSuccess);

        Assert.IsFalse(controller.SetActionDone(nameof(DoSomething), new PlayerId("id-2")).IsSuccess);
    }

    [Test]
    public void SetActionDone_IsSuccess_IfUserAction_AndExpectedUserIdProvided()
    {
        var controller = new ActionController<object>();
        Assert.IsTrue(controller
            .SetActionExpectedNext(nameof(DoSomething))
            .By([new PlayerId("id-1")])
            .IsSuccess);

        Assert.IsTrue(controller.SetActionDone(nameof(DoSomething), new PlayerId("id-1")).IsSuccess);
    }

    [Test]
    public void SetActionDone_IsNotSuccess_IfUserAction_AndExpectedUserIdProvidedNotInOrder()
    {
        var controller = new ActionController<object>();
        Assert.IsTrue(controller
            .SetActionExpectedNext(nameof(DoSomething))
            .By([new PlayerId("id-2"), new PlayerId("id-1")], mustObeyOrder: true)
            .IsSuccess);

        Assert.IsFalse(controller.SetActionDone(nameof(DoSomething), new PlayerId("id-1")).IsSuccess);
    }

    [Test]
    public void SetActionExpectedNext_DoesNotChangeAction_IfCalledMultipleTimes()
    {
        var controller = new ActionController<PlayerId>();
        Assert.IsTrue(controller
            .SetActionExpectedNext(nameof(DoSomething))
            .SetActionExpectedNext(DoSomethingElse)
            .IsSuccess);

        Assert.IsNull(controller.ActionInfo.GetAction(nameof(DoSomethingElse)));
    }

    [Test]
    public void SetActionExpectedNextOr_IsSuccess_AndHaveActions_IfCalledMultipleTimes()
    {
        var controller = new ActionController<object>() as ActionController<object>;
        Assert.IsTrue(controller
            .SetActionExpectedNext(nameof(DoSomething))
            .Or(DoSomethingElse)
            .IsSuccess);

        Assert.IsTrue(controller.ActionInfo.HasAction(nameof(DoSomething)));
        Assert.IsTrue(controller.ActionInfo.HasAction(nameof(DoSomethingElse)));
    }

    [Test]
    public void SetActionExpectedNext_DoesNotChangeAction_IfOnlyMultipleRepeatActionDone()
    {
        var controller = new ActionController<object>();

        Assert.IsTrue(controller
            .SetActionExpectedNext(nameof(DoSomething), ActionRepeat.Multiple)
            .Or(DoSomethingElse)
            .IsSuccess);

        Assert.IsFalse(controller.ActionInfo.GetAction(nameof(DoSomething))?.DoneBefore);
        Assert.IsTrue(controller.SetActionDone(nameof(DoSomething)).IsSuccess);
        Assert.IsTrue(controller.ActionInfo.GetAction(nameof(DoSomething))?.DoneBefore);

        Assert.IsTrue(controller.SetActionExpectedNext(nameof(DoSomethingElseElse)).IsSuccess);
        Assert.IsNull(controller.ActionInfo.GetAction(nameof(DoSomethingElseElse)));
    }

    [Test]
    public void CanMakeAction_IsTrue_After_SetActionDone_MultipleTimes_IsOfMultipleRepeat()
    {
        var controller = new ActionController<PlayerId>();

        var user1 = new PlayerId("id-1");
        var user2 = new PlayerId("id-2");
        Assert.IsTrue(controller
            .SetActionExpectedNext(nameof(DoSomething), ActionRepeat.Multiple)
            .By([user2, user1], mustObeyOrder: true)
            .IsSuccess);

        Assert.IsFalse(controller.CanMakeAction(nameof(DoSomething), user1));
        Assert.IsTrue(controller.CanMakeAction(nameof(DoSomething), user2));

        Assert.IsTrue(controller.SetActionDone(nameof(DoSomething), user2).IsSuccess);
        Assert.IsTrue(controller.CanMakeAction(nameof(DoSomething), user2));
        Assert.IsFalse(controller.CanMakeAction(nameof(DoSomething), user1));
    }

    [Test]
    public void SetByUserIds_IsNotSuccess_IfNotSetActionExpectedBefore()
    {
        var controller = new ActionController<PlayerId>();
        Assert.IsFalse(controller
            .By([new PlayerId("id-1")])
            .IsSuccess);
    }

    [Test]
    public void SetByUserIds_SetsActions_AsUserAction()
    {
        var controller = new ActionController<PlayerId>();
        Assert.IsTrue(controller
            .SetActionExpectedNext(nameof(DoSomething))
            .By([new PlayerId("id-1")])
            .IsSuccess);

        Assert.IsTrue(controller.IsUserAction());
    }
}
