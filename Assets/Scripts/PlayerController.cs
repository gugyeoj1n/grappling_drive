using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [Header( "Rotate Variables" )]
    public float mouseSpeed;
    private float xRot;
    private float yRot;
    private Camera _camera;

    [Header( "Move Variables" )]
    public float moveSpeed;
    public Vector2 inputValue;
    public Vector3 moveValue;

    void Start( )
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _rigidbody = GetComponent<Rigidbody>( );
        _rigidbody.freezeRotation = true;

        _camera = Camera.main;
    }

    void Update( )
    {
        Rotate( );
        Move( );
    }

    private void OnMove( InputValue value )
    {
        inputValue = value.Get<Vector2>( );
    }

    private void Rotate( )
    {
        float mouseX = Input.GetAxisRaw( "Mouse X" ) * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxisRaw( "Mouse Y" ) * mouseSpeed * Time.deltaTime;

        yRot += mouseX;
        xRot -= mouseY;
        xRot = Mathf.Clamp( xRot, -90f, 90f );

        _camera.transform.rotation = Quaternion.Euler( xRot, yRot, 0 );
        transform.rotation = Quaternion.Euler( 0, yRot, 0 );
    }

    private void Move( )
    {
        moveValue = transform.forward * inputValue.y + transform.right * inputValue.x;
        transform.position += moveValue.normalized * moveSpeed * Time.deltaTime;
    }
}