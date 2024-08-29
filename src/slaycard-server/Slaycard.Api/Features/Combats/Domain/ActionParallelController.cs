using Slaycard.Api.Core.Domain;

namespace Slaycard.Api.Features.Combats.Domain;

public class ActionParallelController<TId>(IEnumerable<TId> ids)
    where TId : notnull
{
    private readonly Dictionary<TId, bool> _done = ids.ToDictionary(x => x, x => false);

    public bool IsAllDone { get; private set; }

    public void Run(TId id, Action action)
    {
        if (!_done.TryGetValue(id, out bool done))
            throw new DomainException("not_expected_id");

        if (done)
            throw new DomainException("already_done_action_err");

        action();

        _done[id] = true;

        IsAllDone = _done.All(kv => kv.Value);
    }
}
