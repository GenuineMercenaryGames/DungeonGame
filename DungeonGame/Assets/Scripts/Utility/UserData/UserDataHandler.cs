using System.IO;
using UnityEngine;

public static class UserDataHandler
{
    #region Variables

    private static string pathBase = Application.persistentDataPath;
    private static string pathAuxData = Path.Combine(pathBase, "auxdata.json");
    private static string pathSettings = Path.Combine(pathBase, "settings.json");
    private static string pathSaveData = Path.Combine(pathBase, "savedata.json");
    private static string pathAchievements = Path.Combine(pathBase, "achievements.json");

    public static bool isFirstTimeBoot = false;
    public static bool isFirstTimePlay = false;

    #endregion

    #region PublicMethods

    public static void SaveUserAuxData()
    {
        UserAuxData aux = new();
        aux.hasBootedOnce = !isFirstTimeBoot;
        aux.hasPlayedOnce = !isFirstTimePlay;
        StructWrite(pathAuxData, aux);
    }

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

    public static void SaveAchievements()
    {
        UserAchievementsData achievements = new();
        achievements.achievements = AchievementManager.GetAchievementsVector();
        StructWrite(pathAchievements, achievements);
    }

    public static void SaveAllData()
    {
        SaveUserAuxData();
        SaveUserSettings();
        SaveUserSaveData();
        SaveAchievements();
    }

    public static void LoadUserAuxData()
    {
        UserAuxData aux = new();
        StructRead(pathAuxData, out aux);
        isFirstTimeBoot = !aux.hasBootedOnce;
        isFirstTimePlay = !aux.hasPlayedOnce;
    }

    public static void LoadUserSettings()
    {
        UserSettings config = new();
        StructRead(pathSettings, out config);
        LanguageManager.SetLanguage(config.language);
        QualitySettings.SetQualityLevel(config.quality);
    }

    public static void LoadUserSaveData()
    {
        UserSaveData save = new();
        StructRead(pathSaveData, out save);
        // TODO : Implement these systems so that we can actually load the data somewhere lol
    }

    public static void LoadAchievements()
    {
        UserAchievementsData achievements = new();
        StructRead(pathAchievements, out achievements);
        AchievementManager.SetAchievementsVector(achievements.achievements);
    }

    public static void LoadAllData()
    {
        LoadUserAuxData();
        LoadUserSettings();
        LoadUserSaveData();
        LoadAchievements();
    }

    #endregion

    #region PrivateMethods

    private static bool StructWrite<T>(string path, T data) where T : IUserData
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

    private static bool StructRead<T>(string path, out T data) where T : IUserData
    {
        try
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<T>(json);
            return true;
        }
        catch
        {
            T t = default(T);
            t.SetDefault();
            data = t;
            return false;
        }
    }

    #endregion

}
