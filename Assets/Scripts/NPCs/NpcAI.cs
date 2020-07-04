using UnityEngine;

public abstract class NpcAI : MonoBehaviour
{ //classe abstrata para montagem do comportamento dos npcs (pessoas, inimigos e chefe)
    public GameController Gamecontroller { get; protected set; }
    public Rigidbody NpcRB { get; protected set; }
    public Animator NpcAnimator { get; protected set; }
    public Vector3 NpcMovement { get; protected set; }
    public bool IsWalking { get; protected set; }
    public bool IsHit { get; protected set; }
    public bool IsFollow;
    public float Speed;

    public void Start()
    {
        Gamecontroller = FindObjectOfType(typeof(GameController)) as GameController;
        NpcRB = GetComponent<Rigidbody>();
        NpcAnimator = GetComponent<Animator>();
        IsWalking = true;
    }    
    public void Hitted()
    {
        IsWalking = true;
    }
    //funções abstratas que deverão ser implementadas pelas subclasses
    public abstract void Flip();    
    public abstract void Move();
}
