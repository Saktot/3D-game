using UnityEngine;

namespace Amer
{
    public class PauseModel
    {
        public bool IsPaused { get; private set; }

        public void SetPaused(bool paused){
            IsPaused = paused;
        }

        void Start()
        {
        
        }

        void Update()
        {
        
        }
    }
}
