namespace Game.Battle.UseCases.Queries
{
    public record GetBattleQuery();
    public record GetBattleQueryResponse(
        BattlefieldDTO Battlefield);

    public record BattlefieldDTO(
        PlayerDTO[] Players,
        AssemblyFieldDTO AssemblyField);

    public record PlayerDTO(
        CharacterDTO[] Characters,
        ActionCardDTO[] ActionCards);

    public record CharacterDTO();

    public record ActionCardDTO();

    public record AssemblyFieldDTO(
        string PlayerId,
        ActionCardDTO ActionCard);
}
