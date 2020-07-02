using UnityEngine;

public class ItemThrow : MonoBehaviour
{
    public float Speed;
    private Rigidbody ObjRB;
    private PlayerController Player;
    private float horizontal;

    void Start()
    {
        Player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        ObjRB = GetComponent<Rigidbody>();
        horizontal = Player.isLookLeft ? -1 : 1;            
        ObjRB.velocity = new Vector2(horizontal*Speed, ObjRB.velocity.y);       
    }
    private void Update()
    {
        ObjRB.gameObject.transform.Rotate(0, 0, 5f*(-horizontal));
    }
    private void OnTriggerEnter(Collider collison)
    {
        switch(collison.tag)
        {
            case "Player":
            case "HitBoxCollider":
            case "Mask":
            case "CallResc":
            case "Generator":
                break;
            default:
                Destroy(this.gameObject);
                break;
        }                
    }
}
