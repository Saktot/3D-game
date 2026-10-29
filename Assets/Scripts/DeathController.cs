
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Amer{
    public class DeathController : MonoBehaviour{
        [Header("References")]
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private DeathView deathView;

        public bool IsDead { get; private set; }

        private void Awake(){
            IsDead = false;

            if (deathView != null)
                deathView.SetDeathVisible(false);
            else
                Debug.LogError("DeathController: DeathView is not assigned.", this);
        }

        private void OnEnable(){
            if (playerHealth != null)
                playerHealth.OnDied += HandlePlayerDied;
        }

        private void OnDisable() {
            if (playerHealth != null)
                playerHealth.OnDied -= HandlePlayerDied;
        }

        private void HandlePlayerDied(){
            if (IsDead)
                return;

            IsDead = true;

            Time.timeScale = 0f;

            if (deathView != null)
                deathView.SetDeathVisible(true);
        }

        public void RestartLevel() {
            Time.timeScale = 1f;

            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }
    }
}
