using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    public Transform GameWorld;
    public Transform PlayerTransform;
    public bool Started;    
    public bool GameEnd;

    [Header("Audio")]    
    public AudioSource MusicSource;
    public AudioSource SfxSource;

    [Header("SFX")]
    public AudioClip SfxAtack;
    public AudioClip SfxEnemyDead;
    public AudioClip SfxPlayerDead;
    public AudioClip SfxDamageTaken;
    public AudioClip SfxRescue;
    public AudioClip SfxMenu;
    public AudioClip SfxBossAwake;
    public AudioClip SfxBossDead;

    [Header("Music")]
    public AudioClip MusicGAME;
    public AudioClip MusicSTAGECLEAR;
    public AudioClip MusicGAMEOVER;
    public AudioClip MusicBOSS;

    [Header("General UI")]
    public GameState CurrentState;    
    public GameObject GamePanel;
    public GameObject PausePanel;
    public GameObject GameOverPanel;
    public GameObject StageClearPanel;
    public GameObject PointsPanel;
    public GameObject Go;

    [Header("Points UI")]
    private int MaskedPts;
    public Text MaskedPtsQtdTXT;
    public Text MaskedPtsTXT;
    private int RescuedPts;
    public Text RescuedQtdPtsTXT;
    public Text RescuedPtsTXT;
    public Text TotalPtsTXT;
    public string namePoints;
    public int TotalPoints;
    public GameObject Buttons;
    public GameObject NameField;


    [Header("Player UI")]
    public Image[] LifeImg;
    private float MaxLife;
    public float TotalLife;
    public int PlayerChances;
    public Text ChancesTXT;
    private int MaskedNpcs;
    public Text MaskedTXT;
    private int RescuedNpcs;
    public Text RescuedTXT;

    [Header("Enemy UI")]
    public GameObject EnemyBar;
    public Image[] EnemyLifeImg;
    public float TimerEnemyBar = 4f;

    [Header("Boss UI")]
    public GameObject BossBar;
    public Image[] BossLifeImg;    

    private void Start()
    {
        Cursor.visible=false;
        ChancesTXT.text = PlayerChances.ToString();
        CurrentState = GameState.GAMEPLAY;
        Started = true;
        MaxLife = TotalLife;
    }
    private void Update()
    {       

        switch(CurrentState)
        {
            case GameState.GAMEPLAY:
                if(Input.GetButtonDown("Submit"))
                    PauseScr();
                break;
            case GameState.GAMEOVER:
            case GameState.STAGECLEAR:
                if(Input.anyKey)
                   PointsScr();
               break;
            case GameState.POINTS:
                PointCounter();                
                CurrentState = GameState.END;
                break;

        }
    }
    private void FixedUpdate()
    {
        EnemyBarVanish();
    }
    public void PlaySFX(AudioClip sfxClip)
    {
        SfxSource.PlayOneShot(sfxClip, 0.9f);
    }    
    public void BossHeartController(float EnemyTotalLife)
    {
        foreach(Image h in BossLifeImg)
            h.enabled = false;

        for(int i = 0; i < EnemyTotalLife; i++)
            BossLifeImg[i].enabled = true;
    }
    public void Chances()
    {
        PlayerChances -= 1;
        ChancesTXT.text = PlayerChances.ToString();
    }
    public void EnemyHeartController(int EnemyTotalLife)
    {
        EnemyBar.SetActive(true);
        foreach(Image h in EnemyLifeImg)
            h.enabled = false;

        for(int i = 0; i < EnemyTotalLife; i++)
            EnemyLifeImg[i].enabled = true;
    }
    public void EnemyBarVanish()
    {
        if(TimerEnemyBar > 0)
        {
            TimerEnemyBar -= 1f * Time.deltaTime;
        }
        else
        {
            EnemyBar.SetActive(false);
        }
    }
    public void HeartController()
    {
        foreach(Image h in LifeImg)
            h.enabled = false;

        for(int i = 0; i < TotalLife; i++)
            LifeImg[i].enabled = true;
    }
    public void Maskeds()
    {
        MaskedNpcs += 1;
        LifeRecover(MaskedNpcs/2);
        MaskedTXT.text = MaskedNpcs.ToString();
    }
    public void Rescueds()
    {
        RescuedNpcs += 1;
        LifeRecover(RescuedNpcs);
        RescuedTXT.text = RescuedNpcs.ToString();
    }
    public void LifeRecover(int personSvd)
    {
        if(personSvd % 4 == 0 && TotalLife<MaxLife)
        {
            TotalLife += 1;
            HeartController();
        }        
    }
    public void StageGoBar()
    {
        Go.SetActive(true);
    }
    public void PauseScr()
    {
        PlaySFX(SfxMenu);
        CurrentState = GameState.PAUSE;
        Cursor.visible = true;
        GameEnd = true;
        GameWorld.gameObject.SetActive(false);
        GamePanel.SetActive(false);
        PausePanel.SetActive(true);
        GameOut();
    }
    public void PauseRetrn()
    {
        PlaySFX(SfxMenu);
        CurrentState = GameState.GAMEPLAY;
        Cursor.visible = false;
        GameEnd = false;
        GameWorld.gameObject.SetActive(true);
        GamePanel.SetActive(true);
        PausePanel.SetActive(false);
    }
    public void StageClear()
    {
        Cursor.visible = true;
        GameEnd = true;
        CurrentState = GameState.STAGECLEAR;
        GamePanel.SetActive(false);        
        StageClearPanel.SetActive(true);
        MusicSource.Stop();
        MusicSource.PlayOneShot(MusicSTAGECLEAR, 0.7f);        
    }    
    public void GameOver()
    {
        Cursor.visible = true;
        Started = false;
        GamePanel.SetActive(false);
        GameOverPanel.SetActive(true);        
        PlayerTransform.gameObject.SetActive(false);
        CurrentState = GameState.GAMEOVER;
        MusicSource.Stop();
        MusicSource.PlayOneShot(MusicGAMEOVER, 0.7f);       
    }
    private void PointsScr()
    {
        PlaySFX(SfxMenu);
        if(CurrentState == GameState.GAMEOVER)
            GameOverPanel.SetActive(false);
        else
            StageClearPanel.SetActive(false);

        CurrentState = GameState.POINTS;
        PointsPanel.SetActive(true);
    }
    private void PointCounter()
    {
        MaskedPts = MaskedNpcs * 5;
        RescuedPts = RescuedNpcs * 3;
        MaskedPtsQtdTXT.text = MaskedNpcs.ToString();
        RescuedQtdPtsTXT.text = RescuedNpcs.ToString();
        MaskedPtsTXT.text = MaskedPts.ToString();
        RescuedPtsTXT.text = RescuedPts.ToString();
        TotalPtsTXT.text = (MaskedPts+RescuedPts).ToString();

        TotalPoints = MaskedPts + RescuedPts;         
    }
    public void GetName(string getname)
    {
        namePoints = getname;
    }
    public void EndNameInput()
    {
        if(Input.GetButtonDown("Submit"))
        {
            Buttons.SetActive(true);
            GameFinish();
            NameField.SetActive(false);
            AddScoreEntry(TotalPoints, namePoints);
        }
    }
    public void AddScoreEntry(int score, string name)
    {
        HighScore entry = new HighScore { Score = score, Name = name };

        BinaryFormatter bf = new BinaryFormatter();


        if(File.Exists(Application.persistentDataPath + "/ScoreData.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/ScoreData.dat", FileMode.Open);

            HighScoreList scoreList = (HighScoreList)bf.Deserialize(file);
            file.Close();
            scoreList.ScoreEntryList.Add(entry);

            file = File.Create(Application.persistentDataPath + "/ScoreData.dat");
            bf.Serialize(file, scoreList);
            file.Close();
        }
        else
        {
            HighScoreList scoreList = new HighScoreList
            {
                ScoreEntryList = new List<HighScore>()
            };
            scoreList.ScoreEntryList.Add(entry);
            FileStream file = File.Create(Application.persistentDataPath + "/ScoreData.dat");
            bf.Serialize(file, scoreList);
            file.Close();
        }        
    }
    public void GameRestart()
    {
        PlaySFX(SfxMenu);
        Started = false;
        SceneManager.LoadScene("Menu");
    }
    public void GameQuit()
    {
        PlaySFX(SfxMenu);
        Application.Quit();
    }    
    private void GameOut()
    {        
        GameObject first = GameObject.Find("Back");
        EventSystem _event = EventSystem.current.GetComponent<EventSystem>();
        _event.firstSelectedGameObject = first;
        _event.SetSelectedGameObject(first);
    }
    private void GameFinish()
    {
        PlaySFX(SfxMenu);
        GameObject first = GameObject.Find("Restart");
        EventSystem _event = EventSystem.current.GetComponent<EventSystem>();
        _event.firstSelectedGameObject = first;
        _event.SetSelectedGameObject(first);
    }
}
