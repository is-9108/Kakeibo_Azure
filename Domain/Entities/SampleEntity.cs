namespace Kakeibo.Domain.Entities;

/// <summary>
/// サンプルエンティティ（家計簿の例として）
/// </summary>
public class SampleEntity : Entity
{
    public string Name { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }

    private SampleEntity() { }

    public static SampleEntity Create(string name, decimal amount)
    {
        return new SampleEntity
        {
            Name = name,
            Amount = amount
        };
    }

    public void Update(string name, decimal amount)
    {
        Name = name;
        Amount = amount;
        UpdatedAt = DateTime.UtcNow;
    }
}
