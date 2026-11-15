using UnityEngine;

public class Item : MonoBehaviour
{
    public int value;
    public SpriteRenderer sprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(Sprite image, int v)
    {
        sprite.sprite = image;
        value = v;
    }
}
