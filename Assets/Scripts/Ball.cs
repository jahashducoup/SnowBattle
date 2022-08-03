using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float  SPEED;
    private float speed;
    public float speedMult;
    public Rigidbody2D _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        ResetPos();
    }

    public void ResetPos()
    {
        _rigidbody.position = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
        speed = SPEED;
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1);
        Kickoff();
    }

    private void Kickoff()
    {
        Vector2 direction = Random.insideUnitCircle.normalized;
        while (Mathf.Abs(direction.x) < 0.1 || Mathf.Abs(direction.y) < 0.1)
        {
            direction = Random.insideUnitCircle.normalized;
        }
        //direction = new Vector2(0, -1f);
        _rigidbody.velocity = direction.normalized * speed;
        Debug.Log("velocity on kickoff : " + _rigidbody.velocity);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.GetContact(0).normal;
        Vector2 myCoord = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y);
        Debug.Log("velocity on impact : " + myCoord);
        Debug.Log("normal : " + normal);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -_rigidbody.velocity.y);
        /*if (normal.x != 0)
        {
            _rigidbody.velocity = new Vector2(-_rigidbody.velocity.x, _rigidbody.velocity.y); //*speedMult
        }
        else
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -_rigidbody.velocity.y); //*speedMult
        }*/
        Debug.Log("velocity after bounce : " + _rigidbody.velocity);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 myCoord = _rigidbody.transform.position;
        //Vector2 impact = other.ClosestPoint(myCoord);
        Vector2 impact = other.transform.position;
        Vector2 normal=new Vector2(0,0);
        if (Mathf.Abs(myCoord.y-impact.y)<0.01 && myCoord.x < impact.x)
        {
            normal = new Vector2(-1, 0);
        }
        else if (Mathf.Abs(myCoord.y - impact.y) < 0.01 && myCoord.x > impact.x)
        {
            normal = new Vector2(1, 0);
        }
        else if (Mathf.Abs(myCoord.x - impact.x) < 0.01 && myCoord.y < impact.y)
        {
            normal = new Vector2(0, -1);
        }
        else if (Mathf.Abs(myCoord.x - impact.x) < 0.01 && myCoord.y > impact.y)
        {
            normal = new Vector2(0, 1);
        }
        else if(myCoord.y - impact.y > myCoord.x - impact.x)
        {
            normal = new Vector2(0, -1);
        }
        Debug.Log("ball pos : " + myCoord + ", impact point : " + impact + ", normal : " + normal);
        float angle = Vector2.Angle(_rigidbody.velocity, normal);
        float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
        float tx = _rigidbody.velocity.x;
        float ty = _rigidbody.velocity.y;
        //Debug.Log("angle : " + angle);
        if (normal.x == 1)
        {
            _rigidbody.velocity = new Vector2(cos * tx + sin * ty, -sin * tx - cos * ty);
        }
        if (normal.x == -1)
        {
            _rigidbody.velocity = new Vector2(cos * tx + sin * ty, sin * tx - cos * ty);
        }
        if (normal.y == 1)
        {
            _rigidbody.velocity = new Vector2(-cos * tx - sin * ty, sin * tx + cos * ty);
        }
        if (normal.y == -1)
        {
            _rigidbody.velocity = new Vector2(-cos * tx + sin * ty, sin * tx + cos * ty);
        }
        _rigidbody.velocity *= speedMult;
        //_rigidbody.transform.Rotate(0, 0, 2*angle);
        //_rigidbody.velocity = Vector2.(_rigidbody.velocity, normal);
        Debug.Log("velocity after bounce : " + _rigidbody.velocity);
    }
}
