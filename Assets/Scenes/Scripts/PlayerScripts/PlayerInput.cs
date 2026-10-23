using UnityEngine;

namespace Amer
{
    public class PlayerInput : MonoBehaviour
    {
        public float InputAxisX;
        public float InputAxisZ;
        void Start()
        {
        
        }

        void Update()
        {
            InputAxisX = Input.GetAxisRaw("Horizontal");
            InputAxisZ = Input.GetAxisRaw("Vertical");
        }
    }
}
