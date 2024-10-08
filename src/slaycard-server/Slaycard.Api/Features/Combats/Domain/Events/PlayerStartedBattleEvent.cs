﻿namespace Slaycard.Api.Features.Combats.Domain.Events;

public record PlayerStartedBattleEvent(
    BattleId BattleId,
    PlayerId PlayerId) : BattleEvent(BattleId, PlayerId);
