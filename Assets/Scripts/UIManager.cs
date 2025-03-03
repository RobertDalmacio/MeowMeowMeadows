using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Sprite[] toggleSprites;
    public Image musicToggle;
    AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void SpriteChange() {
        if (musicToggle.sprite == toggleSprites[0]) { // turn off
            musicToggle.sprite = toggleSprites[1];
            return;
        }
        musicToggle.sprite = toggleSprites[0]; // turn on
    }

    public void OnMusicToggleButtonClick() {
        SpriteChange();
        if (!audioSource.isPlaying) {
            audioSource.Play();
        }
        else {
            audioSource.Pause();
        }

    }
}
