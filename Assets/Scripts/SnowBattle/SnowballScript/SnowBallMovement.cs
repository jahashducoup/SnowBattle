using System;
using Unity.VisualScripting;
using UnityEngine;

public class SnowBallMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3 shotPoint;
    private Rigidbody2D body;
    private Vector3 initialPos;
    private float angle;
    public GameObject snowball_ImpactEffect;
    private bool dead;
    public Vector3 masterSpawnPosition;
    public string SpawnedBy;
    public bool mustKillAtFirstCollide = false;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        dead = false;
        speed = 15f;
        //initialPos = transform.position;
        //shotPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //angle = Mathf.Atan2(shotPoint.y-initialPos.y, shotPoint.x-initialPos.x);
        //transform.rotation = Quaternion.Euler(0f, 0f, angle*Mathf.Rad2Deg);        
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == false)
        {
            // to implement with mouse
            //body.velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f)*speed; 
            body.velocity = new Vector3(Mathf.Sign(transform.position.x-masterSpawnPosition.x), 0f, 0f) * speed;
        }
        else
        {
            body.velocity = Vector3.zero;
            Instantiate(snowball_ImpactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(mustKillAtFirstCollide == true 
        && (other.tag == "Wall"))
        {
            dead = true;
        }

        if ((other.tag == "Border" 
        || other.tag == "Player1" 
        || other.tag == "Player2" 
        || other.tag == "Player3" 
        || other.tag == "Player4"))
        {
            if(other.tag != SpawnedBy)
            {
                dead = true;
            }
        }
    }
}
