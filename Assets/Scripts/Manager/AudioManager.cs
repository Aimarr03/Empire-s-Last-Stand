using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] List<MusicData> musicData;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    public float musicVolume => musicSource.volume;
    public float sfxVolume => sfxSource.volume;

    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
    public void VolumeUp_Music()
    {
        float volume = musicVolume;
        volume += 0.1f;
        volume = Mathf.Clamp(volume, 0, 1);
        musicSource.volume = volume;
    }
    public void VolumeDown_Music()
    {
        float volume = musicVolume;
        volume -= 0.1f;
        volume = Mathf.Clamp(volume, 0, 1);
        musicSource.volume = volume;
    }
    public void VolumeUp_SFX()
    {
        float volume = sfxVolume;
        volume += 0.1f;
        volume = Mathf.Clamp(volume, 0, 100);
        sfxSource.volume = volume;
    }
    public void VolumeDown_SFX()
    {
        float volume = sfxVolume;
        volume -= 0.1f;
        volume = Mathf.Clamp(volume, 0, 1);
        sfxSource.volume = volume;
    }

}
[Serializable]
public class MusicData
{
    public string name; 
    public AudioClip clip;
}
