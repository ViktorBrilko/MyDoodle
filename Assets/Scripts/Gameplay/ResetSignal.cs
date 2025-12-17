public class ResetSignal<T>
{
    public T Resetable { get; }

    public ResetSignal(T resetable)
    {
        Resetable = resetable;
    }
}