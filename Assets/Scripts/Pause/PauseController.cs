using UnityEngine;
using UnityEngine.InputSystem;

namespace Amer{
    public class PauseController : MonoBehaviour{
        [Header("References")]
        [SerializeField] private PauseModel pauseModel;
        [SerializeField] private PauseView pauseView;
        [SerializeField] private DeathController deathController;

        private void Awake(){
            Time.timeScale = 1f;

            if (pauseModel == null)
                Debug.LogError("PauseController: PauseModel is not assigned.", this);

            if (pauseView == null)
                Debug.LogError("PauseController: PauseView is not assigned.", this);

            if (pauseView != null)
                pauseView.SetPauseVisible(false);
        }

        private void OnEnable(){
            if (pauseModel != null)
                pauseModel.OnPauseChanged += HandlePauseChanged;
        }

        private void OnDisable() {
            if (pauseModel != null)
                pauseModel.OnPauseChanged -= HandlePauseChanged;

            Time.timeScale = 1f;
        }

        private void Update(){
            if (Keyboard.current != null &&
                Keyboard.current.escapeKey.wasPressedThisFrame){
                TogglePause();
            }
        }

        public void TogglePause() {
            if (deathController != null && deathController.IsDead)
                return;
            if (pauseModel == null)
                return;

            pauseModel.SetPaused(!pauseModel.IsPaused);
        }

        public void ResumeGame(){
            if (deathController != null && deathController.IsDead)
                return;
            if (pauseModel == null)
                return;

            pauseModel.SetPaused(false);
        }

        private void HandlePauseChanged(bool isPaused) {
            Time.timeScale = isPaused ? 0f : 1f;

            if (pauseView != null)
                pauseView.SetPauseVisible(isPaused);
        }
    }
}

