#nullable enable

using Core.Domain;
using System;

namespace Game.Battle.Domain.Artifacts
{
    public record AssembleArtifactResult(
        IDomainEvent[] Events,
        Artifact? Artifact = null)
    {
        public static readonly AssembleArtifactResult Default = new(Array.Empty<IDomainEvent>(), null);
    }
}
