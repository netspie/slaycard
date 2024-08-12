namespace Slaycard.Api.Features.Combats.UseCases;

public record ApplyArtifactApiCommand(
    string SourceUnitId,
    string ArtifactId,
    string TargetUnitId);

public record ApplyArtifactCommand(
    string PlayerId,
    string SourceUnitId,
    string ArtifactId,
    string TargetUnitId);
