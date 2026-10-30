
using UnityEngine;

namespace Amer{
    public class HealthController : MonoBehaviour{
        [Header("References")]
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private HealthView healthView;

        private void OnEnable(){
            if (playerHealth == null){
                Debug.LogError(
                    "HealthController: PlayerHealth is not assigned.", this);
                return;
            }

            playerHealth.OnHealthChanged += HandleHealthChanged;

            HandleHealthChanged(
                playerHealth.CurrentHp,
                playerHealth.MaxHp
            );
        }

        private void OnDisable(){
            if (playerHealth != null)
                playerHealth.OnHealthChanged -= HandleHealthChanged;
        }

        private void HandleHealthChanged(float currentHealth, float maxHealth){
            if (healthView != null)
                healthView.UpdateHealth(currentHealth, maxHealth);
        }
    }
}

