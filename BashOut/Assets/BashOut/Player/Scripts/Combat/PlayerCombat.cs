using UnityEngine;
using UnityEngine.Events;

namespace BashOut.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        public Vector2 LastAttackDir { get; private set; }

        [SerializeField] private Player player;
        [SerializeField] private PlayerInput input;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform attack;
        [SerializeField] private float attackCooldown;
        [SerializeField] private float blockDuration;
        [SerializeField] private float blockCooldown;

        private float currentAttackCooldown;
        private float currentBlockCooldown;
        private float currentBlockDuration;
        public Vector2 AttackDir { get; private set; }

        [HideInInspector] public bool PlayerHitSomething;

        private bool canBlock = true;
        private bool canAttack = true;
        [SerializeField] private UnityEvent<bool> hitResult;

        private void Awake()
        {
            LastAttackDir = new Vector2(1, 0).normalized;

            canBlock = true;
            canAttack = true;
        }
        void Update()
        {
            TryAttack();
            TryBlock();

            currentAttackCooldown -= Time.deltaTime;
            currentBlockCooldown -= Time.deltaTime;
            currentBlockDuration -= Time.deltaTime;

            if (currentBlockDuration < 0)
            {
                player.IsBlocking = false;
            }
            if (input.AttackDirection.magnitude > 0.01f)
            {
                LastAttackDir = input.AttackDirection;
            }
        }

        private void TryBlock()
        {
            if (player.CanBlock == false || canBlock == false)
            {
                return;
            }

            if (input.Block && currentBlockCooldown < 0)
            {
                player.IsBlocking = true;
                canAttack = false;
                currentBlockDuration = blockDuration;
                currentBlockCooldown = blockCooldown;
                animator.Play("Block", 0);
            }
        }

        private void TryAttack()
        {
            if (player.CanAttack == false || canAttack == false)
            {
                return;
            }

            if (input.Attack && currentAttackCooldown < 0)
            {
                PlayerHitSomething = false;
                currentAttackCooldown = attackCooldown;

                Vector2 direction = input.AttackDirection;

                if (direction.sqrMagnitude < 0.001f)
                {
                    direction = LastAttackDir;
                }
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                attack.rotation = Quaternion.Euler(0f, 0f, angle);
                animator.Play("Attack", 0);
                canBlock = false;
                AttackDir = direction;
            }
        }

        public void AttackFinished()
        {
            canBlock = true;
            hitResult?.Invoke(PlayerHitSomething);
        }

        public void BlockFinished()
        {
            canAttack = true;
        }
    }
}
