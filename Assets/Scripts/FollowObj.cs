using UnityEngine;

public class FollowObj : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Vector3.Distance(target.transform.position,transform.position)>.1f)
            transform.position = target.transform.position + offset;
    }
}
