using UnityEngine;

namespace Amer
{
    public class PauseController : MonoBehaviour{
        [SerializeField] private PauseView pauseView;

        private PauseModel model;

        private void Awake(){
            model = new PauseModel();

            Time.timeScale = 1f;
            pauseView.Hide();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)){
                TogglePause();
            }
        }

        public void TogglePause(){
            SetPaused(!model.IsPaused);
        }

        public void Resume(){
            SetPaused(false);
        }

        private void SetPaused(bool paused) {
            model.SetPaused(paused);
            Time.timeScale = paused ? 0f : 1f;

            if (paused)
                pauseView.Show();
            else
                pauseView.Hide();
        }

        public void Quit() {
            Time.timeScale = 1f;
            Application.Quit();
        }
    }
}
