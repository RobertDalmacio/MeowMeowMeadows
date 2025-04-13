using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public Slider volumeSlider;
    public AudioMixer mixer;
    public GameObject settingsPanel;

    private void Awake() {
        settingsPanel.SetActive(false);
    }

    public void PlayButtonClicked() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetVolume() {
        float volume = volumeSlider.value;
        mixer.SetFloat("MusicVolume", volume);
        AudioManager.instance.SetMusicVolume(volume);
    }

}
