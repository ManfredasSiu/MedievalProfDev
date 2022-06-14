using System;

[Serializable]
public class GameResource
{
    public BaseResources code;
    public int amount;

    public GameResource(BaseResources code, int amount)
    {
        this.code = code;
        this.amount = amount;
    }

    public void TurnCostToNegative()
    {
        if (amount >= 0)
        {
            amount = -amount;
        }
    }

    public void AddOrRemove(int value)
    {
        amount += value;
        if (amount < 0) amount = 0;
    }
}
