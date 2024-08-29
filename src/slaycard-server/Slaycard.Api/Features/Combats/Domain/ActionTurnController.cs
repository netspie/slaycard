using Slaycard.Api.Core.Domain;

namespace Slaycard.Api.Features.Combats.Domain;

public class ActionTurnController<TId>(IEnumerable<TId> ids)
    where TId : notnull
{
    public TId[] Ids { get; } = ids.ToArray();

    private int _currentIndex;

    public void Run(TId id, Action action)
    {
        if (!Ids[_currentIndex].Equals(id))
            throw new DomainException("not_your_turn_err");

        action();

        _currentIndex = Ids.Length - 1 == _currentIndex ? 0 : _currentIndex + 1;
    }
}
