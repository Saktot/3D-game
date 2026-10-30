using System;
using UnityEngine;

namespace Amer{
    public class KillCounterController : MonoBehaviour{
        [Header("References")]
        [SerializeField] private KillCounterView killCounterView;

        public int KilledEnemies { get; private set; }
        public int TotalEnemies { get; private set; }

        public event Action OnAllEnemiesKilled;

        private void Awake(){
            if (killCounterView == null)
                Debug.LogError("KillCounterController: KillCounterView is not assigned.", this);
        }

        private void OnEnable(){
            EnemyHealth.OnAnyEnemyDied += HandleEnemyDied;
        }

        private void OnDisable(){
            EnemyHealth.OnAnyEnemyDied -= HandleEnemyDied;
        }

        private void Start(){
            TotalEnemies = FindObjectsByType<EnemyHealth>().Length;
            KilledEnemies = 0;

            UpdateView();
        }

        private void HandleEnemyDied(EnemyHealth enemy){
            KilledEnemies++;
            UpdateView();

            if (KilledEnemies >= TotalEnemies)
                OnAllEnemiesKilled?.Invoke();
        }

        private void UpdateView(){
            if (killCounterView != null)
                killCounterView.UpdateKills(KilledEnemies, TotalEnemies);
        }
    }
}
