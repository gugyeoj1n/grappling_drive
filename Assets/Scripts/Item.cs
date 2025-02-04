using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Fuel };
    public Type type;

    void OnTriggerEnter( Collider other )
    {
        if( other.tag == "Player" )
        {
            switch( type )
            {
                case Type.Fuel :
                    other.transform.GetChild(0).GetChild(0).GetComponent<HookController>().AddFuel( 50f );
                    Destroy(gameObject);
                    break;
                default :
                    break;
            }
        }
    }
}