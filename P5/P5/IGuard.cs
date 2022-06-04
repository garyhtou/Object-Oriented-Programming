namespace Guards;

public interface IGuard
{
    public int Value(uint x);

    public void Toggle();
}