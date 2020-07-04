using UnityEngine;
//instancia o objeto estático para mantê-lo entre as transições de cenas
public class AudioController : MonoBehaviour
{
    public static AudioController instance = null;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
