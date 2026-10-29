using UnityEngine;
using System.Collections;
namespace Amer{
    public class PlayerDamageFeedback : MonoBehaviour{
        [Header("References")]
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private Renderer[] playerRenderers;

        [Header("Damage Effect")]
        [SerializeField] private Color damageColor = Color.red;
        [SerializeField] private float flashDuration = 0.12f;

        private Color[] originalColors;
        private Coroutine flashRoutine;
        private float previousHealth;

        private void Awake() {
            if (playerHealth == null)
                playerHealth = GetComponent<PlayerHealth>();

            if (playerRenderers == null || playerRenderers.Length == 0)
                playerRenderers = GetComponentsInChildren<Renderer>();

            originalColors = new Color[playerRenderers.Length];

            for (int i = 0; i < playerRenderers.Length; i++)
                originalColors[i] = playerRenderers[i].material.color;

            if (playerHealth != null)
                previousHealth = playerHealth.CurrentHp;
        }

        private void OnEnable(){
            if (playerHealth != null)
                playerHealth.OnHealthChanged += OnHealthChanged;
        }

        private void OnDisable() {
            if (playerHealth != null)
                playerHealth.OnHealthChanged -= OnHealthChanged;

            RestoreColors();
        }

        private void OnHealthChanged(float currentHealth, float maxHealth){
            if (currentHealth < previousHealth)
                PlayDamageEffect();

            previousHealth = currentHealth;
        }

        private void PlayDamageEffect(){
            if (flashRoutine != null)
                StopCoroutine(flashRoutine);

            flashRoutine = StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine() {
            SetColor(damageColor);

            // Реальное время позволяет закончить эффект,
            // даже если игра остановилась после смерти.
            yield return new WaitForSecondsRealtime(flashDuration);

            RestoreColors();
            flashRoutine = null;
        }

        private void SetColor(Color color){
            foreach (Renderer rend in playerRenderers) {
                if (rend != null)
                    rend.material.color = color;
            }
        }

        private void RestoreColors(){
            if (playerRenderers == null || originalColors == null)
                return;

            for (int i = 0; i < playerRenderers.Length; i++) {
                if (playerRenderers[i] != null)
                    playerRenderers[i].material.color = originalColors[i];
            }
        }
    }
}