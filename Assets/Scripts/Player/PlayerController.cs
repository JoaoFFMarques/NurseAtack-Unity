using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //controlador da personagem

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
        //input do controle de movimento e ação
        Control();
        if(IsSick)
            Sicked();
        //verificação das animações
        Animating.SetBool("IsWalking", IsWalking);
        Animating.SetFloat("SpeedY", SpeedY);
        Animating.SetBool("IsGrounded", IsGrounded);
        Animating.SetBool("IsAtack", IsAtack);
        Animating.SetBool("IsHit", IsHit);
    }
    private void FixedUpdate()
    {
        //funções para moevr, pular ou rotacionar a personagem
        if(Gamecontroller.TotalLife > 0 && !Gamecontroller.GameEnd)
        {
            Move();
            Jump();
            if(Movement.x > 0 && isLookLeft && !IsAtack)
                Rotate();
            else if(Movement.x < 0 && !isLookLeft && !IsAtack)
                Rotate();
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(!IsHit && !b_IsDead)//se a eprsonage já recebeu hit ela não receb outro
        {
            switch(collision.gameObject.tag)
            {
                case "EnemyHitBoxCollider":
                    if(Gamecontroller.TotalLife > 0)
                    {
                        Gamecontroller.TotalLife -= 1;
                        if(Gamecontroller.TotalLife <= 0)//verifica se a vida zerou
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
                case "Sick"://verifica se colidiu com alguem doente
                    Sick();
                    break;
            }
        }
    }
    private void Control()
    {
        if(Gamecontroller.TotalLife > 0 && !Gamecontroller.GameEnd)
        {//eixos de movimento
            Movement.x = SimpleInput.GetAxis("Horizontal");
            Movement.z = SimpleInput.GetAxis("Vertical");            

            IsGrounded = Physics.CheckSphere(Feet.position, GroundDistance, GroundLayer, QueryTriggerInteraction.Ignore);
            SpeedY = Body.velocity.y;
            //verifica o input dos botões, para funcionar com controles também
            if(SimpleInput.GetButtonDown("Jump"))
                Jumping();
            if(SimpleInput.GetButtonDown("Fire1"))
                Atacking();
            if(SimpleInput.GetButtonDown("Fire2"))
                Masking();
            if(SimpleInput.GetButtonDown("Fire3"))
                Rescuing();
        }
    }
    private void Move()
    {
        if((Movement.x != 0.0 || Movement.z != 0.0))
            IsWalking = true;
        else
            IsWalking = false;

        if(!IsAtack && !IsHit)//não eprmite se movimnetar se esta atacando ou se recebeu dano naquele instante
            Body.MovePosition(Body.position + Movement * MoveSpeed * Time.fixedDeltaTime);
        else if(IsAtack && !IsGrounded && !IsHit)
            Body.MovePosition(Body.position + Movement * MoveSpeed * Time.fixedDeltaTime);
    }
    private void Rotate()//gira a personagem
    {
        float x = transform.localScale.x * -1;

        isLookLeft = !isLookLeft;

        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
    private void Jump()
    {//cotrole do pulo, qto mais se pressionar o botão de pulo maior será a distancia de salto
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
    {//intancia na tela o hit box qdo é feito o chute. a função é chamada em um evento an animação
        GameObject hitBoxTemp = Instantiate(HitBoxPrefab, Hitter.position, Hitter.transform.localRotation);
        hitBoxTemp.transform.SetParent(gameObject.transform);
        Destroy(hitBoxTemp, 0.2f);
    }
    private void Sick()
    {//cotrole de quando se adoece
        IsSick = true;
        SickTime = 6f;
        gameObject.tag = "Sick";
        PlayerSR.color = SickColor;
        MoveSpeed = 3;
    }
    private void Sicked()
    {//efeito do a doença termina
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
    {//intsnacia a mascara na tela
        GameObject mask = Instantiate(MaskObj, Hitter.position, transform.localRotation);
        mask.transform.SetParent(gameObject.transform.parent);
    }
    private void CallRescue()
    {//instancia o alcool em gem
        GameObject call = Instantiate(CallResc, Hitter.position, transform.localRotation);
        call.transform.SetParent(gameObject.transform.parent);
    }
    private void OnHit()
    {//função que verifica qdo inicia a animação de tomar dano. 
        IsHit = true;
        IsAtack = false;
        Gamecontroller.HeartController();
        Gamecontroller.PlaySFX(Gamecontroller.SfxDamageTaken);
    }
    private void IsDead()
    {//função de controle qdo a personagem morre
        if(!b_IsDead)
        {
            Gamecontroller.PlaySFX(Gamecontroller.SfxDamageTaken);
            Gamecontroller.HeartController();
            b_IsDead = true;
            Animating.SetBool("IsDead", b_IsDead);
        }
    }
    public void Ondead()
    {//som de morte chamada na animação de morte
        Gamecontroller.PlaySFX(Gamecontroller.SfxPlayerDead);
    }
    public void PlayerDead()
    {//função chamada caso a vida chegue a zero para ver se ela ressucita ou finaliza o jogo
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
    {//função que finaliza o efeito de tomar dano ela é chamada como evento na propria animação de tomar dano
        IsHit = false;
    }
#pragma warning disable IDE0051 // Remover membros privados não utilizados
    public void OnEndAtack()
#pragma warning restore IDE0051 // Remover membros privados não utilizados
    {//verifca qdo finaliza o ataqeu, chamada na propria animação de ataque ou atirar mascar/gel
        IsAtack = false;
    }   
    public void Jumping()
    {//função de pulo chamada na função Control()
        if(IsGrounded)
        {
            IsJumping = true;
            JumpElapsedTime = 0.0f;
        }
    }
    public void Atacking()
    {//função de ataque chamada na função Control()
        if(!IsAtack)
        {
            Gamecontroller.PlaySFX(Gamecontroller.SfxAtack);
            IsAtack = true;
            Animating.SetTrigger("Atack");
        }
    }
    public void Rescuing()
    {//função de atirar alcool chamada na função Control()
        if(!IsAtack && IsGrounded)
        {
            IsAtack = true;
            Gamecontroller.PlaySFX(Gamecontroller.SfxAtack);
            Animating.SetTrigger("CallResc");
        }
    }
    public void Masking()
    {//função de atirar mascar chamada na função Control()
        if(!IsAtack && IsGrounded)
        {
            IsAtack = true;
            Gamecontroller.PlaySFX(Gamecontroller.SfxAtack);
            Animating.SetTrigger("Shooting");
        }
    }

}