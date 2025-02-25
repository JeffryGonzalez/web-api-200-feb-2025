namespace ProtoType.Api.Services;

public class HitCounter
{

    public int Count { get; private set; }

    public void Increment()
    {
        Count++;
    }
}
