using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject creditsPanel;

    public void Start()
    {
        mainMenuPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1); // We have to set the order of scenes!!! (0: mainMenu, 1: Game Scene ... )
    }

    public void OpenSettings()
    {
        Debug.Log("Settings"); // We will implement this later with volume sliders, graphic settings, etc.
    }

    public void OpenCredits()
    {
        Debug.Log("Credits & Contact");
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit(); // This will not work in the editor, but it should work in a built game
        Debug.Log("Quit Game");
    }

    /// These are the buttons in the credits panel, which will take us back to the main menu or open our social media pages. 
    public void BackToMenu()
    {
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OpenGitHub()
    {
        Application.OpenURL("https://github.com/GenuineMercenaryGames/genuinemercenarygames.github.io.git");
    }

    public void OpenX()
    {
        Application.OpenURL("https://twitter.com/GenuineMercenaryGames");
    }

    public void OpenYoutube() { 
        Application.OpenURL("https://www.youtube.com/@GenuineMercenaryGames");
    }
}
