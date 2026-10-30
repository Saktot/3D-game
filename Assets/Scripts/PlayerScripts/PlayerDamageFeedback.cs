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
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

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
                originalColors[i] = GetRendererColor(playerRenderers[i]);

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
            yield return new WaitForSecondsRealtime(flashDuration);

            RestoreColors();
            flashRoutine = null;
        }

        private void SetColor(Color color){
            foreach (Renderer rend in playerRenderers) {
                if (rend != null)
                    SetRendererColor(rend, color);
            }
        }

        private void RestoreColors(){
            if (playerRenderers == null || originalColors == null)
                return;

            for (int i = 0; i < playerRenderers.Length; i++) {
                if (playerRenderers[i] != null)
                    SetRendererColor(playerRenderers[i], originalColors[i]);
            }
        }

        private Color GetRendererColor(Renderer rend){
            if (rend.material.HasProperty(BaseColorId))
                return rend.material.GetColor(BaseColorId);

            return rend.material.color;
        }

        private void SetRendererColor(Renderer rend, Color color){
            if (rend.material.HasProperty(BaseColorId))
                rend.material.SetColor(BaseColorId, color);
            else
                rend.material.color = color;
        }
    }
}