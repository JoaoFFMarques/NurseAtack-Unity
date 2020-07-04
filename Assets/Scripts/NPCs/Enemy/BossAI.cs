
public class BossAI : GeneralEnemy
{//implementação da subclasse d einimigo generco em um Chefe de fase
    private new void Start()
    {
        base.Start();        
        _Time = 0.4f;
        _TimeSet = 0.8f;//o timer é menor
        _TimerToAtack = _TimeSet;
    }
    private void Update()
    {
        if((gameObject.transform.position.x - Gamecontroller.PlayerTransform.position.x < 30) && !IsFollow)
        {//alem de iniciar a perseguição ao heroi também toca uma udio próprio
            Gamecontroller.PlaySFX(Gamecontroller.SfxBossAwake);
            IsFollow = true;
            Gamecontroller.BossBar.SetActive(true);//ativa a barra de vida especial de chefe
        }
    }
    public override void OnDead()
    {    //destroi o objeto e inicia o fim de partida
        Destroy(this.gameObject);
        Gamecontroller.StageClear();
    }    
}