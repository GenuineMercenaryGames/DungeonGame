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

    public float volumeMaster;
    public float volumeMusic;
    public float volumeSFX;
    public float volumeEntity;
    public float volumeUI;

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
        volumeMaster = 1;
        volumeMusic = 1;
        volumeSFX = 1;
        volumeEntity = 1;
        volumeUI = 1;
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
    public bool hasBootedOnce;
    public bool hasPlayedOnce;

    public void SetDefault()
    {
        hasBootedOnce = false;
        hasPlayedOnce = false;
    }
}

[System.Serializable]
public struct UserAchievementsData : IUserData
{
    public bool[] achievements;

    public void SetDefault()
    {
        achievements = new bool[(int)AchievementManager.Achievement.Count];
        for (int i = 0; i < achievements.Length; ++i)
            achievements[i] = false;
    }
}
