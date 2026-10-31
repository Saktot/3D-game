using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Amer{
    public class SettingsView : MonoBehaviour{
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private TMP_Text volumeText;

        // Игрок подвинул ползунок громкости.
        public event Action<float> OnVolumeChanged;

        private void OnEnable(){
            if (volumeSlider != null)
                volumeSlider.onValueChanged.AddListener(HandleSliderChanged);
        }

        private void OnDisable(){
            if (volumeSlider != null)
                volumeSlider.onValueChanged.RemoveListener(HandleSliderChanged);
        }

        public void SetSettingsVisible(bool isVisible){
            if (settingsPanel != null)
                settingsPanel.SetActive(isVisible);
        }

        // Ставит ползунок в нужное положение, не вызывая OnVolumeChanged.
        public void SetVolume(float volume){
            if (volumeSlider != null)
                volumeSlider.SetValueWithoutNotify(volume);

            UpdateVolumeText(volume);
        }

        private void HandleSliderChanged(float value){
            UpdateVolumeText(value);
            OnVolumeChanged?.Invoke(value);
        }

        private void UpdateVolumeText(float volume){
            if (volumeText != null)
                volumeText.text = $"Громкость: {Mathf.RoundToInt(volume * 100f)}%";
        }
    }
}
