namespace DataExtractors;

public interface IData
{
    public int[] Any();
    public int[] Target(uint z);
    public int Sum(uint z);

    public State GetState();

    public bool IsActive();
    public bool IsInactive();
}