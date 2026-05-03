using UnityEngine;

public interface IUserData
{
    public void SetDefault();
}

[System.Serializable]
public struct UserSettings : IUserData
{
    public int language;
    public int quality;

    public void SetDefault()
    {
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
        language = (int)lang;
        quality = (int)QualitySettings.GetQualityLevel();
    }
}

[System.Serializable]
public struct UserSaveData : IUserData
{
    public int money;
    public int level;
    public int xp;

    public void SetDefault()
    {
        money = 0;
        level = 0;
        xp = 0;
    }
}

[System.Serializable]
public struct UserAuxData : IUserData
{
    public bool boot;
    public bool play;

    public void SetDefault()
    {
        boot = true;
        play = true;
    }
}
