using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : SingletonPersistent<SoundManager>
{
    public enum SoundType
    {
        Master = 0,
        Music,
        SFX,
        Entity,
        UI,
    }

    [System.Serializable]
    public struct Sound
    {
        public string name;
        public AudioClip clip;
        public SoundType type;
    }

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource audioSourceGeneric;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private Sound[] SoundsList;

    public Dictionary<string, Sound> Sounds = new();

    void Start()
    {
        foreach (var sound in SoundsList)
        {
            Sounds.Add(sound.name, sound);
        }
    }

    public void PlaySound(Sound sound)
    {
        
    }

    public void SetVolume(string name, float volume)
    {
        float volumeLinear = Mathf.Clamp01(volume);
        float volumeLogarithmic = Mathf.Log10(volumeLinear) * 20;
        mixer.SetFloat(name, volumeLogarithmic);
    }

    public float GetVolume(string name)
    {
        float db;
        mixer.GetFloat(name, out db);
        float linear = Mathf.Pow(10.0f, db / 20.0f);
        return linear;
    }

    public void SetVolume(SoundType type, float volume)
    {
        switch (type)
        {
            case SoundType.Master: SetVolumeMaster(volume); break;
            case SoundType.Music: SetVolumeMusic(volume); break;
            case SoundType.SFX: SetVolumeSFX(volume); break;
            case SoundType.Entity: SetVolumeEntity(volume); break;
            case SoundType.UI: SetVolumeUI(volume); break;
        }
    }

    public float GetVolume(SoundType type)
    {
        switch (type)
        {
            case SoundType.Master: return GetVolumeMaster();
            case SoundType.Music: return GetVolumeMusic();
            case SoundType.SFX: return GetVolumeSFX();
            case SoundType.Entity: return GetVolumeEntity();
            case SoundType.UI: return GetVolumeUI();
        }
        return 0;
    }

    public void SetVolumeMaster(float volume) { SetVolume("_Volume_Master", volume); }
    public void SetVolumeSFX(float volume) { SetVolume("_Volume_SFX", volume); }
    public void SetVolumeUI(float volume) { SetVolume("_Volume_UI", volume); }
    public void SetVolumeEntity(float volume) { SetVolume("_Volume_Entity", volume); }
    public void SetVolumeMusic(float volume) { SetVolume("_Volume_Music", volume); }

    public float GetVolumeMaster() { return GetVolume("_Volume_Master"); }
    public float GetVolumeSFX() { return GetVolume("_Volume_SFX"); }
    public float GetVolumeUI() { return GetVolume("_Volume_UI"); }
    public float GetVolumeEntity() { return GetVolume("_Volume_Entity"); }
    public float GetVolumeMusic() { return GetVolume("_Volume_Music"); }

}
