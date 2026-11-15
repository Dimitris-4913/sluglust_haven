using System.Threading;
using UnityEngine;

public class NioController : MonoBehaviour
{
    public SaltChunk bulletPrefab;
    public GameManager manager;
    public SpriteRenderer sprite;
    public Sprite idle;
    public Sprite yeet;
    public float chargeTime;
    public float chargeDur = 2;
    public int squashDir = 0;
    public float squashTime = 0.3f;
    public float squashPercent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if((manager.player.transform.position - transform.position).magnitude<10)
        {
            chargeTime += Time.deltaTime;
            if(chargeTime > chargeDur)
            {
                chargeTime -= chargeDur;
                sprite.sprite = yeet;
                squashDir = 1;
                SaltChunk projectile = Instantiate(bulletPrefab);
                //projectile.tilemap = manager.trailTmap;
                projectile.angle = (manager.player.transform.position - transform.position).normalized;
            }
        } else
        {
            if(chargeTime > 0)
            {
                chargeTime = 0;
            }
        }

        if(squashDir != 0)
        {
            squashPercent += (squashDir * Time.deltaTime * 2) / squashTime;
            if(squashPercent > 0.2f)
            {
                squashDir = -1;
            }
            if(squashPercent < 0)
            {
                sprite.sprite = idle;
                squashDir = 0;
                squashPercent = 0;
                sprite.transform.localScale = Vector3.one;
            } else
            {
                sprite.transform.localScale = new Vector2(1+ squashPercent, 1-squashPercent)*0.5f;
            }
        }
    }

}
