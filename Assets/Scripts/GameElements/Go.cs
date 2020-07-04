using UnityEngine;

public class Go : MonoBehaviour
{
    //desativa o objeto de seta de continuar, chamado na propra animação da seta
    public void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
