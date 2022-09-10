using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody _rigidBody;
    private bool _isGrounded;
    private bool _applyForce;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void OnCollisionStay() => _isGrounded = true;
    void OnCollisionExit() => _isGrounded = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _applyForce = true;
    }

    void FixedUpdate()
    {
        // float moveHorizontal = Input.GetAxis("Horizontal");
        // float moveVertical = Input.GetAxis("Vertical");
        //
        // var force = new Vector3(moveHorizontal, _applyForce && _isGrounded ? 20 : 0, moveVertical);
        // _applyForce = false;
        //
        // _rigidBody.AddForce(force * speed);
    }
}