
public class EnemyNPC : GeneralEnemy
{
    //implemnetação da subclasse de inimigo generico
    private new void Start()
    {
        base.Start();
        _Time = 0.4f;
        _TimeSet = 1.2f;
        _TimerToAtack = _TimeSet;
    }
    private void OnBecameVisible()
    {//verificaçãos e ele já pode ir em direção ao personagem
        IsFollow = true;
    }    
    public override void OnDead()
    {//efeito da morte
        Destroy(this.gameObject);
    }
    
}