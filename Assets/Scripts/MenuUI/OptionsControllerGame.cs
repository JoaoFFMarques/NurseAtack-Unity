using UnityEngine;
using UnityEngine.UI;

public class OptionsControllerGame : MonoBehaviour
{//contralador do menud e opções na tela de gameplay
    public GameObject MusicSource;
    public GameObject SFXSource;
    public AudioClip SfxMenu;

    public Text m_MscText;
    public bool m_Music;
    public Text m_SfxText;
    public bool m_Sfx;

    public Text TouchTxt;

    private void Start()
    {//encontra os audiosource que não foram destruidos na transição de tela
        MusicSource = GameObject.FindWithTag("MusicSource");
        SFXSource = GameObject.FindWithTag("SFXSource");
        //verifica se estão ligados ou delsigados
        if(MusicSource.GetComponent<AudioSource>().mute == true)
        {
            m_MscText.text = "MUSIC OFF";
            m_Music = true;
        }
        else
        {
            m_MscText.text = "MUSIC ON";
            m_Music = false;
        }

        if(SFXSource.GetComponent<AudioSource>().mute == true)
        {
            m_SfxText.text = "SFX OFF";
            m_Sfx = true;
        }
        else
        {
            m_SfxText.text = "SFX ON";
            m_Sfx = false;
        }
        
    }
    private void Update()
    {//verifica se a tela de touch para controle esta ligada ou desligada
        if(!OptionsController.TouchScreen)
        {
            TouchTxt.text = "TOUCH CONTROLL OFF";
        }
        else
        {
            TouchTxt.text = "TOUCH CONTROLL ON";
        }
    }
    public void PlaySFX(AudioClip sfxClip)
    {
        SFXSource.GetComponent<AudioSource>().PlayOneShot(sfxClip, 0.9f);
    }
    public void MusicSwitch()
    {//liga delsiga a musica
        PlaySFX(SfxMenu);
        if(!m_Music)
        {
            MusicSource.GetComponent<AudioSource>().mute = true;
            m_MscText.text = "MUSIC OFF";
            m_Music = true;
        }
        else
        {
            MusicSource.GetComponent<AudioSource>().mute = false;
            m_MscText.text = "MUSIC ON";
            m_Music = false;
        }
    }
    public void SFXSwitch()
    {//liga delsiga os efeitos sonoros
        PlaySFX(SfxMenu);
        if(!m_Sfx)
        {
            SFXSource.GetComponent<AudioSource>().mute = true;
            m_SfxText.text = "SFX OFF";
            m_Sfx = true;
        }
        else
        {
            SFXSource.GetComponent<AudioSource>().mute = false;
            m_SfxText.text = "SFX ON";
            m_Sfx = false;
        }
    }
    public void TouchSwitch()
    {//liga delsiga a touchscreen
        PlaySFX(SfxMenu);
        if(!OptionsController.TouchScreen)
        {
            TouchTxt.text = "TOUCH CONTROLL ON";
            OptionsController.TouchScreen = true;
        }
        else
        {
            TouchTxt.text = "TOUCH CONTROLL OFF";
            OptionsController.TouchScreen = false;
        }
    }
}
