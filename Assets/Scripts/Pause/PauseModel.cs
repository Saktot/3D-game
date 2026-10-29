using UnityEngine;
using System;

namespace Amer{
    public class PauseModel : MonoBehaviour{
        public bool IsPaused { get; private set; }

        public event Action<bool> OnPauseChanged;

        public void SetPaused(bool isPaused){
            if (IsPaused == isPaused)
                return;

            IsPaused = isPaused;
            OnPauseChanged?.Invoke(IsPaused);
        }
    }
}
