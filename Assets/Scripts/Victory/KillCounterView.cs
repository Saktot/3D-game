using UnityEngine;
using TMPro;

namespace Amer{
    // Показывает сколько врагов убито.
    public class KillCounterView : MonoBehaviour{
        [SerializeField] private TMP_Text killText;

        public void UpdateKills(int killed, int total){
            if (killText != null)
                killText.text = $"Убито: {killed} / {total}";
        }
    }
}
