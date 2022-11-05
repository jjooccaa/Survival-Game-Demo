
public class ItemBlueprint
{
    public string itemName;

    public int numberOfItemsToSpawn;

    public int numberOfRequirements;

    public string req1;
    public int req1Amount;

    public string req2;
    public int req2Amount;

    public ItemBlueprint(string name, int numOfItemsToSpawn, int numOfReq, string r1Name, int r1Amount, string r2Name, int r2Amount)
    {
        itemName = name;

        numberOfItemsToSpawn = numOfItemsToSpawn;

        numberOfRequirements = numOfReq;

        req1 = r1Name;
        req2 = r2Name;

        req1Amount = r1Amount;
        req2Amount = r2Amount;

    }

    public ItemBlueprint(string name, int numOfItemsToSpawn, int numOfReq, string r1Name, int r1Amount)
    {
        itemName = name;

        numberOfItemsToSpawn = numOfItemsToSpawn;

        numberOfRequirements = numOfReq;

        req1 = r1Name;

        req1Amount = r1Amount;
    }
}
