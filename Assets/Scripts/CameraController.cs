using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector2 _turn;
    private const float Sensitivity = .9f;
    private const float Speed = 5f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        _turn.x += Input.GetAxis("Mouse X") * Sensitivity;
        _turn.y += Input.GetAxis("Mouse Y") * Sensitivity;
        player.transform.localRotation = Quaternion.Euler(0, _turn.x, 0);
        transform.localRotation = Quaternion.Euler(-_turn.y, 0, 0);

        var deltaMove = new Vector3(Input.GetAxisRaw("Horizontal") * Speed * Time.deltaTime, 0, Input.GetAxisRaw("Vertical") * Speed * Time.deltaTime);
        player.transform.Translate(deltaMove);
    }
}