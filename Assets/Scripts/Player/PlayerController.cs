using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Color SickColor;
    public float SickTime;
    private bool IsSick;
    [Header("Move")]
    public float MoveSpeed;
    public bool isLookLeft;
    private bool IsWalking;
    private bool IsAtack;
    private bool IsHit;
    private bool b_IsDead;
    private Vector3 Movement;

    [Header("Ground")]
    public float GroundDistance;
    public LayerMask GroundLayer;
    public Transform Feet;
    private bool IsGrounded;

    [Header("Jump")]
    public float JumpForce;
    public float JumpTime;
    private float JumpElapsedTime;
    private bool IsJumping;
    private float SpeedY;

    [Header("HitBox")]
    public Transform Hitter;
    public GameObject HitBoxPrefab;

    [Header("ThrowObject")]
    public GameObject MaskObj;
    public GameObject CallResc;

    private Animator Animating;
    private Rigidbody Body;
    private GameController Gamecontroller;
    private SpriteRenderer PlayerSR;

    void Start()
    {
        PlayerSR = GetComponent<SpriteRenderer>();
        Gamecontroller = FindObjectOfType(typeof(GameController)) as GameController;
        Body = GetComponent<Rigidbody>();
        Animating = GetComponent<Animator>();
        Gamecontroller.HeartController();
    }
    void Update()
    {
        Control();
        if(IsSick)
            Sicked();
        Animating.SetBool("IsWalking", IsWalking);
        Animating.SetFloat("SpeedY", SpeedY);
        Animating.SetBool("IsGrounded", IsGrounded);
        Animating.SetBool("IsAtack", IsAtack);
        Animating.SetBool("IsHit", IsHit);        
    }
    private void FixedUpdate()
    {
        if(Gamecontroller.TotalLife > 0 && !Gamecontroller.GameEnd)
        {
            Move();
            Jump();
            if(Input.GetAxis("Horizontal") > 0 && isLookLeft && !IsAtack)
                Rotate();
            else if(Input.GetAxis("Horizontal") < 0 && !isLookLeft && !IsAtack)
                Rotate();
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(!IsHit && !b_IsDead)
        {
            switch(collision.gameObject.tag)
            {
                case "EnemyHitBoxCollider":
                    if(Gamecontroller.TotalLife > 0)
                    {
                        Gamecontroller.TotalLife -= 1;
                        if(Gamecontroller.TotalLife <= 0)
                            IsDead();
                        else
                            OnHit();
                    }
                    break;
                case "BossHit":
                    if(Gamecontroller.TotalLife > 0)
                    {
                        Gamecontroller.TotalLife -= 1.25f;
                        if(Gamecontroller.TotalLife <= 0)
                            IsDead();
                        else
                            OnHit();
                    }
                    break;
                case "Sick":
                    Sick();
                    break;
            }
        }        
    }
    private void Control()
    {
        if(Gamecontroller.TotalLife > 0 && !Gamecontroller.GameEnd)
        {
            Movement.x = Input.GetAxis("Horizontal");
            Movement.z = Input.GetAxis("Vertical");
            IsGrounded = Physics.CheckSphere(Feet.position, GroundDistance, GroundLayer, QueryTriggerInteraction.Ignore);
            SpeedY = Body.velocity.y;            
            if(Input.GetButtonDown("Jump") && IsGrounded)
            {
                IsJumping = true;
                JumpElapsedTime = 0.0f;
            }
            if(Input.GetButtonDown("Fire1") && !IsAtack)
            {
                Gamecontroller.PlaySFX(Gamecontroller.SfxAtack);
                IsAtack = true;
                Animating.SetTrigger("Atack");           
            }
            if(Input.GetButtonDown("Fire2") && !IsAtack && IsGrounded)
            {
                IsAtack = true;
                Gamecontroller.PlaySFX(Gamecontroller.SfxAtack);
                Animating.SetTrigger("Shooting");
            }
            if(Input.GetButtonDown("Fire3") && !IsAtack && IsGrounded)
            {
                IsAtack = true;
                Gamecontroller.PlaySFX(Gamecontroller.SfxAtack);
                Animating.SetTrigger("CallResc");
            }
        }
    }
    private void Move()
    {
        if((Movement.x != 0.0 || Movement.z != 0.0))
            IsWalking = true;
        else
            IsWalking = false;

        if(!IsAtack && !IsHit)
            Body.MovePosition(Body.position + Movement * MoveSpeed * Time.fixedDeltaTime);
        else if(IsAtack && !IsGrounded && !IsHit)
            Body.MovePosition(Body.position + Movement * MoveSpeed * Time.fixedDeltaTime);
    }
    private void Rotate()
    {
        float x = transform.localScale.x * -1;

        isLookLeft = !isLookLeft;

        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
    private void Jump()
    {
        if(IsJumping && JumpElapsedTime > (JumpTime / 3))
            if(!Input.GetButton("Jump"))
                IsJumping = false;

        if(IsJumping && JumpElapsedTime < JumpTime)
        {
            JumpElapsedTime += Time.fixedDeltaTime;
            float proportionCompleted = Mathf.Clamp01(JumpElapsedTime / JumpTime);
            float currentForce = Mathf.Lerp(JumpForce, 0.0f, proportionCompleted);
            Body.AddForce(Vector3.up * currentForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
        else
            IsJumping = false;

    }
#pragma warning disable IDE0051 // Remover membros privados não utilizados
    public void HitBoxAtack()
    {
        GameObject hitBoxTemp = Instantiate(HitBoxPrefab, Hitter.position, Hitter.transform.localRotation);
        hitBoxTemp.transform.SetParent(gameObject.transform);
        Destroy(hitBoxTemp, 0.2f);
    }
    private void Sick()
    {
        IsSick = true;
        SickTime = 6f;
        gameObject.tag = "Sick";
        PlayerSR.color = SickColor;
        MoveSpeed = 3;
    }
    private void Sicked()
    {
        if(SickTime > 0)
        {
            SickTime -= 1f * Time.deltaTime;
        }
        else
        {
            IsSick = false;
            MoveSpeed = 6;
            Gamecontroller.TotalLife -= 1;
            Gamecontroller.HeartController();
            gameObject.tag = "Player";
            PlayerSR.color = Color.white;
        }
        if(Gamecontroller.TotalLife <= 0)
            IsDead();
    }
    private void ThrowMask()
    {
        GameObject mask = Instantiate(MaskObj, Hitter.position, transform.localRotation);
        mask.transform.SetParent(gameObject.transform.parent);        
    }
    private void CallRescue()
    {
        GameObject call= Instantiate(CallResc, Hitter.position, transform.localRotation);
        call.transform.SetParent(gameObject.transform.parent);       
    }
    private void OnHit()
    {
        IsHit = true;
        IsAtack = false;        
        Gamecontroller.HeartController();
        Gamecontroller.PlaySFX(Gamecontroller.SfxDamageTaken);        
    }
    private void IsDead()
    {
        if(!b_IsDead)
        {
            Gamecontroller.PlaySFX(Gamecontroller.SfxDamageTaken);
            Gamecontroller.HeartController();
            b_IsDead = true;
            Animating.SetBool("IsDead", b_IsDead);
        }
    }
    public void Ondead()
    {
        Gamecontroller.PlaySFX(Gamecontroller.SfxPlayerDead);
    }
    public void PlayerDead()
    {
        if(Gamecontroller.PlayerChances == 0)
            Gamecontroller.GameOver();
        else
        {
            Gamecontroller.Chances();
            Gamecontroller.TotalLife = 5;
            MoveSpeed = 10;
            Body.position = new Vector3(Body.position.x, 15f, Body.position.z);
            Gamecontroller.HeartController();
            IsHit = false;
            IsAtack = false;
            b_IsDead = false;
            Animating.SetBool("IsDead", false);
        }
    }
    public void PlayerHitted()
    {
        IsHit = false;
    }
#pragma warning disable IDE0051 // Remover membros privados não utilizados
    public void OnEndAtack()
#pragma warning restore IDE0051 // Remover membros privados não utilizados
    {
        IsAtack = false;
    }
}