using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] List<MusicData> musicData;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Bonus SFX CLIP")]
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip cancel;
    public float musicVolume => musicSource.volume;
    public float sfxVolume => sfxSource.volume;
    public bool musicMuted => musicSource.mute;
    public bool sfxMuted => sfxSource.mute;

    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayClick() => PlaySFX(click);
    public void PlayCancel() => PlaySFX(cancel);
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void PlaySFX(AudioClip clip, float maxPitchAddition)
    {
        float bufferPitch = sfxSource.pitch;
        sfxSource.pitch = UnityEngine.Random.Range(bufferPitch, bufferPitch + maxPitchAddition);
        sfxSource.PlayOneShot(clip);
        sfxSource.pitch = bufferPitch;
    }
    public void PlaySFXWithRandomPitch(AudioClip clip)
    {
        float bufferPitch = sfxSource.pitch;
        sfxSource.pitch = UnityEngine.Random.Range(bufferPitch + 0.1f, bufferPitch - 0.1f);
        sfxSource.PlayOneShot(clip);
        sfxSource.pitch = bufferPitch;

    }
    public void PlayMusic(string name)
    {
        foreach(var music in musicData)
        {
            if(music.name == name)
            {
                musicSource.Stop();
                musicSource.clip = music.clip;
                musicSource.Play();
            }
        }
    }
    public async void PlayMusicWithSmoothTrans(string name)
    {
        foreach (var music in musicData)
        {
            if (music.name == name)
            {
                float bufferVolume = musicSource.volume;
                musicSource.DOFade(0, 1.5f);
                await Task.Delay(1500);
                musicSource.Stop();
                musicSource.clip = music.clip;
                musicSource.Play();
                musicSource.DOFade(1, 1.75f);
                return;
            }
        }
    }
    public void ToggleMusic(bool value) => musicSource.mute = !value;
    public void ToggleAudio(bool value) => sfxSource.mute = !value;
    public void StopMusic()=> musicSource.Stop();
    public async void PlayMusicWithSmoothTrans(AudioClip clip)
    {
        float bufferVolume = musicSource.volume;
        musicSource.DOFade(0, 1.5f);
        await Task.Delay(1500);
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
        musicSource.DOFade(1, 1.75f);
        
    }
    public void VolumeUp_Music()
    {
        AudioManager.instance.PlaySFXWithRandomPitch(click);
        float volume = musicVolume;
        volume += 0.1f;
        volume = Mathf.Clamp(volume, 0, 1);
        musicSource.volume = volume;
    }
    public void VolumeDown_Music()
    {
        AudioManager.instance.PlaySFXWithRandomPitch(click);
        float volume = musicVolume;
        volume -= 0.1f;
        volume = Mathf.Clamp(volume, 0, 1);
        musicSource.volume = volume;
    }
    public void VolumeUp_SFX()
    {
        float volume = sfxVolume;
        volume += 0.1f;
        volume = Mathf.Clamp(volume, 0, 1);
        sfxSource.volume = volume;
        AudioManager.instance.PlaySFXWithRandomPitch(click);
    }
    public void VolumeDown_SFX()
    {
        float volume = sfxVolume;
        volume -= 0.1f;
        volume = Mathf.Clamp(volume, 0, 1);
        sfxSource.volume = volume;
        AudioManager.instance.PlaySFXWithRandomPitch(click);
    }

}
[Serializable]
public class MusicData
{
    public string name; 
    public AudioClip clip;
}
