using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerObject : MonoBehaviour
{
    private static SceneManagerObject instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Przyk�ad: znajd� mikstur� i w��cz j�
        GameObject potion = GameObject.Find("speed_potion"); // lub dok�adna nazwa mikstury w hierarchii
        if (potion != null && !potion.activeSelf)
        {
            potion.SetActive(true);
            Debug.Log("Mikstura w��czona po za�adowaniu sceny");
        }
    }
}
