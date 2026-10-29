using UnityEngine;

namespace Amer{
    public class DeathView : MonoBehaviour{
        [SerializeField] private GameObject deathPanel;

        public void SetDeathVisible(bool isVisible){
            if (deathPanel != null)
                deathPanel.SetActive(isVisible);
        }
    }
}

