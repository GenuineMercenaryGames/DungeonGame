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
        UserDataHandler.LoadAllData();
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(1);
    }
}
