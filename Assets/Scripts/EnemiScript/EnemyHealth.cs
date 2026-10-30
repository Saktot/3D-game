using UnityEngine;
using System;

namespace Amer{
    public class EnemyHealth : MonoBehaviour{
        [Header("Health")]
        [SerializeField, Min(1f)]
        private float maxHp = 30f;

        private float currentHp;

        public float CurrentHp => currentHp;

        // Общее событие для всех врагов: "какой-то враг умер".
        // static - значит, подписаться можно, не зная конкретного врага (так делает счётчик убийств).
        public static event Action<EnemyHealth> OnAnyEnemyDied;

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
            OnAnyEnemyDied?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
