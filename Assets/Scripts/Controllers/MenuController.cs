using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{    
    [Header("Audio")]
    public AudioSource SfxSource;

    [Header("SFX")]
    public AudioClip SfxMenu;

    [Header("General UI")]
    public GameState CurrentState;
    public GameObject TitlePanel;
    public GameObject IntroPanel_1;
    public GameObject IntroPanel_2;
    public GameObject HowPlayPanel_1;
    public GameObject HowPlayPanel_2;
    public GameObject ControlPanel;
    public GameObject CreditPanel;
    public GameObject RankPanel;
    private bool inputChanged;
    private float TimerInput = 1f;

    private void Update()
    {       
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
    {
        SfxSource.PlayOneShot(sfxClip, 0.9f);
    }
    public void GameIntro_1()
    {
        PlaySFX(SfxMenu);
        CurrentState = GameState.INTRO_1;
        TitlePanel.SetActive(false);
        IntroPanel_1.SetActive(true);
    }
    public void GameIntro_2()
    {
        WaitForInput();
        if(Input.anyKeyDown && inputChanged)
        {
            PlaySFX(SfxMenu);
            CurrentState = GameState.INTRO_2;
            IntroPanel_1.SetActive(false);
            IntroPanel_2.SetActive(true);
        }
    }
    public void HowtoPlay_1()
    {
        PlaySFX(SfxMenu);
        CurrentState = GameState.HOWPLAY_1;
        TitlePanel.SetActive(false);
        HowPlayPanel_1.SetActive(true);
    }
    public void HowtoPlay_2()
    {
       WaitForInput();
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
    {
        HowPlayPanel_2.SetActive(false);
        MenuReturn();
    }    
    public void GameStart()
    {
        if(Input.anyKeyDown)
        {
            SceneManager.LoadScene("Stage_One");
        }
    }
    public void GameQuit()
    {
        PlaySFX(SfxMenu);
        Application.Quit();
    }
    public void MenuControl()
    {
        CurrentState = GameState.CONTROL;
        ControlPanel.SetActive(true);

        MenuOut();
    }
    public void ControlMenuReturn()
    {        
        ControlPanel.SetActive(false);
        MenuReturn();
    }
    public void MenuRank()
    {
        CurrentState = GameState.RANK;
        RankPanel.SetActive(true);

        MenuOut();
    }
    public void RankMenuReturn()
    {       
        RankPanel.SetActive(false);
        MenuReturn();
    }
    public void MenuCredit()
    {
        CurrentState = GameState.CREDIT;
        CreditPanel.SetActive(true);

        MenuOut();
    }
    public void CreditMenuReturn()
    {        
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
    {
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
