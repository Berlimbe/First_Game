public enum ItemType { Consumable, Equipment }

public class Item
{
    public string Name { get; set; }
    public ItemType Type { get; set; }
    public int EffectValue { get; set; } 
}