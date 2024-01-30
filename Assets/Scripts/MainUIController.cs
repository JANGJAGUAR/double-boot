using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject levelSelectDisplay;

    [SerializeField]
    private GameObject preferencesDisplay;

    private float BGMVolume;
    private float SFXVolume;
    private int BGMOrder;
    private int SFXOrder;

    [SerializeField]
    private GameObject[] BGMSize;
    [SerializeField]
    private GameObject[] SFXSize;

    private void Start()
    {
        levelSelectDisplay.SetActive(false);
        preferencesDisplay.SetActive(false);

        AudioManager.Instance.PlayBGM("AudioClip/Menu");
        
        BGMVolume = PlayerPrefs.GetFloat("BGM", 0.6f) * 100;
        SFXVolume = PlayerPrefs.GetFloat("SFX", 0.6f) * 100;
        BGMOrder = PlayerPrefs.GetInt("BGMOrder", 3);
        SFXOrder = PlayerPrefs.GetInt("SFXOrder", 3);
        // ���� ���� Ȯ��

        Debug.Log("BGM order�� " + BGMOrder);
        Debug.Log("SFX order�� " + SFXOrder);

        for (int i=0;i<6;i++)
        {
            BGMSize[i].SetActive(false);
        }
        BGMSize[BGMOrder].SetActive(true);
        // BGM UI �ݿ�
        
        for(int i=0;i<6;i++)
        {
            SFXSize[i].SetActive(false);
        }
        SFXSize[SFXOrder].SetActive(true);
        // SFX UI �ݿ�
    }

    public void OnClick_LevelSelectDisplay(bool On)
    {
        levelSelectDisplay.SetActive(On);
        AudioManager.Instance.PlaySFX("AudioClip/Button");
    }

    public void OnClick_PreferencesDisplay(bool On)
    {
        preferencesDisplay.SetActive(On);
        AudioManager.Instance.PlaySFX("AudioClip/Button");
    }

    public void OnClick_ExitButton()
    {
        AudioManager.Instance.PlaySFX("AudioClip/Button");
        Application.Quit();
    }

    public void OnClick_LoadToScene(string LevelName)
    {
        AudioManager.Instance.PlaySFX("AudioClip/Button");
        SceneController.instance.LoadStage(LevelName);
    }
    
    public void OnClick_BGMVolumeBig()
    {
        AudioManager.Instance.PlaySFX("Button");
        if (BGMVolume < 100)
        {
            BGMVolume += 20;
            BGMOrder++;
            for (int i = 0; i < 6; i++)
            {
                BGMSize[i].SetActive(false);
            }
            BGMSize[BGMOrder].SetActive(true);
            PlayerPrefs.SetFloat("BGM", BGMVolume / 100.0f);
            PlayerPrefs.SetInt("BGMOrder", BGMOrder);
            AudioManager.Instance.bgmSource.volume = PlayerPrefs.GetFloat("BGM", 0.6f);
        }
    } // BGM ���� ���

    public void OnClick_BGMVolumeSmall()
    {
        AudioManager.Instance.PlaySFX("Button");
        if (BGMVolume > 10)
        {
            BGMVolume -= 20;
            BGMOrder--;
            for (int i = 0; i < 6; i++)
            {
                BGMSize[i].SetActive(false);
            }
            BGMSize[BGMOrder].SetActive(true);
            PlayerPrefs.SetFloat("BGM", BGMVolume / 100.0f);
            PlayerPrefs.SetInt("BGMOrder", BGMOrder);
            AudioManager.Instance.bgmSource.volume = PlayerPrefs.GetFloat("BGM", 0.6f);
        }
    } // BGM ���� �϶�

    public void OnClick_SFXVolumeBig()
    {
        AudioManager.Instance.PlaySFX("Button");
        if (SFXVolume < 100)
        {
            SFXVolume += 20;
            SFXOrder++;
            for (int i = 0; i < 6; i++)
            {
                SFXSize[i].SetActive(false);
            }
            SFXSize[SFXOrder].SetActive(true);

            PlayerPrefs.SetFloat("SFX", SFXVolume / 100.0f);
            PlayerPrefs.SetInt("SFXOrder", SFXOrder);

            AudioManager.Instance.sfxSource.volume = PlayerPrefs.GetFloat("SFX", 0.6f);
        }
    }
    public void OnClick_SFXVolumeSmall()
    {
        AudioManager.Instance.PlaySFX("Button");
        if (SFXVolume > 10)
        {
            SFXVolume -= 20;
            SFXOrder--;
            for (int i = 0; i < 6; i++)
            {
                SFXSize[i].SetActive(false);
            }
            SFXSize[SFXOrder].SetActive(true);

            PlayerPrefs.SetFloat("SFX", SFXVolume / 100.0f);
            PlayerPrefs.SetInt("SFXOrder", SFXOrder);

            AudioManager.Instance.sfxSource.volume = PlayerPrefs.GetFloat("SFX", 0.6f);
        }
    }
}
