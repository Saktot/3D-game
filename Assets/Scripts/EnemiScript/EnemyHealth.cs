using UnityEngine;

namespace Amer{
    public class EnemyHealth : MonoBehaviour{
        [Header("Health")]
        [SerializeField, Min(1f)]
        private float maxHp = 30f;

        private float currentHp;

        public float CurrentHp => currentHp;

        private void Awake(){
            currentHp = maxHp;
        }

        public void TakeDamage(float damage) {
            // Мёртвого врага нельзя повредить повторно
            if (currentHp <= 0f || damage <= 0f)
                return;

            currentHp = Mathf.Max(0f, currentHp - damage);

            Debug.Log($"{name}: HP = {currentHp}/{maxHp}");

            if (currentHp <= 0f)
                Die();
        }

        private void Die(){
            Destroy(gameObject);
        }
    }
}
