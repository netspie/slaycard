﻿using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Combats.Domain.Events;

public record MissedEvent(
    BattleId BattleId,
    PlayerId PlayerId,
    UnitId OriginUnitId,
    UnitId TargetUnitId) : BattleEvent(BattleId, PlayerId);
