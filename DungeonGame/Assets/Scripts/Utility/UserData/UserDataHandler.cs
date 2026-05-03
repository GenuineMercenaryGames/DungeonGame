using System.IO;
using UnityEngine;

public static class UserDataHandler
{
    #region Variables

    private static string pathBase = Application.persistentDataPath;
    private static string pathSettings = Path.Combine(pathBase, "settings.json");
    private static string pathSaveData = Path.Combine(pathBase, "savedata.json");

    private static bool isFirstTime = false;

    #endregion

    #region PublicMethods

    public static bool IsFirstLaunch() { return isFirstTime; }

    public static void SaveUserSettings()
    {
        UserSettings config = new();
        config.language = (int)LanguageManager.GetLanguage();
        config.quality = QualitySettings.GetQualityLevel();
        StructWrite(pathSettings, config);
    }

    public static void SaveUserSaveData()
    {
        // TODO : Implement these systems so that we can actually save some data lol
        UserSaveData save = new();
        save.money = 0;
        save.level = 0;
        save.xp = 0;
        StructWrite(pathSaveData, save);
    }

    public static void SaveAllData()
    {
        SaveUserSettings();
        SaveUserSaveData();
    }

    public static void LoadUserSettings()
    {
        UserSettings config = new();
        if (!StructRead(pathSettings, out config))
        {
            isFirstTime = true;
            config = UserSettings.Default();
        }
        LanguageManager.SetLanguage(config.language);
        QualitySettings.SetQualityLevel(config.quality);
    }

    public static void LoadUserSaveData()
    {
        UserSaveData save = new();
        if (!StructRead(pathSaveData, out save))
        {
            isFirstTime = true;
            save = UserSaveData.Default();
        }
        // TODO : Implement these systems so that we can actually load the data somewhere lol
    }

    public static void LoadAllData()
    {
        LoadUserSettings();
        LoadUserSaveData();
    }

    #endregion

    #region PrivateMethods

    private static bool StructWrite<T>(string path, T data) where T : struct
    {
        try
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(path, json);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool StructRead<T>(string path, out T data) where T : struct
    {
        try
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<T>(json);
            return true;
        }
        catch
        {
            data = default(T);
            return false;
        }
    }

    #endregion

}
