using System.Collections;
using UnityEngine;
//script de comportamento das pessoas
public class PersonNPC : NpcAI
{
    public bool IsLookLeft;

    public bool _Start;
    
    private bool SickStart;
    public Color SickColor;
    public GameObject HitBox;
    private bool Masked = false;
    private bool Contaminated;
    private bool IsRescued;
    private float Horizontal;
    private float Vertical;
    private float TimeToWalk = 3.0f;    
    private SpriteRenderer PersonSR;

    new private void Start()
    {
        base.Start();
        //implementação da base e das particularidades
        PersonSR = GetComponent<SpriteRenderer>();
        StartCoroutine("Walking");
        SickStart = Random.Range(0, 100) <= 25;//verifica quem será gerado já contaminado
        if(SickStart)
            Sick();
    }
    private void FixedUpdate()
    {
        if(!Gamecontroller.GameEnd)
        {//controle de rotação e movimento do eprsonagem
            if((Horizontal > 0 && IsLookLeft) || (Horizontal < 0 && !IsLookLeft))
                Flip();
            Move();
        }
    }
    private void OnTriggerEnter(Collider collison)
    {
        switch(collison.gameObject.tag)
        {
            case "Mask"://qdo a amscara atinge o objeto
                if(!Contaminated && !Masked && !IsRescued)
                { 
                    _Start = false;
                    Masked = true;
                    Gamecontroller.Maskeds();
                    StopCoroutine("Walking");
                    MoveOut();
                }
                break;
            case "Despawner"://retirar o objeto da cena
                if(!_Start)
                {
                    Destroy(this.gameObject);
                }
                break;
            case "Sick"://verificar se ficou doente
                if(!Masked && !IsRescued)
                    Sick();
                break;
            case "CallResc"://qdo atingido pelo alcool em gel evrifica se esta contaminado
                if(Contaminated && !IsRescued)
                {
                    _Start = false;
                    Contaminated = false;
                    IsRescued = true;
                    HitBox.tag = "Person";
                    Gamecontroller.Rescueds();
                    StopCoroutine("Walking");
                    SickOut();
                }
                break;
        }        
    }
    private void MoveOut()
    {//função que movimenta o objeto rpa fora da tela
        int Rand = Random.Range(0, 50);

        if(Rand < 25)
            Horizontal = -1;
        else
            Horizontal = 1;
        Speed = 10;
        Vertical = 0.0f;
        this.gameObject.layer = LayerMask.NameToLayer("MovingOut");

        NpcAnimator.SetBool("Mask", Masked);
    }
    private void Sick()
    {//função para controlar os efeitos da doença
        Contaminated = true;        
        HitBox.tag = "Sick";//permite proliferar a doença
        PersonSR.color = SickColor;
        Speed = 3;
        TimeToWalk = 5.0f;
    }
    private void SickOut()
    {//retirar o objeto contaminado e chamando a animação própria
        PersonSR.color = Color.white;
        
        Speed = 20;
        
        int Rand = Random.Range(0, 50);

        if(Rand < 25)
            Horizontal = -1;
        else
            Horizontal = 1;
       
        Vertical = 0.0f;
        this.gameObject.layer = LayerMask.NameToLayer("MovingOut");
        Gamecontroller.PlaySFX(Gamecontroller.SfxRescue);
        NpcAnimator.SetBool("IsRescued", IsRescued);
    }    
    public override void Flip()
    {//função que gira o objeto an direção para onde se movimentar
        IsLookLeft = !IsLookLeft;
        
        if(IsLookLeft)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);
    }
    public override void Move()
    {//faz o controle de movimento
        NpcRB.velocity = new Vector3(Horizontal * Speed, NpcRB.velocity.y, Vertical * Speed);
        if(!IsRescued)
        {
            if(Horizontal != 0 || Vertical != 0)
                NpcAnimator.SetBool("IsWalk", true);
            else
                NpcAnimator.SetBool("IsWalk", false);
        }
    }
    private int RandMov()
    {//gera um valor aleatório para detemrinar a direção do movimento do objeto
        int Rand = Random.Range(0, 100);

        if(Rand < 33)
            return -1;
        else if(Rand < 66)
            return 0;
        else
            return 1;
    }
    IEnumerator Walking()
    {
        Horizontal = RandMov();
        
        if(transform.position.z > -3.5 || transform.position.z < 7)
            Vertical = RandMov();        
        else
            Vertical = 0;

        yield return new WaitForSeconds(TimeToWalk);
        StartCoroutine("Walking");
    }   

}
