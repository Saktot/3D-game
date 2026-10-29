
using UnityEngine;

namespace Amer{
    public class EnemyAI : MonoBehaviour{
        private enum EnemyState{
            Idle,
            Chase,
            Investigate,
            Windup,
            Recovery
        }

        [Header("Player")]
        [SerializeField] private Transform player;
        [SerializeField] private Transform eyePoint;

        [Header("Detection")]
        [SerializeField] private float detectionRadius = 6f;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private float searchDuration = 2f;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 2.5f;
        [SerializeField] private float stoppingDistance = 1.2f;

        [Header("Attack")]
        [SerializeField] private float attackRange = 1.5f;
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private float attackWindup = 0.4f;
        [SerializeField] private float recoveryDuration = 0.5f;

        private Rigidbody rb;
        private PlayerHealth playerHealth;

        private EnemyState state = EnemyState.Idle;
        private Vector3 lastSeenPosition;

        private float lostSightTime;
        private float stateTimer;
        private float nextAttackTime;

        private void Awake(){
            rb = GetComponent<Rigidbody>();

            if (player == null){
                GameObject playerObject =
                    GameObject.FindGameObjectWithTag("Player");

                if (playerObject != null)
                    player = playerObject.transform;
            }

            if (player != null)
                playerHealth = player.GetComponent<PlayerHealth>();
        }

        private void FixedUpdate() {
            if (player == null || playerHealth == null) {
                StopMovement();
                return;
            }

            if (playerHealth.CurrentHp <= 0) {
                StopMovement();
                return;
            }

            bool inRange =
                HorizontalDistance(player.position, rb.position)
                <= detectionRadius;

            bool canSee = inRange && CanSeePlayer();

            if (canSee){
                lastSeenPosition = player.position;
                lostSightTime = 0f;
            }
            else {
                lostSightTime += Time.fixedDeltaTime;
            }

            // Обработка подготовки к удару.
            if (state == EnemyState.Windup) {
                StopMovement();

                // Игрок успел уйти или скрылся за стеной.
                if (!canSee ||
                    HorizontalDistance(player.position, rb.position)
                    > attackRange){
                    state = canSee
                        ? EnemyState.Chase
                        : EnemyState.Investigate;

                    return;
                }

                stateTimer -= Time.fixedDeltaTime;

                if (stateTimer <= 0f) {
                    AttackPlayer();
                    state = EnemyState.Recovery;
                    stateTimer = recoveryDuration;
                    nextAttackTime = Time.time + attackCooldown;
                }

                return;
            }

            // Восстановление после удара.
            if (state == EnemyState.Recovery) {
                StopMovement();
                stateTimer -= Time.fixedDeltaTime;

                if (stateTimer <= 0f){
                    state = canSee
                        ? EnemyState.Chase
                        : EnemyState.Investigate;
                }

                return;
            }

            if (canSee){
                float distance =
                    HorizontalDistance(player.position, rb.position);

                if (distance <= attackRange &&
                    Time.time >= nextAttackTime) {
                    state = EnemyState.Windup;
                    stateTimer = attackWindup;
                    StopMovement();
                    return;
                }

                state = EnemyState.Chase;

                if (distance > stoppingDistance)
                    MoveTowards(player.position);
                else
                    StopMovement();

                return;
            }

            // проверяем последнее место player
            if (lostSightTime < searchDuration &&
                state != EnemyState.Idle){
                state = EnemyState.Investigate;

                if (HorizontalDistance(lastSeenPosition, rb.position)
                    > stoppingDistance) {
                    MoveTowards(lastSeenPosition);
                }
                else {
                    StopMovement();
                }
            }
            else{
                state = EnemyState.Idle;
                StopMovement();
            }
        }

        private void AttackPlayer() {
            if (playerHealth == null)
                return;

            // Повторная проверка перед нанесением урона.
            if (HorizontalDistance(player.position, rb.position)
                > attackRange || !CanSeePlayer()) {
                return;
            }

            playerHealth.TakeDamage(attackDamage);
        }

        private void MoveTowards(Vector3 target)  {
            Vector3 direction = target - rb.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.01f){
                StopMovement();
                return;
            }

            direction.Normalize();

            Vector3 velocity = rb.linearVelocity;
            velocity.x = direction.x * moveSpeed;
            velocity.z = direction.z * moveSpeed;

            rb.linearVelocity = velocity;
        }

        private void StopMovement() {
            if (rb == null)
                return;

            Vector3 velocity = rb.linearVelocity;
            velocity.x = 0f;
            velocity.z = 0f;
            rb.linearVelocity = velocity;
        }

        private float HorizontalDistance(Vector3 a, Vector3 b) {
            a.y = 0f;
            b.y = 0f;
            return Vector3.Distance(a, b);
        }

        private bool CanSeePlayer() {
            Vector3 origin = eyePoint != null
                ? eyePoint.position
                : rb.position + Vector3.up * 0.5f;

            Vector3 target = player.position;
            Vector3 direction = target - origin;
            float distance = direction.magnitude;

            if (distance <= 0.01f)
                return true;

            return !Physics.Raycast(
                origin,
                direction.normalized,
                distance,
                obstacleMask,
                QueryTriggerInteraction.Ignore
            );
        }

        private void OnDrawGizmosSelected(){
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}

