using Core.Collections;

namespace Slaycard.Api.Features.Combats.Domain;

public class ActionController<TId> : IActionController<TId>
    where TId : class
{
    private bool _canSetMore;

    public ActionInfo<TId> ActionInfo { get; private set; } = new();

    public ActionController() { }
    public ActionController(Delegate @delegate) =>
        SetActionExpectedNext(@delegate.Method.Name);

    public ActionController(string actionName) =>
        SetActionExpectedNext(actionName);

    public bool IsUserAction() =>
        !ActionInfo.ExpectedPlayers.IsNullOrEmpty() &&
        ActionInfo.Actions.All(a => a.IsUserAction);

    public IChainedOperation<TId> SetActionDone(string name, TId? userId = null)
    {
        _canSetMore = false;

        var requiresUserAction = ActionInfo.RequiresUserAction();
        if (requiresUserAction && userId is null ||
            !requiresUserAction && userId is not null)
            return this.Failure();

        if (!ActionInfo.HasAction(name))
            return this.Failure();

        if (userId is not null && !ActionInfo.HasUser(userId))
            return this.Failure();

        if (!ActionInfo.CanMakeAction(name, userId))
            return this.Failure();

        var action = ActionInfo.Actions.First(a => a.Name == name);
        var otherActions = ActionInfo.Actions.Where(a => a != action).ToArray();

        action = new(action.Name, action.IsUserAction, DoneBefore: true,
            action.AlreadyMadeActionByPlayers.Append(userId).Distinct().ToArray(),
            action.Repeat);

        ActionInfo = new()
        {
            Actions = otherActions.Append(action).ToArray(),
            ExpectedPlayers = ActionInfo.ExpectedPlayers,
            MustObeyOrder = ActionInfo.MustObeyOrder
        };

        return this.Success();
    }

    public IChainedOperation<TId> SetActionExpectedNext(string name, ActionRepeat repeat = default)
    {
        if (!ActionInfo.HasDoneActions())
            return this.Success();

        _canSetMore = true;

        ActionInfo = new()
        {
            Actions = new[] { new ActionData<TId>(name, IsUserAction: false, DoneBefore: false, Array.Empty<TId>(), repeat) },
        };

        return this.Success();
    }

    public IChainedOperation<TId> Or(string name, ActionRepeat repeat = default)
    {
        if (!_canSetMore)
            return this.Success();

        ActionInfo = new()
        {
            Actions = ActionInfo.Actions.Append(
                new ActionData<TId>(name, IsUserAction: false, DoneBefore: false, Array.Empty<TId>(), repeat)).ToArray(),
        };

        return this.Success();
    }

    public bool CanMakeAction(string name, TId? user = null) => ActionInfo.CanMakeAction(name, user);

    public IChainedOperation<TId> By(TId[] userIds, bool mustObeyOrder = false)
    {
        if (!ActionInfo.HasActions())
            return this.Failure();

        if (ActionInfo.ExpectedPlayers.Any())
            return this.Success();

        ActionInfo = new()
        {
            Actions = ActionInfo.Actions.Select(a =>
                new ActionData<TId>(a.Name, IsUserAction: true, a.DoneBefore, a.AlreadyMadeActionByPlayers, a.Repeat)).ToArray(),

            ExpectedPlayers = userIds,
            MustObeyOrder = mustObeyOrder,
        };

        return this.Success();
    }
}

public interface IActionController<TId>
    where TId : class
{
    #region By Name String

    IChainedOperation<TId> SetActionDone(string name, TId? userId = null);
    IChainedOperation<TId> SetActionExpectedNext(string name, ActionRepeat repeat = default);
    IChainedOperation<TId> Or(string name, ActionRepeat repeat = default);
    bool CanMakeAction(string name, TId? user = null);

    #endregion

    #region By Name Delegate

    IChainedOperation<TId> SetActionDone(Delegate @delegate, TId? userId = null) => SetActionDone(@delegate.Method.Name, userId);
    IChainedOperation<TId> SetActionExpectedNext(Delegate @delegate, ActionRepeat repeat = default) => SetActionExpectedNext(@delegate.Method.Name, repeat);
    IChainedOperation<TId> Or(Delegate @delegate, ActionRepeat repeat = default) => Or(@delegate.Method.Name, repeat);
    bool CanMakeAction(Delegate @delegate, TId? user = null) => CanMakeAction(@delegate.Method.Name, user);

    #endregion

    #region Other

    IChainedOperation<TId> By(TId[] userIds, bool mustObeyOrder = false);
    bool IsUserAction();

    ActionInfo<TId> ActionInfo { get; }

    #endregion
}

public interface IChainedOperation<TId> : IActionController<TId>
    where TId : class
{
    bool IsSuccess { get; }
}

public class ChainedOperation<TId> : IChainedOperation<TId>
    where TId : class
{
    private readonly IActionController<TId> _controller;

    public ChainedOperation(
        IActionController<TId> controller, bool isSuccess)
    {
        _controller = controller;
        IsSuccess = isSuccess;
    }

    public static void Create(IActionController<TId> controller, bool isSuccess) =>
        new ChainedOperation<TId>(controller, isSuccess);

    public bool IsSuccess { get; }

    public IChainedOperation<TId> SetActionDone(string name, TId? userId = null)
    {
        //if (!IsSuccess)
            // return this;

        return _controller.SetActionDone(name, userId);
    }

    public IChainedOperation<TId> SetActionExpectedNext(string name, ActionRepeat repeat = default)
    {
        //if (!IsSuccess)
            // return this;

        return _controller.SetActionExpectedNext(name, repeat);
    }

    public bool CanMakeAction(string name, TId? user = null) =>
        _controller.CanMakeAction(name, user);

    public IChainedOperation<TId> By(TId[] userIds, bool mustObeyOrder = false)
    {
        //if (!IsSuccess)
            // return this;

        return _controller.By(userIds, mustObeyOrder);
    }

    public IChainedOperation<TId> Or(string name, ActionRepeat repeat = ActionRepeat.Single)
    {
        //if (!IsSuccess)
            // return this;

        return _controller.Or(name, repeat);
    }

    public bool IsUserAction() => _controller.IsUserAction();

    public ActionInfo<TId> ActionInfo => _controller.ActionInfo;
}

public static class ChainedOperationExtensions
{
    public static IChainedOperation<TId> Success<TId>(this IActionController<TId> controller) where TId : class =>
        new ChainedOperation<TId>(controller, true);

    public static IChainedOperation<TId> Failure<TId>(this IActionController<TId> controller) where TId : class =>
        new ChainedOperation<TId>(controller, false);
}

public enum ActionRepeat
{
    Single,
    Multiple
}

public class ActionInfo<TId>
    where TId : class
{
    public ActionData<TId>[] Actions { get; init; } = Array.Empty<ActionData<TId>>();
    public bool MustObeyOrder { get; init; }
    public TId[] ExpectedPlayers { get; init; } = Array.Empty<TId>();

    public bool CanMakeAction(string name, TId? userId = null)
    {
        if (name.IsNullOrEmpty())
            return false;

        if (userId is null && !ExpectedPlayers.IsNullOrEmpty())
            return false;

        if (userId is not null && ExpectedPlayers.IsNullOrEmpty())
            return false;

        // Check if user done any single time action before, which forbids to make any more
        var singleTimeActions = Actions.Where(a => a.Repeat is ActionRepeat.Single).ToArray();
        if (singleTimeActions.Any(a => !a.IsUserAction && a.DoneBefore) ||
            singleTimeActions.Any(a => a.IsUserAction && a.AlreadyMadeActionByPlayers.Contains(userId)))
            return false;

        // Check if given action is even expected 
        var action = Actions.FirstOrDefault(a => a.Name == name);
        if (!name.IsNullOrEmpty() && action is null)
            return false;

        if (MustObeyOrder && !ExpectedPlayers.IsNullOrEmpty())
        {
            int expectedOrderIndex = Array.FindIndex(ExpectedPlayers, id => id.Equals(userId));
            if (expectedOrderIndex < 0)
                return false;

            var usersBefore = ExpectedPlayers.Take(expectedOrderIndex).ToArray();
            var allUsersThatDoneSingleActions = singleTimeActions
                .SelectMany(a => a.AlreadyMadeActionByPlayers).Distinct().ToArray();

            if (Enumerable.SequenceEqual(usersBefore, allUsersThatDoneSingleActions))
                return true;
        }
        else
        {
            if (action is null) 
                return false;

            if (action.Repeat is ActionRepeat.Multiple)
                return true;

            if (!action.AlreadyMadeActionByPlayers.Contains(userId))
                return true;
        }

        return false;
    }

    public bool HasDoneActions()
    {
        if (Actions.IsNullOrEmpty())
            return true;

        var singleTimeActions = Actions.Where(a => a.Repeat is ActionRepeat.Single).ToArray();
        if (ExpectedPlayers.IsNullOrEmpty())
        {
            if (!singleTimeActions.Any(a => a.DoneBefore))
                return false;
        }
        else
        {
            var alreadyMadeActionPlayers = singleTimeActions
                .SelectMany(a => a.AlreadyMadeActionByPlayers)
                .Distinct()
                .ToArray();

            if (!Enumerable.SequenceEqual(alreadyMadeActionPlayers, ExpectedPlayers))
                return false;
        }

        return true;
    }

    public bool HasActions() =>
        !Actions.IsNullOrEmpty();

    public bool HasAction(string name) =>
        Actions.Any(a => a.Name == name);

    public bool HasUser(TId userId) =>
        ExpectedPlayers.Contains(userId);

    public bool RequiresUserAction() => ExpectedPlayers.Any();

    public ActionData<TId>? GetAction(string name) =>
        Actions.FirstOrDefault(a => a.Name == name);

    public string[] GetActionNames() =>
        Actions.Select(a => a.Name).ToArray();
}

public record ActionData<TId>(
    string Name,
    bool IsUserAction,
    bool DoneBefore,
    TId?[] AlreadyMadeActionByPlayers,
    ActionRepeat Repeat = default)
    where TId : class;
