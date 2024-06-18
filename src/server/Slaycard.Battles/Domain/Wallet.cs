
namespace Slaycard.Battles.Domain;

public record WalletEventHandler()
{
    public async Task On(PurchaseRequestedEvent @event)
    {

    }
}

public record TakeMoneyCommand(
    string WalletId,
    decimal Amount);

public record TakeMoneyCommandHandler(
    IWalletRepository WalletRepository)
{
    public async Task Handle(TakeMoneyCommand command)
    {
        var wallet = await WalletRepository.GetById(new WalletId(command.WalletId));

        wallet.TakeMoney(new Money(command.Amount));

        // publish events.. store..
    }
}

public interface IWalletRepository
{
    Task<Wallet> GetById(WalletId walletId);
}

public class Wallet : AggregateRoot
{
    public WalletId Id { get; init; }
    public Money Money { get; private set; }

    public void TakeMoney(Money money)
    {
        if (Money < money)
        {
            AddEvent(new MoneyWithdrawNotAllowedEvent(Id, money));
            return;
        }

        Money -= money;

        AddEvent(new MoneyWithdrawedEvent(Id, money));
    }
}

// Withdraw?
public record MoneyWithdrawedEvent(
    WalletId WalletId, 
    Money Amount) : IDomainEvent;

public record MoneyWithdrawNotAllowedEvent(
    WalletId WalletId,
    Money Amount) : IDomainEvent;

public record WalletId(string Value);
