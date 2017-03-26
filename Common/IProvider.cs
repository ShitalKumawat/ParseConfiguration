namespace Common
{
    public interface IProvider<T>
    {
        T Read();
    }
}
