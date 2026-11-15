using UnityEngine;
using UnityEngine.Tilemaps;

public class SaltChunk : MonoBehaviour
{
    float life = 0;
    public Vector2 angle;
    //public Tilemap tilemap;
    public Tile saltTile;
    public int speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        life += Time.deltaTime;
        if(life > 50)
        {
            Destroy(gameObject);
        }
        transform.position += (Vector3)angle * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().health -= 10;
        }
    }
}
