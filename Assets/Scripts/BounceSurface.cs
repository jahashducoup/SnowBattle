using UnityEngine;

public class BounceSurface : MonoBehaviour
{
    public float bounceStrength;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            Vector2 normal = collision.GetContact(0).normal;
            Vector2 myCoord = new Vector2(ball._rigidbody.velocity.x, ball._rigidbody.velocity.y);
            //Debug.Log(Mathf.Abs(Mathf.Atan2(normal.y-myCoord.y,normal.x-myCoord.x)*Mathf.Rad2Deg));
            //Debug.Log(Vector2.Angle(normal, myCoord));
            //ball.AddForce(-normal * this.bounceStrength);
            //ball._rigidbody.velocity=new Vector2(ball._rigidbody.velocity.x*1.2f, ball._rigidbody.velocity.y * 1.2f);

        }
    }
}
