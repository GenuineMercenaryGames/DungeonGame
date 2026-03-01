using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); // We have to set the order of scenes!!! (0: mainMenu, 1: Game Scene ... )
    }

    public void OpenSettings()
    {
        Debug.Log("Settings"); // We will implement this later with volume sliders, graphic settings, etc.
    }

    public void QuitGame()
    {
        Application.Quit(); // This will not work in the editor, but it should work in a built game
        Debug.Log("Quit Game");
    }
}
