using UnityEngine;

namespace Amer{
    public class EnemyAI : MonoBehaviour{
        // Состояния врага
        private enum EnemyState{
            Idle,
            Chase,
            Attack
        }

        [Header("Player")]
        [SerializeField] private Transform player;
        [SerializeField] private Transform eyePoint;

        [Header("Detection")]
        [SerializeField] private float detectionRadius = 6f;
        [SerializeField] private LayerMask obstacleMask;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 2.5f;

        [Header("Attack")]
        [SerializeField] private float attackRange = 1.5f;
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private float attackCooldown = 1.5f;

        private Rigidbody rb;
        private PlayerHealth playerHealth;

        private EnemyState state = EnemyState.Idle;
        private float nextAttackTime;

        private void Awake(){
            rb = GetComponent<Rigidbody>();

            if (rb == null){
                Debug.LogError("EnemyAI requires Rigidbody.", this);
                enabled = false;
                return;
            }

            if (player == null){
                GameObject playerObject =
                    GameObject.FindGameObjectWithTag("Player");

                if (playerObject != null)
                    player = playerObject.transform;
            }

            if (player != null)
                playerHealth = player.GetComponent<PlayerHealth>();
        }

        private void FixedUpdate(){
            // если игрока нет останавливается
            if (player == null ||
                playerHealth == null ||
                playerHealth.CurrentHp <= 0f){
                ChangeState(EnemyState.Idle);
                StopMovement();
                return;
            }

            bool canSeePlayer = CanSeePlayer();

            float distance = HorizontalDistance(
                player.position,rb.position);

            // Выполняем логику текущего состояния
            switch (state){
                case EnemyState.Idle:
                    StopMovement();

                    if (canSeePlayer){
                        if (distance <= attackRange)
                            ChangeState(EnemyState.Attack);
                        else
                            ChangeState(EnemyState.Chase);
                    }

                    break;

                case EnemyState.Chase:
                    if (!canSeePlayer){
                        ChangeState(EnemyState.Idle);
                        StopMovement();
                        break;
                    }

                    if (distance <= attackRange){
                        ChangeState(EnemyState.Attack);
                        StopMovement();
                        break;
                    }

                    MoveTowards(player.position);
                    break;

                case EnemyState.Attack:
                    StopMovement();

                    if (!canSeePlayer){
                        ChangeState(EnemyState.Idle);
                        break;
                    }

                    if (distance > attackRange){
                        ChangeState(EnemyState.Chase);
                        break;
                    }

                    TryAttack();
                    break;
            }
        }

        private void TryAttack(){
            // Проверяем перезарядку атаки
            if (Time.time < nextAttackTime)
                return;

            // Дополнительная проверка перед нанесением урона
            if (HorizontalDistance(player.position, rb.position)
                > attackRange || !CanSeePlayer())
                return;

            playerHealth.TakeDamage(attackDamage);

            nextAttackTime = Time.time + attackCooldown;
        }

        private void ChangeState(EnemyState newState){
            if (state == newState)
                return;
            state = newState;
        }

        private void MoveTowards(Vector3 target){
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

        private void StopMovement(){
            Vector3 velocity = rb.linearVelocity;
            velocity.x = 0f;
            velocity.z = 0f;

            rb.linearVelocity = velocity;
        }

        private float HorizontalDistance(Vector3 a, Vector3 b){
            a.y = 0f;
            b.y = 0f;

            return Vector3.Distance(a, b);
        }

        private bool CanSeePlayer(){
            Vector3 origin = eyePoint != null
                ? eyePoint.position
                : rb.position + Vector3.up * 0.5f;

            Vector3 target = player.position + Vector3.up * 0.5f;

            if (HorizontalDistance(player.position, rb.position)
                > detectionRadius)
                return false;

            Vector3 direction = target - origin;
            float distance = direction.magnitude;

            if (distance <= 0.01f)
                return true;

            // Через препятстсвие raycast не проходит
            return !Physics.Raycast(
                origin, direction.normalized, distance,
                obstacleMask, QueryTriggerInteraction.Ignore);
        }

        private void OnDrawGizmosSelected(){
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}

