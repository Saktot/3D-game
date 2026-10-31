using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Amer
{
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Attack")]
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private float attackCooldown = 1f;
        [SerializeField] private float attackDistance = 1f;
        [SerializeField] private float attackRadius = 0.7f;
        [SerializeField] private float hitDelay = 0.2f;

        [Header("Combo")]
        [SerializeField] private int comboLength = 3;
        [SerializeField] private float comboResetTime = 1.5f;

        [Header("Sound")]
        [SerializeField] private AudioSource combatAudioSource;
        [SerializeField] private AudioClip[] attackSounds;
        [SerializeField] private AudioClip[] hitSounds;

        [Header("Enemy")]
        [SerializeField] private LayerMask enemyMask;

        private float nextAttackTime;
        private float lastAttackTime;
        private int comboStep;

        private PlayerHealth playerHealth;
        private PlayerAnimation playerAnimation;

        private void Awake(){
            playerHealth = GetComponent<PlayerHealth>();
            playerAnimation = GetComponent<PlayerAnimation>();
        }

        private void Update(){
            if (Time.timeScale <= 0f)
                return;

            if (playerHealth != null &&
                playerHealth.CurrentHp <= 0f)
                return;

            if (Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame){
                Attack();
            }
        }

        private void Attack(){
            if (Time.time < nextAttackTime)
                return;

            nextAttackTime = Time.time + attackCooldown;

            if (Time.time - lastAttackTime > comboResetTime ||
                comboStep >= comboLength){
                comboStep = 0;
            }

            comboStep++;
            lastAttackTime = Time.time;

            if (playerAnimation != null)
                playerAnimation.PlayAttack(comboStep);

            PlayAttackSound();

            StartCoroutine(HitAfterDelay());
        }

        private IEnumerator HitAfterDelay() {
            yield return new WaitForSeconds(hitDelay);

            if (playerHealth != null && playerHealth.IsDead)
                yield break;

            DealDamage();
        }

        private void DealDamage()
        {
            Vector3 attackPosition =
                transform.position +
                transform.forward * attackDistance +
                Vector3.up * 0.5f;

            Collider[] hits = Physics.OverlapSphere(
                attackPosition,attackRadius,enemyMask);

            foreach (Collider hit in hits){
                EnemyHealth enemy =
                    hit.GetComponentInParent<EnemyHealth>();

                if (enemy != null){
                    enemy.TakeDamage(attackDamage);
                    PlayHitSound();
                    break;
                }
            }
        }

        private void PlayAttackSound(){
            if (combatAudioSource == null ||
                attackSounds == null ||
                attackSounds.Length == 0)
                return;

            int index = Mathf.Clamp(comboStep - 1,0,attackSounds.Length - 1);

            AudioClip clip = attackSounds[index];

            if (clip != null)
                combatAudioSource.PlayOneShot(clip);
        }

        private void PlayHitSound(){
            if (combatAudioSource == null ||
                hitSounds == null ||hitSounds.Length == 0)
                return;

            int index = Random.Range(0, hitSounds.Length);

            AudioClip clip = hitSounds[index];

            if (clip != null)
                combatAudioSource.PlayOneShot(clip);
        }

        private void OnDrawGizmosSelected(){
            Vector3 attackPosition =transform.position +
                transform.forward * attackDistance +
                Vector3.up * 0.5f;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPosition,attackRadius);
        }
    }
}
