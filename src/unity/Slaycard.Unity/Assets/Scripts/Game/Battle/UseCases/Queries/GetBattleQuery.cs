namespace Game.Battle.UseCases.Queries
{
    public record GetBattleQuery();
    public record GetBattleQueryResponse(
        BattleDTO Battlefield);

    public record BattleDTO(
        string Id,
        string CurrentTurnPlayerId,
        PlayerDTO[] Players,
        AssemblyFieldDTO AssemblyField = null);

    public record PlayerDTO(
        string Id,
        CharacterCardDTO[] Characters,
        ActionCardDTO[] ActionCards = null);

    public record CharacterCardDTO(
        string Id,
        string Name);

    public record ActionCardDTO(
        string Id,
        string Name,
        string PlayerId);

    public record AssemblyFieldDTO(
        ActionCardDTO ActionCard = null);
}
