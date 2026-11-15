[System.Serializable]
public class DictElement<T>
{
    public string key;
    public T value;

    public DictElement(string _k, T _v)
    {
        key = _k;
        value = _v;
    }
}
