using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsCanvas;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public Slider volSlider;
    public Slider bloomSlider;

    public AudioMixer mixer;
    public UnityEngine.Rendering.VolumeProfile volumeProfile;
    public Button back;

    private void Awake()
    {
        SetVolume(PersistentData.data.volume);
        volSlider.value = PersistentData.data.volume;

        SetBloom(PersistentData.data.bloom);
        bloomSlider.value = PersistentData.data.bloom;

        volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));        
    }

    private void Update()
    {
        // Check for the Escape key press
        if (Time.timeScale == 1f)
        {
            optionsCanvas.SetActive(false);
        }
    }

    public void SetVolume (float volume)
    {
        mixer.SetFloat("volume", volume);
        PersistentData.data.volume = volume;
        if(volume <= -20)
        {
            mixer.SetFloat("volume", -80);
            PersistentData.data.volume = -80;
        }
    }

    public void SetBloom (float intensity)
    {
        UnityEngine.Rendering.Universal.Bloom bloom;

        if (!volumeProfile.TryGet(out bloom)) throw new System.NullReferenceException(nameof(bloom));
        bloom.intensity.Override(intensity);
        PersistentData.data.bloom = intensity;
    }

    public void Back()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        optionsCanvas.SetActive(false);
        if (currentScene.name == ("MainMenu"))
            mainMenu.SetActive(true);
        else
            pauseMenu.SetActive(true);
    }
}
