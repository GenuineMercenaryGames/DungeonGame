using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    #region Variables

    [Header("Image References")]
    [SerializeField] private Image transitionScreen;

    [Header("Menu References - Other")]
    [SerializeField] private Transform titleMenu;
    [SerializeField] private Transform mainMenu;
    [SerializeField] private Transform contractMenu;
    [SerializeField] private Transform quitPopUpMenu;
    [SerializeField] private Transform tutorialPopUpMenu;

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
        StartCoroutine(TransitionCoroutine(0.5f, 0.0f, false));
        LoadTitleMenu();
    }

    public IEnumerator TransitionCoroutine(float transitionSpeed, float targetOpacity, bool disableClickDuringTransition)
    {
        targetOpacity = Mathf.Clamp01(Mathf.Abs(targetOpacity));
        transitionScreen.raycastTarget = disableClickDuringTransition; // Disable clicking during transition

        float sign = 1.0f;
        if (targetOpacity < transitionScreen.color.a) sign = -1.0f;

        while (Mathf.Abs(transitionScreen.color.a - targetOpacity) > 0.001f)
        {
            float opacity = transitionScreen.color.a + sign * transitionSpeed * Time.deltaTime;
            transitionScreen.color = new Color(transitionScreen.color.r, transitionScreen.color.g, transitionScreen.color.b, opacity);
            yield return null;
        }

        transitionScreen.color = new Color(transitionScreen.color.r, transitionScreen.color.g, transitionScreen.color.b, targetOpacity);
        transitionScreen.raycastTarget = false; // Enable clicking at the end of the transition
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
    public void LoadQuitPopUpMenu() { LoadMenu(quitPopUpMenu); }
    public void LoadTutorialPopUpMenu() { LoadMenu(tutorialPopUpMenu); }
    public void LoadMainMenuConditional()
    {
        if (UserDataHandler.isFirstTimeBoot)
        {
            UserDataHandler.isFirstTimeBoot = false;
            UserDataHandler.SaveUserAuxData();
            LoadContractMenu();
        }
        else
        {
            LoadMainMenu();
        }
    }
    public void LoadPlayMenuConditional()
    {
        if (UserDataHandler.isFirstTimePlay)
        {
            UserDataHandler.isFirstTimePlay = false;
            UserDataHandler.SaveUserAuxData();
            LoadTutorialPopUpMenu();
        }
        else
        {
            LoadPlayMenu();
        }
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

    #region PublicMethods - Save

    public void SaveSettings()
    {
        UserDataHandler.SaveUserSettings();
    }

    public void SaveGame()
    {
        UserDataHandler.SaveUserSaveData();
    }

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
        SetMenuLoaded(quitPopUpMenu, false);
        SetMenuLoaded(tutorialPopUpMenu, false);
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
