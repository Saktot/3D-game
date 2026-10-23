using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Amer
{
    public class HealthView : MonoBehaviour{
        [SerializeField] private PlayerHealth playerHp;
        [SerializeField] private Image hpFill;
        [SerializeField] private TMP_Text hpText;

        private void OnEnable(){
            if (playerHp == null)
                return;

            playerHp.OnHealthChanged += UpdateHealth;

            // актуальное здоровье
            UpdateHealth(playerHp.CurrentHp, playerHp.MaxHp);
        }

        private void OnDisable(){
            if (playerHp != null)
                playerHp.OnHealthChanged -= UpdateHealth;
        }

        private void UpdateHealth(float currentHealth, float maxHealth){
            hpFill.fillAmount =
                maxHealth > 0 ? currentHealth / maxHealth : 0f;

            hpText.text =
                $"{Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";
        }
    }
}
