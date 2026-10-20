
using UnityEngine;

public class NetworkManagerPersistence : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
