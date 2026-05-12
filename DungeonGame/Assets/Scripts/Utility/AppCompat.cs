using System;
using UnityEngine;

public class SoundAppPaths
{
    public static readonly string PERSISTENT_DATA = Application.persistentDataPath;
    public static readonly string PATH_RESOURCE_SFX = "Sound/SFX";
    public static readonly string PATH_RESOURCE_MUSIC = "Sound/Music";
}

public class AudioNames
{
    public static readonly string BackgroundMusic = "InGameMusic2";
    public static readonly string ItemPickupSfx = "PickupItem";
}

public enum T_AudioSources
{
    MUSIC_BACKGROUND = 0,
    SFX_ITEM_PICKUP  = 1,
    SFX_PISTOL_SHOT  = 2
}
