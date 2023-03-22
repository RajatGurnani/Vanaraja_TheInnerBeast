using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<T>();
            if (Instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                Instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
