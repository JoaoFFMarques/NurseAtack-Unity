using UnityEngine;
//implementação da de menu que mostra os controles para as diferenets plataformas
public class ControllScreen : MonoBehaviour
{
    public GameObject Keyboard;
    public GameObject Joystick;
    public GameObject TouchScreen;
    private GameObject Previous;


    private void Start()
    {//inicia mostrando o teclado (padrão)
        Previous = Keyboard;
        Previous.SetActive(true);
    }

    public void ShowKeyboard()
    {//chama o teclado através de um botão
        Previous.SetActive(false);
        Previous = Keyboard;
        Previous.SetActive(true);
        
    }
    public void ShowJoystick()
    {//chama o joystick através de um botão
        Previous.SetActive(false);
        Previous = Joystick;
        Previous.SetActive(true);

    }
    public void ShowTouch()
    {//chama a tela de touch através de um botão
        Previous.SetActive(false);
        Previous = TouchScreen;
        Previous.SetActive(true);

    }
}
