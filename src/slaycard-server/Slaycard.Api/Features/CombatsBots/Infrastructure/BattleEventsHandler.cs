using Mediator;
using Slaycard.Api.Core.Exceptions;
using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications;
using Slaycard.Api.Features.Combats.Infrastructure.ClientNotifications.Content;
using Slaycard.Api.Features.CombatsBots.Domain;
using Slaycard.Api.Features.CombatsBots.Infrastructure;

namespace Slaycard.Api.Features.CombatsBots.EventHandlers;

public record BattleEventsHandler(
    BotTimeoutClock BotTimeoutClock,
    IBotRepository BotRepository,
    IBattleRepository BattleRepository) :
    INotificationHandler<ClientBattleNotification>
{
    public async ValueTask Handle(ClientBattleNotification n, CancellationToken ct)
    {
        //BotTimeoutClock.Update(new PlayerId(n.Metadata));

        var ns = n.Notifications;

        var gameOverN = n.Notifications.OfType<GameOverClientNotification>().FirstOrDefault();
        if (gameOverN is not null)
            ns = [gameOverN];

        foreach (var subN in ns)
        {
            if (subN is BotCreatedClientNotification botCreated)
            {
                await BotRepository.Add(
                    new Bot(
                        new PlayerId(botCreated.BotId),
                        new BattleId(botCreated.BattleId)));

                continue;
            }

            if (subN is PassedClientNotification passed)
            {
                var bot = await Ex.TryCatch(() => BotRepository.Get(new PlayerId(n.Metadata.NextPlayerId)));
                if (bot is null)
                    continue;

                var battle = await Ex.TryCatch(() => BattleRepository.Get(new BattleId(passed.BattleId)));
                if (battle is null)
                    continue;

                battle.ApplyArtifact(
                    new PlayerId(n.Metadata.NextPlayerId),
                    new UnitId(n.Metadata.NextUnitId),
                    new ArtifactId("attack"));

                await BattleRepository.Update(battle);

                continue;
            }

            if (subN is GameOverClientNotification over)
            {
                var playerIds = over.PlayerIds.Map(id => new PlayerId(id));

                await BotRepository.DeleteMany(playerIds);
                await Ex.TryCatch(() => BattleRepository.Delete(new BattleId(over.BattleId)));

                return;
            }
        }
    }
}
