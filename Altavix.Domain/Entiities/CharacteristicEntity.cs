namespace Altavix.Domain;

public class CharacteristicEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; } = true;
}
