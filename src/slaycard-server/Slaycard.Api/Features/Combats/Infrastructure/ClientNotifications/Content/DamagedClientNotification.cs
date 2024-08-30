using Mediator;
using Slaycard.Api.Features.Combats.Domain.Events;

namespace Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications.Content;

public record DamagedClientNotification(
    string BattleId,
    string OriginPlayerId,
    string OriginUnitId,
    string TargetPlayerId,
    string TargetUnitId,
    double Damage,
    bool IsCritic) : Notification("Damaged"), INotification;

public static class DamagedEvent_To_ClientNotification_Converter
{
    public static DamagedClientNotification ToNotification(this DamagedEvent ev) =>
        new(ev.BattleId.Value,
            ev.OriginPlayerId.Value,
            ev.OriginUnitId.Value,
            ev.TargetPlayerId.Value,
            ev.TargetUnitId.Value,
            ev.Damage,
            ev.IsCritic);
}
