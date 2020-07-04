using UnityEngine;
using UnityEngine.EventSystems;

public class Title : MonoBehaviour
{
    //controle de animaçãod e rotação de abertura do jogo
    public GameObject Spin_Title;
    public GameObject m_Title;
    public GameObject m_Buttons;
    private readonly float RotateSpead=10f;
    private float TimerVanish=6f;
    private bool m_Stop;
    private bool setselected;


    void Update()
    {//se anmação não terminou ele continua com ela
        if(!m_Stop)
        {
            Move();
            TitleVanish();
            if(Input.anyKey)
                TimerVanish = 0f;
            Cursor.visible = false;
        }
        else
        {
            m_Title.SetActive(true);
            m_Buttons.SetActive(true);
            Cursor.visible = true;
            
            if(Input.anyKey && !setselected)//a animação pdoe ser interrompida com o pressionar de qqr tecla/botão
            {
                GameObject first = GameObject.Find("Start");
                EventSystem _event = EventSystem.current.GetComponent<EventSystem>();
                _event.firstSelectedGameObject = first;
                _event.SetSelectedGameObject(first);
                setselected = true;
            }

        }
    }
    private void Move()
    {//faz o movimento do titulo e rotação na tela
        Vector3 pos = this.transform.position;
        pos.z = 0f;
        Spin_Title.transform.position = Vector3.MoveTowards(Spin_Title.transform.position, pos, 23f * Time.deltaTime);
        Spin_Title.transform.Rotate(0, 0, this.RotateSpead);
    }
    public void TitleVanish()
    {//o titulo girando some da tela
        if(TimerVanish > 0)
        {
            TimerVanish -= 1f * Time.deltaTime;
        }
        else
        {
            Spin_Title.SetActive(false);
            m_Stop = true;
        }
    }
}
