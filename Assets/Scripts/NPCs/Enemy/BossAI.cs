
public class BossAI : GeneralEnemy
{
    private new void Start()
    {
        base.Start();        
        _Time = 0.4f;
        _TimeSet = 0.8f;
        _TimerToAtack = _TimeSet;
    }
    private void Update()
    {
        if((gameObject.transform.position.x - Gamecontroller.PlayerTransform.position.x < 30) && !IsFollow)
        {
            Gamecontroller.PlaySFX(Gamecontroller.SfxBossAwake);
            IsFollow = true;
            Gamecontroller.BossBar.SetActive(true);
        }
    }
    public override void OnDead()
    {        
        Destroy(this.gameObject);
        Gamecontroller.StageClear();
    }    
}