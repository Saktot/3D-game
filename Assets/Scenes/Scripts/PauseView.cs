using UnityEngine;

namespace Amer
{
    public class PauseView : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;

        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public void Show(){
            pausePanel.SetActive(true);
        }

        public void Hide(){
            pausePanel.SetActive(false);
        }
    }
}
