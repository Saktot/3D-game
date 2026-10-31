using UnityEngine;

namespace Amer{
    // Настройки игры: сейчас только общая громкость звука.
    public class SettingsController : MonoBehaviour{
        [Header("References")]
        [SerializeField] private SettingsView settingsView;

        // Ключ, под которым громкость сохраняется между запусками игры.
        private const string VolumeKey = "MasterVolume";

        private void Awake(){
            if (settingsView == null){
                Debug.LogError("SettingsController: SettingsView is not assigned.", this);
                return;
            }

            settingsView.SetSettingsVisible(false);

            // Загружаем сохранённую громкость (если её нет - 100%).
            float volume = PlayerPrefs.GetFloat(VolumeKey, 1f);
            ApplyVolume(volume);
            settingsView.SetVolume(volume);
        }

        private void OnEnable(){
            if (settingsView != null)
                settingsView.OnVolumeChanged += HandleVolumeChanged;
        }

        private void OnDisable(){
            if (settingsView != null)
                settingsView.OnVolumeChanged -= HandleVolumeChanged;
        }

        public void OpenSettings(){
            if (settingsView != null)
                settingsView.SetSettingsVisible(true);
        }

        // Кнопка "Назад" в окне настроек.
        public void CloseSettings(){
            if (settingsView != null)
                settingsView.SetSettingsVisible(false);
        }

        private void HandleVolumeChanged(float volume){
            ApplyVolume(volume);

            PlayerPrefs.SetFloat(VolumeKey, volume);
            PlayerPrefs.Save();
        }

        private void ApplyVolume(float volume){
            // AudioListener.volume - общая громкость всех звуков в игре (от 0 до 1).
            AudioListener.volume = Mathf.Clamp01(volume);
        }
    }
}
