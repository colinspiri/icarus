using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour {
    // components
    private Slider _slider;
    public AudioSettings audioSettings;

    // constants
    public string mixerChannel;

    private void Awake() {
        _slider = GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start() {
        _slider.onValueChanged.AddListener(value => {
            audioSettings.ChangeVolume(mixerChannel, value);
        });
    }

    public void PlaySFXTestSound() {
        // play SFX sound to test SFX slider
    }

    private void OnEnable() {
        _slider.value = audioSettings.GetVolume(mixerChannel);
    }
}