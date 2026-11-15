[System.Serializable] public class GoodDict<T>
{
    public DictElement<T>[] values;

    public GoodDict(int length)
    {
        values = new DictElement<T>[length];
    }

    public bool hasKey(string s)
    {
        foreach(DictElement<T> x in values)
        {
            if (x.key.Equals(s))
            {
                return true;
            }
        }
        return false;
    }
    public T get(string key)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i].key.Equals(key))
            {
                return values[i].value;
            }
        }
        return default(T);
    }

    public void set(string key, T value)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i].key.Equals(key))
            {
                values[i].value = value;
            }
        }
    }

    public T valAt(int index)
    {
        return values[index].value;
    }

    public string keyAt(int index)
    {
        return values[index].key;
    }

    public int Length()
    {
        return values.Length;
    }
}