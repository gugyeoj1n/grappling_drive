using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [Header("Rotate Variables")]
    public float mouseSpeed;
    private float xRot;
    private float yRot;
    private Camera _camera;

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
}