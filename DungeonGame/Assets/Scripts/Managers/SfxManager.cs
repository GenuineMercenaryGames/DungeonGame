using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SfxManager : Singleton<SfxManager>
{
    private float m_sfxVolume = 0.2f;

    private Dictionary<string, AudioClip> m_sfxSoundDictionary = null;

    private AudioSource m_sfxMusic;

    public override void Awake()
    {
        base.Awake();

        m_sfxMusic = CreteAudioSource("Sfx", false);

        m_sfxSoundDictionary = new Dictionary<string, AudioClip>();

        AudioClip[] sfxAudioVector = Resources.LoadAll<AudioClip>(SoundAppPaths.PATH_RESOURCE_SFX);
        for (int i = 0; i < sfxAudioVector.Length; i++)
        {
            m_sfxSoundDictionary.Add(sfxAudioVector[i].name, sfxAudioVector[i]);
            Debug.Log(sfxAudioVector[i].name);
        }
    }

    public AudioSource CreteAudioSource(string name, bool isLoop)
    {
        GameObject temporalAudioHost = new GameObject(name);
        AudioSource audioSource = temporalAudioHost.AddComponent<AudioSource>() as AudioSource;
        audioSource.playOnAwake = false;
        audioSource.loop = isLoop;
        audioSource.spatialBlend = 0.0f;
        temporalAudioHost.transform.SetParent(this.transform);
        return audioSource;
    }

    public void PlaySfx(string audioName)
    {
        if (m_sfxSoundDictionary == null)
        {
            Debug.LogError("m_sfxSoundDictionary is NULL");
            return;
        }

        if (m_sfxSoundDictionary.TryGetValue(audioName, out AudioClip clip))
        {
            //m_sfxMusic.clip = clip;
            //m_sfxMusic.volume = m_sfxVolume;
            m_sfxMusic.PlayOneShot(clip, m_sfxVolume);
            Debug.Log("Sfx music");
        }
        else
        {
            Debug.LogWarning("Sfx not found: " + audioName);
        }
    }

    public float SfxVolume
    {
        get
        {
            return m_sfxVolume;
        }
        set
        {
            value = Mathf.Clamp(value, 0, 1);
            m_sfxMusic.volume = m_sfxVolume;
            m_sfxVolume = value;
        }
    }
    public float SfxVolumeSave
    {
        get
        {
            return m_sfxVolume;
        }
        set
        {
            value = Mathf.Clamp(value, 0, 1);
            m_sfxMusic.volume = m_sfxVolume;
            //PlayerPrefs.SetFloat(AppPlayerPrefKeys.SFX_VOLUME, value);
            m_sfxVolume = value;
        }
    }
}