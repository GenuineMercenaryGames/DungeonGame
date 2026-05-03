using UnityEngine;

[System.Serializable]
public struct UserSettings
{
    public int language;
    public int quality;

    public static UserSettings Default()
    {
        UserSettings config = new();
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
        config.language = (int)lang;
        config.quality = (int)QualitySettings.GetQualityLevel();
        return config;
    }
}

[System.Serializable]
public struct UserSaveData
{
    public int money;
    public int level;
    public int xp;

    public static UserSaveData Default()
    {
        UserSaveData save = new();
        save.money = 0;
        save.level = 0;
        save.xp = 0;
        return save;
    }
}
