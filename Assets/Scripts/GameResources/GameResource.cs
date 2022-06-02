using System;

[Serializable]
public class GameResource
{
    public string code;
    public int amount;

    public GameResource(string code, int amount)
    {
        this.code = code;
        this.amount = amount;
    }

    public void AddOrRemove(int value)
    {
        amount += value;
        if (amount < 0) amount = 0;
    }
}
