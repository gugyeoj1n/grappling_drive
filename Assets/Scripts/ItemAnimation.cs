using UnityEngine;

public class ItemAnimation : MonoBehaviour
{
    void Update( )
    {
        transform.Rotate( Vector3.up * 20f * Time.deltaTime );
    }
}