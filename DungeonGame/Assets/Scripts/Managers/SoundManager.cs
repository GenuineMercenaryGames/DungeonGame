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

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioSourceMaster;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioSource audioSourceSFX;
    [SerializeField] private AudioSource audioSourceEntity;
    [SerializeField] private AudioSource audioSourceUI;

    [Header("Sounds")]
    [SerializeField] private Sound[] SoundsList;

    public Dictionary<string, Sound> Sounds = new();

    void Start()
    {
        foreach (var sound in SoundsList)
        {
            if (sound.name == null || sound.clip == null)
                continue;
            Sounds.Add(sound.name, sound);
        }
    }

    public void PlaySound(Sound sound)
    {
        PlaySound(sound.clip, sound.type);
    }

    public void PlaySound(string name)
    {
        if (Sounds.ContainsKey(name))
            PlaySound(Sounds[name]);
        else
            Debug.Log($"Could not find Sound with name \"{name}\"");
    }

    public void PlaySound(AudioClip clip, SoundType type)
    {
        switch (type)
        {
            case SoundType.Master: PlaySoundMaster(clip); break;
            case SoundType.Music: PlaySoundMusic(clip); break;
            case SoundType.SFX: PlaySoundSFX(clip); break;
            case SoundType.Entity: PlaySoundEntity(clip); break;
            case SoundType.UI: PlaySoundUI(clip); break;
        }
    }

    public void PlaySoundMaster(AudioClip clip) { audioSourceMaster.PlayOneShot(clip); }
    public void PlaySoundMusic(AudioClip clip) { audioSourceMusic.Stop(); audioSourceMusic.clip = clip; audioSourceMusic.Play(); }
    public void PlaySoundSFX(AudioClip clip) { audioSourceSFX.PlayOneShot(clip); }
    public void PlaySoundEntity(AudioClip clip) { audioSourceEntity.PlayOneShot(clip); }
    public void PlaySoundUI(AudioClip clip) { audioSourceUI.PlayOneShot(clip); }

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
