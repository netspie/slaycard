namespace Game.Battle.UseCases.Events
{
    public record ActionCardAssembledEvent(
        string BattleId,
        string PlayerId,
        string ActionCardId)
    {
        public record AssembledCardDTO(
            string AssembledCardId);
    }
}
