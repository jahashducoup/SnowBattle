using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float speed = 10.0f;
    [System.NonSerialized]
    public Rigidbody2D _rigidbody;
    [System.NonSerialized]
    public Vector2 _basePos, _direction, _upDirection, _downDirection;
    [System.NonSerialized]
    public KeyCode upKey, downKey;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        ResetPos();


    }
    private void Update()
    {
        if (Input.GetKey(this.upKey))
        {
            _direction = this._upDirection;
        }
        else if (Input.GetKey(this.downKey))
        {
            _direction = this._downDirection;
        }
        else
        {
            _direction = Vector2.zero;
        }
    }
    private void FixedUpdate()
    {

        _rigidbody.velocity = _direction.normalized * this.speed;
    }
    public void ResetPos()
    {
        transform.position = this._basePos;
        //_rigidbody.position = this._basePos;
        _rigidbody.velocity = Vector2.zero;
    }
}
