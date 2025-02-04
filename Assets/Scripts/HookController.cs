using UnityEngine;
using System.Collections;

public class HookController : MonoBehaviour
{
    public Transform _player;
    public Transform _hookPoint;
    private Rigidbody _rigidbody;

    private Vector3 fastMoveStartPosition;
    private Vector3 fastMoveTargetPosition;
    private Camera _camera;
    private RaycastHit _hit;
    private LineRenderer _line;
    public LayerMask grapplingObj;

    public bool isGrappling = false;
    private Vector3 hitSpot;
    private SpringJoint _spring;
    public float fastMoveSpeed;

    public float dashForce = 20f;
    public float maxDashAmount = 100f;
    public float dashConsumption = 30f;
    public float dashRegenRate = 20f;
    public float currentDashAmount;

    void Start( )
    {
        _camera = Camera.main;
        _rigidbody = _player.GetComponent<Rigidbody>( );
        _line = GetComponent<LineRenderer>( );
        currentDashAmount = maxDashAmount;
    }

    void Update( )
    {
        if( Input.GetMouseButtonDown( 0 ) )
        {
            ShootHook( );
        }

        if( Input.GetMouseButtonUp( 0 ) )
        {
            EndShoot( );
        }

        if( Input.GetKey( KeyCode.LeftShift ) && currentDashAmount > 0 )
        {
            Dash( );
            currentDashAmount -= dashConsumption * Time.deltaTime;
        }
        else
        {
            RegenerateDash( );
        }

        currentDashAmount = Mathf.Clamp( currentDashAmount, 0f, maxDashAmount );
        IngameUIManager.instance.SetBoostGauge( currentDashAmount );

        float scrollWheel = Input.GetAxis( "Mouse ScrollWheel" );
        if ( isGrappling && Mathf.Abs( scrollWheel ) > 0f )
        {
            if ( scrollWheel < 0f )
            {
                fastMoveStartPosition = _player.position;
                fastMoveTargetPosition = _hit.point;
                StartCoroutine( FastMoveCoroutine( _player, fastMoveStartPosition, fastMoveTargetPosition, fastMoveSpeed ) );
            }
        }

        DrawHook( );
    }

    private void ShootHook( )
    {
        if( Physics.Raycast( _camera.transform.position, _camera.transform.forward, out _hit, 100f, grapplingObj ) )
        {
            isGrappling = true;
            hitSpot = _hit.point;
            
            _line.positionCount = 2;
            
            float distance = Vector3.Distance( transform.position, hitSpot );
            _spring = _player.gameObject.AddComponent<SpringJoint>( );
            _spring.autoConfigureConnectedAnchor = false;
            _spring.connectedAnchor = hitSpot;
            _spring.minDistance = distance * 0.3f;
            _spring.maxDistance = distance;
            _spring.spring = 5f;
            _spring.damper = 5f;
        }
    }

    private void DrawHook( )
    {
        if( isGrappling )
        {
            _line.SetPosition( 0, _hookPoint.position );
            _line.SetPosition( 1, hitSpot );
            transform.LookAt( hitSpot );
        }
    }

    private void EndShoot( )
    {
        isGrappling = false;
        _line.positionCount = 0;
        Destroy( _spring );
        transform.localRotation = Quaternion.identity;
    }

    private void Dash( )
    {
        _rigidbody.AddForce( _camera.transform.forward * dashForce, ForceMode.Force );
    }

    private void RegenerateDash( )
    {
        if( currentDashAmount < maxDashAmount )
        {
            currentDashAmount += dashRegenRate * Time.deltaTime;
        }
    }

    private IEnumerator FastMoveCoroutine( Transform target, Vector3 start, Vector3 end, float speed )
    {
        float t = 0f;
        while ( t < 0.85f )
        {
            t += Time.deltaTime * ( speed / Vector3.Distance( start, end ) );
            Vector3 newPosition = Vector3.Lerp( start, end, t );
            _rigidbody.linearVelocity = ( newPosition - _player.position ) * fastMoveSpeed;
            yield return null;
        }
    }

    public void AddFuel( float value )
    {
        currentDashAmount += value;
        if( currentDashAmount > maxDashAmount )
            currentDashAmount = maxDashAmount;
    }
}