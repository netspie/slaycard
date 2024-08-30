using Mediator;
using static Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications.ClientBattleNotification;

namespace Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications;

public record ClientBattleNotification(
    MetaData Metadata,
    object[] Notifications) : INotification
{
    public record MetaData(
        string BattleId,
        string NextPlayerId,
        string NextUnitId);
};
