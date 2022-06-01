public class GameResource
{
    public string Name { get; }
    public int Amount { get; private set; }

    public GameResource(string name, int amount)
    {
        Name = name;
        Amount = amount;
    }

    public void AddOrRemove(int value)
    {
        Amount += value;
        if (Amount < 0) Amount = 0;
    }
}
