using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    //controlador do menu de opções da tela inicial
    public GameObject MusicSource;
    public GameObject SfxSource;
    public AudioClip SfxMenu;

    public Text m_MscText;
    public bool m_Music;
    public Text m_SfxText;
    public bool m_Sfx;

    public Text TouchTxt;
#if UNITY_ANDROID //diretiva de compilação para a plataformaandroid
    public static bool TouchScreen=true;
#else
    public static bool TouchScreen=false;
#endif

    private void Start()
    {
        MusicSource = GameObject.FindWithTag("MusicSource");
        SfxSource = GameObject.FindWithTag("SFXSource");
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
        SfxSource.GetComponent<AudioSource>().PlayOneShot(sfxClip, 0.9f);
    }
    //liga ou desliga a musica;
    public void MusicSwitch()
    {
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
    //liga ou desliga os efeitos sonoros;
    {
        PlaySFX(SfxMenu);
        if(!m_Sfx)
        {
            SfxSource.GetComponent<AudioSource>().mute = true;
            m_SfxText.text = "SFX OFF";
            m_Sfx = true;
        }
        else
        {
            SfxSource.GetComponent<AudioSource>().mute = false;
            m_SfxText.text = "SFX ON";
            m_Sfx = false;
        }
    }
    public void TouchSwitch()
        //liga ou desliga a tela de controle para celular
    {
        PlaySFX(SfxMenu);
        if(!TouchScreen)
        {
            TouchTxt.text = "TOUCH CONTROLL ON";
            TouchScreen = true;
        }
        else
        {
            TouchTxt.text = "TOUCH CONTROLL OFF";
            TouchScreen = false;
        }
    }
}
