using UnityEngine;
using UnityEngine.SceneManagement;

namespace Amer{
    public class VictoryController : MonoBehaviour{
        [Header("References")]
        [SerializeField] private KillCounterController killCounterController;
        [SerializeField] private VictoryView victoryView;

        public bool IsVictory { get; private set; }

        private void Awake(){
            IsVictory = false;

            if (victoryView != null)
                victoryView.SetVictoryVisible(false);
            else
                Debug.LogError("VictoryController: VictoryView is not assigned.", this);
        }

        private void OnEnable(){
            if (killCounterController != null)
                killCounterController.OnAllEnemiesKilled += HandleAllEnemiesKilled;
        }

        private void OnDisable(){
            if (killCounterController != null)
                killCounterController.OnAllEnemiesKilled -= HandleAllEnemiesKilled;
        }

        private void HandleAllEnemiesKilled(){
            if (IsVictory)
                return;
            IsVictory = true;
            Time.timeScale = 0f;
            if (victoryView != null)
                victoryView.SetVictoryVisible(true);
        }

        public void RestartLevel(){
            Time.timeScale = 1f;

            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }

        // Кнопка "Выйти" в меню победы.
        public void QuitGame(){
            GameQuit.Quit();
        }
    }
}
