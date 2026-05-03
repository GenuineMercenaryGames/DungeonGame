using UnityEngine;
using UnityEngine.SceneManagement;

public class InitSceneScript : MonoBehaviour
{
    void Start()
    {
        Init();
        LoadMainMenu();
    }

    private void Init()
    {
        // TODO : Implement logic to load settings and user data from disk
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(1);
    }
}
