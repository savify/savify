using App.BuildingBlocks.Domain;

namespace App.Modules.Transactions.Domain.Transactions;

public class Transaction : Entity, IAggregateRoot
{
    public TransactionId Id { get; }

    private TransactionType _type;

    private Source _source;

    private Target _target;

    private DateTime _madeOn;

    private string _comment;

    private ICollection<string> _tags;

    public static Transaction AddNew(TransactionType type, Source source, Target target, DateTime madeOn, string comment, IEnumerable<string> tags)
    {
        return new Transaction(type, source, target, madeOn, comment, tags);
    }

    public Transaction(TransactionType type, Source source, Target target, DateTime madeOn, string comment, IEnumerable<string> tags)
    {
        Id = new TransactionId(Guid.NewGuid());
        _type = type;
        _source = source;
        _target = target;
        _madeOn = madeOn;
        _comment = comment;
        _tags = tags.ToList();

        AddDomainEvent(new TransactionAddedDomainEvent(Id, _type, _source, _target));
    }

    public Transaction()
    { }
}
