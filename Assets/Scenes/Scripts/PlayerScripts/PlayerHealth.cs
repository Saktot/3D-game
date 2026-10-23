using UnityEngine;
using System;

namespace Amer
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private float maxHp = 100f;
        [SerializeField] private float currentHp;

        public float MaxHp => maxHp;
        public float CurrentHp => currentHp;

        public event Action<float, float> OnHealthChanged;
        public event Action OnDied;

        private bool _isDead;

        private void Awake() {
            maxHp = Mathf.Max(1f, maxHp);
            currentHp = maxHp;
        }

        public void TakeDamage(float damage) {
            if (_isDead || damage <= 0f)
                return;

            currentHp = Mathf.Max(0f, currentHp - damage);

            OnHealthChanged?.Invoke(currentHp, maxHp);

            if (currentHp <= 0f) {
                _isDead = true;
                OnDied?.Invoke();
            }
        }

        public void Heal(float amount){
            if (_isDead || amount <= 0f)
                return;

            currentHp = Mathf.Min(maxHp, currentHp + amount);

            OnHealthChanged?.Invoke(currentHp, maxHp);
        }
    }
}
