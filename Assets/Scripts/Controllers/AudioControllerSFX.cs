using UnityEngine;
//instancia o objeto estático para mantê-lo entre as transições de cenas
public class AudioControllerSFX : MonoBehaviour
{
    public static AudioControllerSFX instance = null;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
