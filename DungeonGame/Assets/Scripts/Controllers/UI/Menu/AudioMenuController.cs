using UnityEngine;
using UnityEngine.UI;

public class AudioMenuController : MonoBehaviour
{
    [SerializeField] private Slider sliderVolumeMaster;
    [SerializeField] private Slider sliderVolumeMusic;
    [SerializeField] private Slider sliderVolumeSFX;
    [SerializeField] private Slider sliderVolumeEntity;
    [SerializeField] private Slider sliderVolumeUI;

    void Start()
    {
        sliderVolumeMaster.value = SoundManager.Instance.GetVolumeMaster();
        sliderVolumeMusic.value = SoundManager.Instance.GetVolumeMusic();
        sliderVolumeSFX.value = SoundManager.Instance.GetVolumeSFX();
        sliderVolumeEntity.value = SoundManager.Instance.GetVolumeEntity();
        sliderVolumeUI.value = SoundManager.Instance.GetVolumeUI();
    }

    public void SetVolumeMaster() { SoundManager.Instance.SetVolumeMaster(sliderVolumeMaster.value); }
    public void SetVolumeMusic() { SoundManager.Instance.SetVolumeMusic(sliderVolumeMusic.value); }
    public void SetVolumeSFX() { SoundManager.Instance.SetVolumeSFX(sliderVolumeSFX.value); }
    public void SetVolumeEntity() { SoundManager.Instance.SetVolumeEntity(sliderVolumeEntity.value); }
    public void SetVolumeUI() { SoundManager.Instance.SetVolumeUI(sliderVolumeUI.value); }
}
