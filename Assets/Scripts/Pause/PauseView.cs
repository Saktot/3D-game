using UnityEngine;

namespace Amer{
    public class PauseView : MonoBehaviour{
        [SerializeField] private GameObject pausePanel;

        public void SetPauseVisible(bool isVisible) {
            if (pausePanel != null)
                pausePanel.SetActive(isVisible);
        }
    }
}
