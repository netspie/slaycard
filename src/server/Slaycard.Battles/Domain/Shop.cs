namespace Slaycard.Battles.Domain;

public record ShopEventHandler()
{
    public async Task On(MoneyWithdrawedEvent @event)
    {

    }

    public async Task On(MoneyWithdrawNotAllowedEvent @event)
    {

    }
}

public record RequestPurchaseCommand(
    string BuyerId,
    string ShopItemId);

public record RequestPurchaseCommandHandler(
    IBuyerRepository BuyerRepository,
    IShopItemRepository ShopItemRepository)
{
    public async Task Handle(RequestPurchaseCommand command)
    {
        var buyer = await BuyerRepository.GetById(new BuyerId(command.BuyerId));
        var shopItem = await ShopItemRepository.GetById(new ShopItemId(command.ShopItemId));

        buyer.RequestPurchase(shopItem);
    }
}

public interface IBuyerRepository
{
    Task<Buyer> GetById(BuyerId buyerId);
}

public interface IShopItemRepository
{
    Task<ShopItem> GetById(ShopItemId shopItem);
}

public class ShopItem
{
    public ShopItemId ShopItemId { get; init; }
    public Money Cost { get; private set; }
}

public record ShopItemId(string Value);

public class Buyer : AggregateRoot
{
    public BuyerId Id { get; init; }
    public Money Money { get; private set; }

    public void RequestPurchase(ShopItem shopItem)
    {
        if (Money < shopItem.Cost)
            throw new Exception("NO_MONEY: Not enough money to buy this item");

        Money -= shopItem.Cost;

        AddEvent(
            new PurchaseRequestedEvent(Id, shopItem));
    }
}

public record BuyerId(string Value);

public record PurchaseRequestedEvent(
     BuyerId BuyerId,
     ShopItem ShopItem) : IDomainEvent;

public class AggregateRoot
{
    private readonly List<IDomainEvent> _events = new();

    public IEnumerable<IDomainEvent> Events => _events;

    public void AddEvent(IDomainEvent @event) =>
        _events.Add(@event);
}

public interface IDomainEvent
{

}

public record Money(decimal Value)
{
    public static Money operator +(Money l, Money r) =>
        new Money(l.Value + r.Value);

    public static Money operator -(Money l, Money r) =>
       new Money(l.Value - r.Value);

    public static bool operator <(Money l, Money r) =>
        l.Value < r.Value;

    public static bool operator >(Money l, Money r) =>
        l.Value > r.Value;
}
