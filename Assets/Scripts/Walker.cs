using UnityEngine;

public class Walker
{
    public Vector3Int position;
    public float angle;
    public bool exists = false;
    
    public Walker(Vector3Int _p)
    {
        position = _p;
        exists = true;
        angle = Random.Range(0, Mathf.PI * 2);
    }

    public Walker()
    {
        exists = false;
    }
}
