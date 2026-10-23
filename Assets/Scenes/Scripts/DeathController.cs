using UnityEngine;
using UnityEngine.SceneManagement;

namespace Amer
{
    public class DeathController : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private GameObject deathPanel;

        private void Awake() {
            deathPanel.SetActive(false);
        }

        private void OnEnable() {
            if (playerHealth != null)
                playerHealth.OnDied += ShowDeathScreen;
        }

        private void OnDisable() {
            if (playerHealth != null)
                playerHealth.OnDied -= ShowDeathScreen;
        }

        private void ShowDeathScreen() {
            deathPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        public void RestartLevel(){
            Time.timeScale = 1f;

            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }
    }
}
