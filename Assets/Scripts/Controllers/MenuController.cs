using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;//controlador do menu inicial do jogo

public class MenuController : MonoBehaviour
{
    [Header("Audio")]
    public GameObject MusicSource;
    public GameObject SfxSource;
    public AudioClip MusicMenu;

    [Header("SFX")]
    public AudioClip SfxMenu;

    [Header("General UI")]//recebe todos os paineis do canvas
    public GameState CurrentState;//recebe p estado do jogo
    public GameObject TitlePanel;
    public GameObject IntroPanel_1;
    public GameObject IntroPanel_2;
    public GameObject HowPlayPanel_1;
    public GameObject HowPlayPanel_2;
    public GameObject ControlPanel;
    public GameObject OptionsPanel;
    public GameObject CreditPanel;
    public GameObject RankPanel;
    private bool inputChanged;
    private float TimerInput = 1f;

    private void Start()
    {
        MusicSource = GameObject.FindWithTag("MusicSource");
        SfxSource = GameObject.FindWithTag("SFXSource");
        MusicSource.GetComponent<AudioSource>().clip = MusicMenu;
        MusicSource.GetComponent<AudioSource>().Play();
        MusicSource.GetComponent<AudioSource>().loop = true;
    }
    private void Update()
    {       //verifica o estado do jogo para fazera  sequencia de ativação de paineis/inicializaçção do jogo principal
        switch(CurrentState)
        {            
            case GameState.INTRO_1:
                GameIntro_2();
                break;
            case GameState.INTRO_2:
                GameStart();
                break;
            case GameState.HOWPLAY_1:
                    HowtoPlay_2();
                break;
        }       
    }    
    public void PlaySFX(AudioClip sfxClip)
    {//funçãod e tocar efeitos sonoros
        SfxSource.GetComponent<AudioSource>().PlayOneShot(sfxClip, 0.9f);
    }
    public void GameIntro_1()
    {//primeiro painel, chamado por botão
        PlaySFX(SfxMenu);
        CurrentState = GameState.INTRO_1;
        TitlePanel.SetActive(false);
        IntroPanel_1.SetActive(true);
    }
    public void GameIntro_2()
    {//paibnel chamdo no update dependo do estado do jogo
        WaitForInput();//função com um contador de tempo para não pular  direto para a tela qdo usado via joystick
        if(Input.anyKeyDown && inputChanged)
        {
            PlaySFX(SfxMenu);
            CurrentState = GameState.INTRO_2;
            IntroPanel_1.SetActive(false);
            IntroPanel_2.SetActive(true);
        }
    }
    public void HowtoPlay_1()
    {//painel chamado por botão
        PlaySFX(SfxMenu);
        CurrentState = GameState.HOWPLAY_1;
        TitlePanel.SetActive(false);
        HowPlayPanel_1.SetActive(true);
    }
    public void HowtoPlay_2()
    {//paibnel chamdo no update dependo do estado do jogo
        WaitForInput();//função com um contador de tempo para não pular  direto para a tela qdo usado via joystick
        if(Input.anyKey && inputChanged)
        {            
            PlaySFX(SfxMenu);
            CurrentState = GameState.HOWPLAY_2;            
            HowPlayPanel_1.SetActive(false);
            HowPlayPanel_2.SetActive(true);
            GameObject first = GameObject.Find("Return");
            EventSystem _event = EventSystem.current.GetComponent<EventSystem>();
            _event.firstSelectedGameObject = first;
            _event.SetSelectedGameObject(first);            
        }
    }
    public void HowtoPlayReturn()
    {//volta para a tela principal
        HowPlayPanel_2.SetActive(false);
        MenuReturn();
    }    
    public void GameStart()
    {//inciar o jogo
        if(Input.anyKeyDown)
        {
            SceneManager.LoadScene("Stage_One");
        }
    }
    public void GameQuit()
    {//sair do jogo
        PlaySFX(SfxMenu);
        Application.Quit();
    }
    public void MenuControl()
    {//chama o painel por botão
        CurrentState = GameState.CONTROL;
        ControlPanel.SetActive(true);

        MenuOut();
    }
    public void ControlMenuReturn()
    {      //volta para a tela principal  
        ControlPanel.SetActive(false);
        MenuReturn();//função de chamara a tela principal
    }
    public void MenuOptions()
    {//chama o painel por botão
        CurrentState = GameState.OPTIONS;
        OptionsPanel.SetActive(true);

        MenuOut();
    }
    public void OptionsMenuReturn()
    {//volta para a tela principal
        OptionsPanel.SetActive(false);
        MenuReturn();
    }
    public void MenuRank()
    {//chama o painel por botão
        CurrentState = GameState.RANK;
        RankPanel.SetActive(true);

        MenuOut();
    }
    public void RankMenuReturn()
    {       //volta para a tela principal
        RankPanel.SetActive(false);
        MenuReturn();
    }
    public void MenuCredit()
    {//chama o painel por botão
        CurrentState = GameState.CREDIT;
        CreditPanel.SetActive(true);

        MenuOut();
    }
    public void CreditMenuReturn()
    {        //volta para a tela principal
        CreditPanel.SetActive(false);
        MenuReturn();
    }
    private void MenuReturn()
    {
        PlaySFX(SfxMenu);
        TitlePanel.SetActive(true);
        CurrentState = GameState.MENU;       
        GameObject first = GameObject.Find("Start");
        EventSystem _event = EventSystem.current.GetComponent<EventSystem>();
        _event.firstSelectedGameObject = first;       
        _event.SetSelectedGameObject(first);
        TimerInput = 1f;
        inputChanged = false;
    }
    private void MenuOut()
    {
        PlaySFX(SfxMenu);
        TitlePanel.SetActive(false);
        GameObject first = GameObject.Find("Return");
        EventSystem _event = EventSystem.current.GetComponent<EventSystem>();
        _event.firstSelectedGameObject = first;       
        _event.SetSelectedGameObject(first);        
    }
    private void WaitForInput()
    {//contador de tempo para agauradar e não pular tela
        if(TimerInput > 0)
        {
            TimerInput -= 1f * Time.deltaTime;
        }
        else
        {
           inputChanged = true;
        }
    }
}
