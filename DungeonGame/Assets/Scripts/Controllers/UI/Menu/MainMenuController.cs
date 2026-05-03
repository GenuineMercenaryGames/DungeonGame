using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    #region Variables

    [Header("Menu References - Other")]
    [SerializeField] private Transform titleMenu;
    [SerializeField] private Transform mainMenu;

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
        AdjustInitialSettings();
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

    private void AdjustInitialSettings()
    {
        // TODO : Add some logic to save this stuff later on with user settings files and whatnot, but for now, we always do this logic no matter what.
        SystemLanguage syslang = Application.systemLanguage;
        Language lang;
        switch (syslang)
        {
            default:
            case SystemLanguage.English:
                lang = Language.English;
                break;
            case SystemLanguage.Catalan:
            case SystemLanguage.Spanish:
                lang = Language.Spanish;
                break;
            case SystemLanguage.French:
                lang = Language.French;
                break;
            case SystemLanguage.German:
                lang = Language.German;
                break;
        }
        LanguageManager.SetLanguage(lang);
    }

    #endregion
}
