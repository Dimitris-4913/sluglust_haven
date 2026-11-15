using UnityEngine;

public class Global
{
    public static bool RandomChance(float denom)
    {
        return Random.Range(0, denom) < 1;
    }
}
