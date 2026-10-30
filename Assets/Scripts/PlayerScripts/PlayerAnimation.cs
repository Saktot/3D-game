using UnityEngine;

namespace Amer{
    public class PlayerAnimation : MonoBehaviour{
        [Header("References")]
        [SerializeField] private Animator animator;

        [Header("Animation")]
        [SerializeField] private float animationSmooth = 0.1f;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int AttackIndexHash = Animator.StringToHash("AttackIndex");

        private Rigidbody rb;

        private void Awake(){
            rb = GetComponent<Rigidbody>();
            if (animator == null)
                animator = GetComponentInChildren<Animator>();
        }

        private void Update(){
            if (animator == null || rb == null)
                return;
            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0f;

            float speed = velocity.magnitude;

            animator.SetFloat(SpeedHash, speed, animationSmooth, Time.deltaTime);
        }

        public void PlayAttack(int attackIndex){
            if (animator == null)
                return;

            animator.SetInteger(AttackIndexHash, attackIndex);
            animator.SetTrigger(AttackHash);
        }
    }
}
