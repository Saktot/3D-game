
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Amer{
    public class HealthView : MonoBehaviour{
        [SerializeField] private Image hpFill;
        [SerializeField] private TMP_Text hpText;

        public void UpdateHealth(float currentHealth, float maxHealth){
            if (hpFill != null){
                hpFill.fillAmount =
                    maxHealth > 0f ? currentHealth / maxHealth : 0f;
            }

            if (hpText != null){
                hpText.text =
                    $"{Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";
            }
        }
    }
}

