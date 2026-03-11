using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    #region Variables

    [Header("Menu References")]
    [SerializeField] private Transform mainMenu;
    [SerializeField] private Transform playMenu;
    [SerializeField] private Transform settingsMenu;
    [SerializeField] private Transform achievementsMenu;
    [SerializeField] private Transform creditsMenu;

    [Header("Menu Settings")]
    [SerializeField] private int gameplaySceneIndex = 1; // TODO : Change to be a string for easier handling later on.

    #endregion

    #region MonoBehaviour

    void Start()
    {
        LoadMainMenu();
    }

    #endregion

    #region PublicMethods - Buttons

    public void PlayGame()
    {
        SceneManager.LoadScene(gameplaySceneIndex);
    }

    public void LoadMainMenu()
    {
        LoadMenu(mainMenu);
    }

    public void LoadPlayMenu()
    {
        LoadMenu(playMenu);
    }

    public void LoadSettingsMenu()
    {
        LoadMenu(settingsMenu);
    }

    public void LoadAchievementsMenu()
    {
        LoadMenu(achievementsMenu);
    }

    public void LoadCreditsMenu()
    {
        LoadMenu(creditsMenu);
    }

    public void QuitGame()
    {
        Application.Quit(); // This does not work in the editor, but it does work on buils.
    }

    #endregion

    #region PublicMethods - OpenURL

    public void OpenGitHub()
    {
        OpenURL("https://github.com/GenuineMercenaryGames/genuinemercenarygames.github.io.git");
    }

    public void OpenX()
    {
        OpenURL("https://twitter.com/GenuineMercenaryGames");
    }

    public void OpenYoutube()
    { 
        OpenURL("https://www.youtube.com/@GenuineMercenaryGames");
    }

    #endregion

    #region PrivateMethods

    private void UnloadAllMenus()
    {
        SetMenuLoaded(mainMenu, false);
        SetMenuLoaded(playMenu, false);
        SetMenuLoaded(settingsMenu, false);
        SetMenuLoaded(achievementsMenu, false);
        SetMenuLoaded(creditsMenu, false);
    }

    private void SetMenuLoaded(Transform menu, bool loaded)
    {
        menu.gameObject.SetActive(loaded);
    }

    private void LoadMenu(Transform menu)
    {
        UnloadAllMenus();
        SetMenuLoaded(menu, true);
    }

    private void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    #endregion
}
