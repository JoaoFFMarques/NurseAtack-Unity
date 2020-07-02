
public class EnemyNPC : GeneralEnemy
{
    private new void Start()
    {
        base.Start();
        _Time = 0.4f;
        _TimeSet = 1.2f;
        _TimerToAtack = _TimeSet;
    }
    private void OnBecameVisible()
    {
        IsFollow = true;
    }    
    public override void OnDead()
    {
        Destroy(this.gameObject);
    }
    
}