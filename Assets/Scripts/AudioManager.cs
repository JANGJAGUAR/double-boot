using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } = null;

    [SerializeField]
    public AudioSource bgmSource;

    [SerializeField]
    public AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(string name)
    {
        AudioClip bgmClip = Resources.Load<AudioClip>(name);
        if (bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.volume = PlayerPrefs.GetFloat("BGM", 0.6f);
            bgmSource.Play();
        }
        else
        {
            Debug.LogError("BGM Clip�� �����ϴ�");
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(string name)
    {
        AudioClip sfxClip = Resources.Load<AudioClip>(name);

        if (sfxClip != null)
        {
            if (sfxSource.isPlaying == false)
            {
                sfxSource.clip = sfxClip;
                sfxSource.volume = PlayerPrefs.GetFloat("SFX", 0.6f);
                sfxSource.spatialBlend = 0;
                sfxSource.Play();
                return;
            }
        }
    }

    public void StopSFX()
    {
        if (sfxSource.isPlaying)
        {
            sfxSource.Stop();
        }
    }
}
