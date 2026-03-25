using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackgroundMusicManager : Singleton<BackgroundMusicManager>
{
    [SerializeField] private AudioMixerGroup musicMixerGroup;

    public AudioSource m_backgroundMusic;
    private float m_musicVolume;
    public Dictionary<string, AudioClip> m_musicSoundDictionary = null;

    private Coroutine m_fadeCoroutine;
    

    public override void Awake()
    {
         base.Awake();

         m_backgroundMusic = CreateAudioSource("Music", true);

         m_musicSoundDictionary = new Dictionary<string, AudioClip>();

         if (musicMixerGroup != null)
             m_backgroundMusic.outputAudioMixerGroup = musicMixerGroup;

        m_musicVolume = 0.5f;//PlayerPrefs.GetFloat(AppPlayerPrefKeys.MUSIC_VOLUME);

         AudioClip[] backgroundAudioVector = Resources.LoadAll<AudioClip>(SoundAppPaths.PATH_RESOURCE_MUSIC);
         for (int i = 0; i < backgroundAudioVector.Length; i++)
         {
             m_musicSoundDictionary.Add(backgroundAudioVector[i].name, backgroundAudioVector[i]);
             //Debug.Log(backgroundAudioVector[i].name);
         }
    }

    public AudioSource CreateAudioSource(string name, bool isLoop)
    {
        GameObject temporalAudioHost = new GameObject(name);
        temporalAudioHost.transform.SetParent(transform);

        AudioSource audioSource = temporalAudioHost.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = isLoop;
        audioSource.spatialBlend = 0.0f;

        return audioSource;
    }

    public void PlayBackgroundMusic(string audioName)
    {
        if (m_musicSoundDictionary == null)
        {
            Debug.LogError("m_musicSoundDictionary is NULL");
            return;
        }

        if (m_musicSoundDictionary.TryGetValue(audioName, out AudioClip clip))
        {
            m_backgroundMusic.clip = clip;
            m_backgroundMusic.volume = m_musicVolume;
            m_backgroundMusic.Play();
            Debug.Log("Play music");
        }
        else
        {
            Debug.LogWarning("Music not found: " + audioName);
        }
    }

    public void PlayBackgroundMusicWithFade(string audioName, float fadeInTime = 1f)
    {
        if (m_musicSoundDictionary == null)
        {
            Debug.LogError("Music dictionary is NULL");
            return;
        }

        if (!m_musicSoundDictionary.TryGetValue(audioName, out AudioClip clip))
        {
            Debug.LogWarning("Music not found: " + audioName);
            return;
        }

        if (m_fadeCoroutine != null)
            StopCoroutine(m_fadeCoroutine);

        m_fadeCoroutine = StartCoroutine(PlayMusicFadeCoroutine(clip, fadeInTime));
    }

    public void StopBackgroundMusic()
    {
        if (m_backgroundMusic != null)
        {
            m_backgroundMusic.Stop();
        }
    }

    public void StopBackgroundMusicWithFade(float fadeOutTime = 1f)
    {
        if (m_fadeCoroutine != null)
            StopCoroutine(m_fadeCoroutine);

        m_fadeCoroutine = StartCoroutine(FadeOutCoroutine(fadeOutTime));
    }

    public void PauseBackgroundMusic()
    {
        if (m_backgroundMusic != null)
        {
            m_backgroundMusic.Pause();
        }
    }

    private IEnumerator PlayMusicFadeCoroutine(AudioClip newClip, float fadeInTime)
    {
        if (m_backgroundMusic.isPlaying)
        {
            yield return FadeTo(0f, fadeInTime * 0.5f);
            m_backgroundMusic.Stop();
        }

        m_backgroundMusic.clip = newClip;
        m_backgroundMusic.volume = 0f;
        m_backgroundMusic.Play();

        yield return FadeTo(m_musicVolume, fadeInTime);
        m_fadeCoroutine = null;
    }

    private IEnumerator FadeOutCoroutine(float fadeOutTime)
    {
        yield return FadeTo(0f, fadeOutTime);
        m_backgroundMusic.Stop();
        m_fadeCoroutine = null;
    }

    private IEnumerator FadeTo(float targetVolume, float duration)
    {
        float startVolume = m_backgroundMusic.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = duration <= 0f ? 1f : elapsed / duration;
            m_backgroundMusic.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        m_backgroundMusic.volume = targetVolume;
    }

    public float MusicVolume
    {
        get
        {
            return m_musicVolume;
        }
        set
        {
            value = Mathf.Clamp(value, 0, 1);
            m_musicVolume = value;
            if (m_backgroundMusic != null)
                m_backgroundMusic.volume = m_musicVolume;
        }
    }
    public float MusicVolumeSave
    {
        get
        {
            return m_musicVolume;
        }
        set
        {
            value = Mathf.Clamp(value, 0, 1);
            m_musicVolume = value;

            if (m_backgroundMusic != null)
                m_backgroundMusic.volume = m_musicVolume;

            //PlayerPrefs.SetFloat(AppPlayerPrefKeys.MUSIC_VOLUME, value); // Saving of the progress
        }
    }
}