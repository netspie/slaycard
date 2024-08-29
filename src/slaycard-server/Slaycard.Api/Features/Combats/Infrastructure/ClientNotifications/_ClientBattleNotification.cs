using static Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications.ClientBattleNotification;

namespace Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications;

public record ClientBattleNotification(
    MetaData Metadata,
    object Notification)
{
    public record MetaData(string NextUnitId);
};