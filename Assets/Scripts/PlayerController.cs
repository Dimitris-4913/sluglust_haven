using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public TMP_Text text;
    public int cash;
    public int health = 100;
    public float bounceTime = 0.7f;
    public float bounceInitial;
    public float bounceHeight = 0.25f;
    public float[,] inventoryBounce = new float[6,2];
    public GameManager manager;
    public Item nearestItem;
    string curAnimString = "10";
    public GoodDict<Sprite[]> anims;
    public Sprite[] curAnim;
    public SpriteRenderer sprite;
    public float animTime;
    public float frameTime = 0.15f;
    float frameTimeMult = 1f;
    public Vector2 MoveDir;
    public Item[] inventory = new Item[6];

    public int speed;
    public float accelTime;
    public float accelProgress;
    public Rigidbody2D rigidbody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        bounceInitial = Mathf.Sqrt((2*bounceHeight) / bounceTime);
    }

    // Update is called once per frame
    void Update()
    {
        MoveDir = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            MoveDir += Vector2.up;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            MoveDir += Vector2.left;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            MoveDir += Vector2.down;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            MoveDir += Vector2.right;
        }

        if(MoveDir.magnitude > 0)
        {
            accelProgress = Mathf.Min(1, accelProgress + (Time.deltaTime / accelTime));
            string moveString = MoveDir.x + "" + MoveDir.y;
            if (!curAnimString.Equals(moveString) && anims.hasKey(moveString))
            {
                curAnimString = moveString;
                ChangeAnimation(anims.get(moveString), false);
            }
        } else {
            accelProgress = Mathf.Max(0, accelProgress - (Time.deltaTime / accelTime));
        }
        frameTimeMult = 1 - (accelProgress*0.35f);

        transform.position += (Vector3) (MoveDir.normalized * speed * accelProgress * Time.deltaTime);


        animTime += (Time.deltaTime / (frameTime*frameTimeMult));
        if (animTime > curAnim.Length)
        {
            animTime -= curAnim.Length;
        }
        sprite.sprite = curAnim[Mathf.Clamp(Mathf.FloorToInt(animTime), 0, curAnim.Length - 1)];


        for(int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                inventoryBounce[i, 0] += inventoryBounce[i, 1]*Time.deltaTime;
                inventoryBounce[i, 1] -= (2*Time.deltaTime*bounceInitial) / bounceTime;
                if (inventoryBounce[i,1]<0 && inventoryBounce[i, 0] < 0)
                {
                    inventoryBounce[i, 0] = 0;
                    inventoryBounce[i,1] = bounceInitial;
                }
            }
        }

        if (curAnimString.Equals("01"))
        {
            for(int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] != null)
                {
                    inventory[i].transform.position = transform.position + new Vector3(0,-0.05f*(i+2) + inventoryBounce[i,0], -0.05f* (i + 1));
                }
            }
        }
        else if (curAnimString.Equals("0-1"))
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] != null)
                {
                    inventory[i].transform.position = transform.position + new Vector3(0, 0.05f * (i + 2) + inventoryBounce[i,0], 0.05f * (i+1));
                }
            }
        }
        else if(curAnimString.Equals("10"))
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] != null)
                {
                    inventory[i].transform.position = transform.position + new Vector3(-0.2f*(i+1), 0.1f + inventoryBounce[i,0] , -0.05f);
                }
            }
        }
        else if(curAnimString.Equals("-10"))
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] != null)
                {
                    inventory[i].transform.position = transform.position + new Vector3(0.2f * (i + 1), 0.1f + inventoryBounce[i,0], -0.05f);
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (nearestItem != null)
            {
                if (inventory[0] != null)
                {
                    manager.loot.Add(inventory[0]);
                    inventory[0].transform.position = nearestItem.transform.position;
                }
                manager.loot.Remove(nearestItem);
                inventory[0] = nearestItem;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TryGrabToSpot(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TryGrabToSpot(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TryGrabToSpot(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            TryGrabToSpot(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            TryGrabToSpot(5);
        }

        text.text = "Health: "+health+"    Money: "+cash;

        if(health < 0)
        {
            SceneManager.LoadScene("Death");
        }
    }

    public void TryGrabToSpot(int spot)
    {
        if (nearestItem != null)
        {
            if (inventory[spot] != null)
            {
                manager.loot.Add(inventory[spot]);
                inventory[spot].transform.position = nearestItem.transform.position;
            }
            manager.loot.Remove(nearestItem);
            inventory[spot] = nearestItem;
            inventoryBounce[spot,0] = 0;
            inventoryBounce[spot,1] = bounceInitial;
            inventory[spot].sprite.color = Color.white;
        }

    }

    public void ChangeAnimation(Sprite[] newAnim, bool restart, bool _loop = false, float _time = 0.15f, string _end = "")
    {
        if (newAnim.Length == 0)
        {
            newAnim = anims.values[0].value;
        }
        curAnim = newAnim;
        frameTime = _time;
        if (restart)
        {
            animTime = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Deposit"))
        {
            for(int i = 0; i < 6; i++)
            {
                if (inventory[i] != null)
                {
                    cash += inventory[i].value;
                    Destroy(inventory[i].gameObject);
                    inventory[i] = null;
                }
            }
        }
    }
}
