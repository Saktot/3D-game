using UnityEngine;

namespace Amer{
    public class VictoryView : MonoBehaviour{
        [SerializeField] private GameObject victoryPanel;

        public void SetVictoryVisible(bool isVisible){
            if (victoryPanel != null)
                victoryPanel.SetActive(isVisible);
        }
    }
}
