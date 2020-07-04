using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.EventSystems;
//controlador do jogo
public class GameController : MonoBehaviour
{
    public Transform GameWorld;
    public Transform PlayerTransform;
    public bool Started;    //determina o etsado do jogo inciado
    public bool GameEnd;//determia o finald e jogo

    [Header("Audio")]    
    public GameObject MusicSource;
    public GameObject SfxSource;

    [Header("SFX")]
    public AudioClip SfxAtack;
    public AudioClip SfxEnemyDead;
    public AudioClip SfxPlayerDead;
    public AudioClip SfxDamageTaken;
    public AudioClip SfxRescue;
    public AudioClip SfxMenu;
    public AudioClip SfxBossAwake;
    public AudioClip SfxBossDead;
    //recebe os audios a serem usados

    [Header("Music")]
    public AudioClip MusicGAME;
    public AudioClip MusicSTAGECLEAR;
    public AudioClip MusicGAMEOVER;
    public AudioClip MusicBOSS;
    //determina os audios a serem usados

    [Header("General UI")]
    //telas do canvas
    public GameState CurrentState;    
    public GameObject GamePanel;
    public GameObject PausePanel;
    public GameObject OptPanel;
    public GameObject GameOverPanel;
    public GameObject StageClearPanel;
    public GameObject PointsPanel;
    public GameObject Go;
    public GameObject TouchControll;
    public OptionsController Opt;

    [Header("Points UI")]
    //controle da tela de pontuação
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
    //controle da tela do jogador
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
    //controle da barra de vida do inimigo
    public GameObject EnemyBar;
    public Image[] EnemyLifeImg;
    public float TimerEnemyBar = 4f;

    [Header("Boss UI")]
    //controle da barra de vida do chefe
    public GameObject BossBar;
    public Image[] BossLifeImg;    

    private void Start()
    {//desiga o cursosr do mouse na tela de jogo
        Cursor.visible=false;
        //verifica se a tela de touch esta ligada
        TouchScreenSwitch();
        //recebe os gameobjects de audiosource
        MusicSource = GameObject.FindWithTag("MusicSource");
        SfxSource = GameObject.FindWithTag("SFXSource");
        MusicSource.GetComponent<AudioSource>().clip=MusicGAME;
        MusicSource.GetComponent<AudioSource>().Play();
        MusicSource.GetComponent<AudioSource>().loop = true;
        ChancesTXT.text = PlayerChances.ToString();
        CurrentState = GameState.GAMEPLAY;
        Started = true;
        MaxLife = TotalLife;
    }
    private void Update()
    {       //verifica o estado de jogo

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
    {//tempo para a abarra de vida do inimigo sumir da tela
        EnemyBarVanish();
    }
    public void PlaySFX(AudioClip sfxClip)
    {//função de tocar efeitos sonoros
        SfxSource.GetComponent<AudioSource>().PlayOneShot(sfxClip, 0.9f);
    }    
    public void BossHeartController(float EnemyTotalLife)
    {//controla a vida do chefe
        foreach(Image h in BossLifeImg)
            h.enabled = false;

        for(int i = 0; i < EnemyTotalLife; i++)
            BossLifeImg[i].enabled = true;
    }
    public void Chances()
    {//controla as chances da personagem
        PlayerChances -= 1;
        ChancesTXT.text = PlayerChances.ToString();
    }
    public void EnemyHeartController(int EnemyTotalLife)
    {//controla a barra de inimigo
        EnemyBar.SetActive(true);
        foreach(Image h in EnemyLifeImg)
            h.enabled = false;

        for(int i = 0; i < EnemyTotalLife; i++)
            EnemyLifeImg[i].enabled = true;
    }
    public void EnemyBarVanish()
    {//função para a barra do inimigo sumir
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
    {//controle da barra de vida da personagem
        foreach(Image h in LifeImg)
            h.enabled = false;

        for(int i = 0; i < TotalLife; i++)
            LifeImg[i].enabled = true;
    }
    public void Maskeds()
    {//recebe qtos personagem receberam amscaras
        MaskedNpcs += 1;
        LifeRecover(MaskedNpcs/2);
        MaskedTXT.text = MaskedNpcs.ToString();
    }
    public void Rescueds()
    {//recebe qtos personagens foram resgatados
        RescuedNpcs += 1;
        LifeRecover(RescuedNpcs);
        RescuedTXT.text = RescuedNpcs.ToString();
    }
    public void LifeRecover(int personSvd)
    {//recupera a vida  da personagem pela qtdade de pessoas salvas/com mascara
        if(personSvd % 4 == 0 && TotalLife<MaxLife)
        {
            TotalLife += 1;
            HeartController();
        }        
    }
    public void StageGoBar()
    {//chama a barra de continuar
        Go.SetActive(true);
    }
    public void PauseScr()
    {// a tela de pause ao usar a tela apropriada (enter no teclado)
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
    {//volta da tela de pause por botão
        PlaySFX(SfxMenu);
        CurrentState = GameState.GAMEPLAY;
        Cursor.visible = false;
        GameEnd = false;
        GameWorld.gameObject.SetActive(true);
        GamePanel.SetActive(true);
        PausePanel.SetActive(false);
    }
    public void StageClear()
    {//tela chamada ao fim do cenário com exito
        Cursor.visible = true;
        GameEnd = true;
        CurrentState = GameState.STAGECLEAR;
        GamePanel.SetActive(false);        
        StageClearPanel.SetActive(true);
        MusicSource.GetComponent<AudioSource>().Stop();
        MusicSource.GetComponent<AudioSource>().PlayOneShot(MusicSTAGECLEAR, 1f);        
    }    
    public void GameOver()
    {//chama a tela de fimd e jogo se a personagem morreu e gastou todas suas chances
        Cursor.visible = true;
        Started = false;
        GamePanel.SetActive(false);
        GameOverPanel.SetActive(true);        
        PlayerTransform.gameObject.SetActive(false);
        CurrentState = GameState.GAMEOVER;
        MusicSource.GetComponent<AudioSource>().Stop();
        MusicSource.GetComponent<AudioSource>().PlayOneShot(MusicGAMEOVER, 0.7f);       
    }
    private void PointsScr()
    {//chama  a tela d epontuação final
        PlaySFX(SfxMenu);
        if(CurrentState == GameState.GAMEOVER)
            GameOverPanel.SetActive(false);
        else
            StageClearPanel.SetActive(false);

        CurrentState = GameState.POINTS;
        PointsPanel.SetActive(true);
    }
    private void PointCounter()
    {//calcula a pontuação pelas pessoas que receberam mascara/foram salvas
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
    {//recebe o nome num textfield
        namePoints = getname;
    }
    public void EndNameInput()
    {//termina de receber o nome e chama a função para salvar a pontuação
        Buttons.SetActive(true);
        GameFinish();
        NameField.SetActive(false);
        AddScoreEntry(TotalPoints, namePoints);
    }
    public void AddScoreEntry(int score, string name)
    {//função que recebe a pontuação e a grava na lista e depois num arquivo
        HighScore entry = new HighScore { Score = score, Name = name };

        BinaryFormatter bf = new BinaryFormatter();

        //se o arquivo existir
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
        {//arquivo sendo criado a primeira vez
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
    {//reinicia o jogo chamado por botão
        PlaySFX(SfxMenu);
        Started = false;
        SceneManager.LoadScene("Menu");
    }
    public void GameQuit()
    {//finaliza o jogo chamado por botão
        PlaySFX(SfxMenu);
        Application.Quit();
    }    
    private void GameOut()
    {        //determina o primeiro botão para o eventsystem e poder navegar com o direcional
        GameObject first = GameObject.Find("Back");
        EventSystem _event = EventSystem.current.GetComponent<EventSystem>();
        _event.firstSelectedGameObject = first;
        _event.SetSelectedGameObject(first);
    }
    private void GameFinish()
    {//determina o primeiro botão do event system
        PlaySFX(SfxMenu);
        GameObject first = GameObject.Find("Restart");
        EventSystem _event = EventSystem.current.GetComponent<EventSystem>();
        _event.firstSelectedGameObject = first;
        _event.SetSelectedGameObject(first);
    }
    public void CallOptions()
    {//chamado por botão
        PausePanel.SetActive(false);
        OptPanel.SetActive(true);
        GameOut();
    }
    public void OptionsRtrn()
    {//chamado por botão
        TouchScreenSwitch();
        OptPanel.SetActive(false);
        PausePanel.SetActive(true);
        GameOut();
    }
    public void TouchScreenSwitch()
    {//verifica a ativação da tela de touch
        if(OptionsController.TouchScreen)
            TouchControll.SetActive(true);
        else
            TouchControll.SetActive(false);
    }
}
