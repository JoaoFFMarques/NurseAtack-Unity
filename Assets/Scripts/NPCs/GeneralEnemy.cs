using System.Collections;
using UnityEngine;

public abstract class GeneralEnemy : NpcAI
{//subclasse que controla os inimigos
    [Header("HitBox")]
    public Transform Hitter;
    public GameObject HitBoxPrefab;

    public float EnemyTotalLife;
    public bool IsLookLeft;

    public float _TimerToAtack;
    public float _TimeSet;
    public float _Time;
    public bool CanAtack;
    public bool IsDead;

    public void FixedUpdate()
    {//controle do movimento, do giro e do ataque do inimigo
        if(Gamecontroller.TotalLife > 0 && !IsDead && !Gamecontroller.GameEnd)
        {
            Flip();
            Move();
            if(CanAtack)//se o inimigo puder atacar ele vai fazer
            {
                EnemyAtack();
            }
            else
                TimerToAtack();//função que coloca um timer entre os ataques dos inimigos
            if(!IsWalking)
                StartCoroutine("Walk");
        }
    }
    public void OnTriggerEnter(Collider collision)
    {//controle de colisção com os ataques da personagem
        if(collision.CompareTag("HitBoxCollider") && !IsHit && !IsDead)
        {            
            IsHit = true;
            IsWalking = false;                
            switch(gameObject.tag)
            {
                case "Enemy"://se for um inimigo comum
                    EnemyTotalLife -= 1;
                    Gamecontroller.EnemyHeartController((int)EnemyTotalLife);
                    Gamecontroller.TimerEnemyBar = 3f;
                    break;
                case "Boss"://se for um chefe o dano é menor
                    EnemyTotalLife -= 0.5f;
                    Gamecontroller.BossHeartController(EnemyTotalLife);
                    break;
            }                
            Gamecontroller.PlaySFX(Gamecontroller.SfxDamageTaken);
            if(EnemyTotalLife <= 0)//verifica se a vida zerou para a animação de morte
            {
                IsDead = true;
                if(gameObject.CompareTag("Boss"))
                    Gamecontroller.PlaySFX(Gamecontroller.SfxBossDead);
                else
                    Gamecontroller.PlaySFX(Gamecontroller.SfxEnemyDead);
                NpcAnimator.SetTrigger("IsDead");
            }
            else//se a vida não zerou inicia a animação de ter tomado dano
            {
                NpcAnimator.SetTrigger("Hitted");
                IsHit = false;
            }
        }
        
        
    }    
    public void TimerToAtack()//contador para qdo será o próximo ataque
    {
        if(_TimerToAtack > 0)//se o contador não zerar não pode atacar ainda
        {
            CanAtack = false;
            _TimerToAtack -= _Time * Time.deltaTime;
        }
        else
        {
            CanAtack = true;//se e puder atacar o timer volta ao valor inicial
            _TimerToAtack = _TimeSet;
        }
    }
    public void EnemyAtack()
    {//controle do ataque d inimigo
        if((Gamecontroller.PlayerTransform.position.x - transform.position.x <= 5 && !IsLookLeft) ||
            (transform.position.x - Gamecontroller.PlayerTransform.position.x <= 5 && IsLookLeft))
        {
            Gamecontroller.PlaySFX(Gamecontroller.SfxAtack);
            NpcAnimator.SetTrigger("Atack");

            CanAtack = false;
        }
    }
    public void EnemyOnEndAtack()
    {//verifica o fim do ataque. chamada como evento na animação de ataque
        CanAtack = false;
    }
    public void EnemyHitBoxAtack()    
    {//instancia o hit box do ataque do inimgo, chamada como evento na propria animação
        GameObject hitBoxTemp = Instantiate(HitBoxPrefab, Hitter.position, transform.localRotation);
        hitBoxTemp.transform.SetParent(gameObject.transform);
        Destroy(hitBoxTemp, 0.2F);
    }
    public override void Move()
    {//faz a movimentação do inimigo
        var pos = Gamecontroller.PlayerTransform.position;
        if(IsFollow && IsWalking)
        {
            if((IsLookLeft && transform.position.x <= pos.x+0.5f) || 
                (!IsLookLeft && transform.position.x >= pos.x - 0.5f))
            {
                IsWalking = false;
            }
            else
                transform.position = Vector3.MoveTowards(transform.position, pos, Speed * Time.deltaTime);
            NpcAnimator.SetBool("IsWalking", IsWalking);
        }
    }
    IEnumerator Walk()
    {
        IsWalking = true;
        yield return new WaitForSeconds(1f*Time.fixedDeltaTime);
    }
    public override void Flip()
    {//controle de rotação
        IsLookLeft = Gamecontroller.PlayerTransform.position.x <= transform.position.x;
        if(IsLookLeft)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);
    }
    public abstract void OnDead();//classe abstrata para fazera  verioficação de morte
}
