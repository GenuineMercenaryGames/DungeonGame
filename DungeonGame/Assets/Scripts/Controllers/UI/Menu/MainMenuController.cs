using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    #region Variables

    [Header("Menu References - Other")]
    [SerializeField] private Transform titleMenu;
    [SerializeField] private Transform mainMenu;
    [SerializeField] private Transform contractMenu;

    [Header("Menu References - Subcategories")]
    [SerializeField] private Transform playMenu;
    [SerializeField] private Transform settingsMenu;
    [SerializeField] private Transform achievementsMenu;
    [SerializeField] private Transform creditsMenu;

    [Header("Menu References - Settings")]
    [SerializeField] private Transform languageSettingsMenu;
    [SerializeField] private Transform graphicsSettingsMenu;
    [SerializeField] private Transform audioSettingsMenu;

    [Header("Menu References - Gameplay")]
    [SerializeField] private Transform playerLoadoutMenu;
    [SerializeField] private Transform levelSelectMenu;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        LoadTitleMenu();
    }

    #endregion

    #region PublicMethods - Buttons

    public void QuitGame()
    {
        // TODO : Add logic to display an "are you sure mf???" pop up before quitting for real.
        Application.Quit(); // NOTE : Do not panic, this does not work in the editor (for obvious reasons), but it does work on release buils.
    }

    public void PlayGame(int id)
    {
        SceneManager.LoadScene(id);
    }

    public void PlayGame()
    {
        PlayGame(2);
    }

    public void LoadTitleMenu() { LoadMenu(titleMenu); }
    public void LoadMainMenu() { LoadMenu(mainMenu); }
    public void LoadPlayMenu() { LoadMenu(playMenu); }
    public void LoadSettingsMenu() { LoadMenu(settingsMenu); }
    public void LoadAchievementsMenu() { LoadMenu(achievementsMenu); }
    public void LoadCreditsMenu() { LoadMenu(creditsMenu); }
    public void LoadLanguageSettingsMenu() { LoadMenu(languageSettingsMenu); }
    public void LoadGraphicsSettingsMenu() { LoadMenu(graphicsSettingsMenu); }
    public void LoadAudioSettingsMenu() { LoadMenu(audioSettingsMenu); }
    public void LoadPlayerLoadoutMenu() { LoadMenu(playerLoadoutMenu); }
    public void LoadLevelSelectMenu() { LoadMenu(levelSelectMenu); }
    public void LoadContractMenu() { LoadMenu(contractMenu); }
    public void LoadMainMenuConditional()
    {
        if (UserDataHandler.IsFirstLaunch())
        {
            LoadContractMenu();
        }
        else
        {
            LoadMainMenu();
        }
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

    #region PublicMethods - Select Loadout

    public void SelectWeaponPrimary(GameObject prefab)
    {
        MatchManager.Instance.selectedWeaponPrimary = prefab;
    }

    public void SelectWeaponSecondary(GameObject prefab)
    {
        MatchManager.Instance.selectedWeaponSecondary = prefab;
    }

    #endregion

    #region PublicMethods - Language

    public void SetLanguage(Language language)
    {
        LanguageManager.SetLanguage(language);
        LoadLanguageSettingsMenu();
    }

    public void SetLanguageEnglish() { SetLanguage(Language.English); }
    public void SetLanguageSpanish() { SetLanguage(Language.Spanish); }
    public void SetLanguageFrench() { SetLanguage(Language.French); }
    public void SetLanguageGerman() { SetLanguage(Language.German); }

    #endregion

    #region PublicMethods - Graphics

    public void SetGraphicsQuality(int level)
    {
        QualitySettings.SetQualityLevel(level);
    }

    public void SetQualityVeryLow() { SetGraphicsQuality(0); }
    public void SetQualityLow() { SetGraphicsQuality(1); }
    public void SetQualityMedium() { SetGraphicsQuality(2); }
    public void SetQualityHigh() { SetGraphicsQuality(3); }
    public void SetQualityVeryHigh() { SetGraphicsQuality(4); }
    public void SetQualityUltra() { SetGraphicsQuality(5); }

    #endregion

    #region PrivateMethods

    private void UnloadAllMenus()
    {
        // The absolute most ugliest fucking hack ever made. This grew out of control the more menus we had, but we had to commit because there's no more time to clean this up and make a proper menu loading stack system now.
        SetMenuLoaded(titleMenu, false);
        SetMenuLoaded(mainMenu, false);
        SetMenuLoaded(playMenu, false);
        SetMenuLoaded(settingsMenu, false);
        SetMenuLoaded(achievementsMenu, false);
        SetMenuLoaded(creditsMenu, false);
        SetMenuLoaded(languageSettingsMenu, false);
        SetMenuLoaded(graphicsSettingsMenu, false);
        SetMenuLoaded(audioSettingsMenu, false);
        SetMenuLoaded(playerLoadoutMenu, false);
        SetMenuLoaded(levelSelectMenu, false);
        SetMenuLoaded(contractMenu, false);
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
