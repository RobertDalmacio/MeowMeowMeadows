using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    public float currentBGMVolume = 0f;

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetMusicVolume(float volume) {
        currentBGMVolume = volume;
    }

}
