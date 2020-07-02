using System.Collections;
using UnityEngine;

public abstract class GeneralEnemy : NpcAI
{
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
    {
        if(Gamecontroller.TotalLife > 0 && !IsDead && !Gamecontroller.GameEnd)
        {
            Flip();
            Move();
            if(CanAtack)
            {
                EnemyAtack();
            }
            else
                TimerToAtack();
            if(!IsWalking)
                StartCoroutine("Walk");
        }
    }
    public void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("HitBoxCollider") && !IsHit && !IsDead)
        {            
            IsHit = true;
            IsWalking = false;                
            switch(gameObject.tag)
            {
                case "Enemy":
                    EnemyTotalLife -= 1;
                    Gamecontroller.EnemyHeartController((int)EnemyTotalLife);
                    Gamecontroller.TimerEnemyBar = 3f;
                    break;
                case "Boss":
                    EnemyTotalLife -= 0.5f;
                    Gamecontroller.BossHeartController(EnemyTotalLife);
                    break;
            }                
            Gamecontroller.PlaySFX(Gamecontroller.SfxDamageTaken);
            if(EnemyTotalLife <= 0)
            {
                IsDead = true;
                if(gameObject.CompareTag("Boss"))
                    Gamecontroller.PlaySFX(Gamecontroller.SfxBossDead);
                else
                    Gamecontroller.PlaySFX(Gamecontroller.SfxEnemyDead);
                NpcAnimator.SetTrigger("IsDead");
            }
            else
            {
                NpcAnimator.SetTrigger("Hitted");
                IsHit = false;
            }
        }
        
        
    }    
    public void TimerToAtack()
    {
        if(_TimerToAtack > 0)
        {
            CanAtack = false;
            _TimerToAtack -= _Time * Time.deltaTime;
        }
        else
        {
            CanAtack = true;
            _TimerToAtack = _TimeSet;
        }
    }
    public void EnemyAtack()
    {
        if((Gamecontroller.PlayerTransform.position.x - transform.position.x <= 5 && !IsLookLeft) ||
            (transform.position.x - Gamecontroller.PlayerTransform.position.x <= 5 && IsLookLeft))
        {
            Gamecontroller.PlaySFX(Gamecontroller.SfxAtack);
            NpcAnimator.SetTrigger("Atack");

            CanAtack = false;
        }
    }
    public void EnemyOnEndAtack()
#pragma warning restore IDE0051 // Remover membros privados não utilizados
    {
        CanAtack = false;
    }
    public void EnemyHitBoxAtack()
#pragma warning disable IDE0051 // Remover membros privados não utilizados    
    {
        GameObject hitBoxTemp = Instantiate(HitBoxPrefab, Hitter.position, transform.localRotation);
        hitBoxTemp.transform.SetParent(gameObject.transform);
        Destroy(hitBoxTemp, 0.2F);
    }
    public override void Move()
    {
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
    {
        IsLookLeft = Gamecontroller.PlayerTransform.position.x <= transform.position.x;
        if(IsLookLeft)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);
    }
    public abstract void OnDead();
}
