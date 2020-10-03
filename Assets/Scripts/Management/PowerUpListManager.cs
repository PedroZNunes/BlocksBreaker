using UnityEngine;

public class PowerUpListManager : MonoBehaviour
{

    static private PowerUpListManager instance;
    //singleton Process
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
