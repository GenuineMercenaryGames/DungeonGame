using System;
using UnityEditor;
using UnityEngine;

public class SoundAppPaths
{
    public static readonly String PERSISTENT_DATA = Application.persistentDataPath;
    public static readonly String PATH_RESOURCE_SFX = "Music/Sfx";
    public static readonly String PATH_RESOURCE_MUSIC = "Music/Background";
}

public class AudioNames
{
    public static readonly String BackgroundMusic = "InGameMusic2";
    public static readonly String ItemPickupSfx = "PickupItem";    
}

public enum T_AudioSources
{
    MUSIC_BACKGROUND = 0,
    SFX_ITEM_PICKUP  = 1,
    SFX_PISTOL_SHOT  = 2
}