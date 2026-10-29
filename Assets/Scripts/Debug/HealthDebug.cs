using UnityEngine;

namespace Amer
{
    public class HealthDebug : MonoBehaviour
    {
        [SerializeField] private PlayerHealth hp;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
                hp.TakeDamage(10f);

            if (Input.GetKeyDown(KeyCode.J))
                hp.Heal(10f);
        }
    }
}
