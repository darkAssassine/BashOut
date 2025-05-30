using NeuronCrafter.NeuralNetwork;
using System.Collections;
using UnityEngine;

namespace BashOut.Player
{
    public class PlayerAI : PlayerInput
    {
        [SerializeField] private NeuralNetwork agent;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Player player;
        [SerializeField] private float TrainIntervalInSeconds = 30;
        [SerializeField] private float FitnessEarningPerAttack;
        [SerializeField] private float FitnessEarningPerEnviromentAttack;
        [SerializeField] private float FitnessLossPerDead;
        [SerializeField] private float FittnesEarningForMoving;
        [SerializeField] private float FittnesLosePerSecond;
        [SerializeField] private float FitnessLoseForFailedBlock;
        [SerializeField] private float FitnessEarnForSuccesfullBlock;
        [SerializeField] private float FitnessLoseForAttackMiss;
        [SerializeField] private float TrainThreshold;
        [SerializeField] private float rayDistance;
        [SerializeField] private LayerMask layer;

        private float timeStanding;
        private bool InDeadzone;
        private void Awake()
        {
            agent.Initialize("Bot1", player.Name);
        }

        private IEnumerator Start()
        {
            agent.Initialize("Bot1", player.Name);
            while (true)
            {
                yield return new WaitForSeconds(TrainIntervalInSeconds);
                agent.Train();
            }
        }

        void FixedUpdate()
        {
            agent.AddFitnees(-Mathf.Abs(FittnesLosePerSecond) * Time.deltaTime);

            if (Mathf.Abs(rb.linearVelocityX) > 0.5f && Mathf.Abs(rb.linearVelocityY) > 0.5f)
            {
                agent.AddFitnees(Mathf.Abs(FittnesEarningForMoving) * Time.deltaTime);
                timeStanding = 0;
            }
            else
            {
                timeStanding += Time.deltaTime;
                if (timeStanding > 1f)
                {
                    agent.Train();
                    timeStanding = 0;
                    return;
                }
            }
            Vector2 directionToNeareastEnemy = Vector2.zero;
            bool shouldBlock = false;

            Player nearestEnemy = PlayerManager.GetNearestEnemy(transform.position);
            if (nearestEnemy != null)
            {
                directionToNeareastEnemy = (nearestEnemy.transform.position - transform.position).normalized * 10;

                shouldBlock = (nearestEnemy.PlayerInput.Attack || nearestEnemy.CanAttack) || InDeadzone;
            }
            else
            {
                shouldBlock = InDeadzone;
            }

            float distance1 = RayCheck(new Vector2(1, -1));
            float distance2 = RayCheck(new Vector2(-1, 1));
            float distance3 = RayCheck(new Vector2(-1, -1));
            float distance4 = RayCheck(new Vector2(1, 1));
            float distance5 = RayCheck(new Vector2(1, 0));
            float distance6 = RayCheck(new Vector2(-1, 0));
            float distance7 = RayCheck(new Vector2(0, 1));
            float distance8 = RayCheck(new Vector2(0, -1));

            Vector2 velocity = rb.linearVelocity.normalized;

            float[] inputs =
            {
                velocity.x,
                velocity.y,
                directionToNeareastEnemy.x,
                directionToNeareastEnemy.y,
                distance1,
                distance2,
                distance3,
                distance4,
                distance5,
                distance6,
                distance7,
                distance8,
                shouldBlock ? 1 : 0,
            };


            float[] output = agent.Run(inputs);

            Jump = output[0] > 0;
            MoveDirection = new Vector2(output[1], output[2]);
            Attack = output[3] > 0;
            AttackDirection = new Vector2(output[4], output[5]);
            AttackDirection = RoundToNearestDirection(AttackDirection);
            Block = output[6] > 0;

            if (agent.FitnessValue < TrainThreshold)
            {
                agent.Train();
                timeStanding = 0;
                return;
            }
        }

        public void OnPlayerDied(Player _player)
        {
            agent.AddFitnees(-Mathf.Abs(FitnessLossPerDead));
            agent.Train();
        }

        public void OnPlayerAttacked()
        {
            agent.AddFitnees(Mathf.Abs(FitnessEarningPerAttack));
        }

        public void OnEnviromentAttacked()
        {
            agent.AddFitnees(Mathf.Abs(FitnessEarningPerEnviromentAttack));
        }

        private Vector2 RoundToNearestDirection(Vector2 input)
        {
            if (input.sqrMagnitude < 0.01f)
                return Vector2.zero;

            float x = Mathf.Round(Mathf.Clamp(input.normalized.x, -1f, 1f));
            float y = Mathf.Round(Mathf.Clamp(input.normalized.y, -1f, 1f));

            return new Vector2(x, y).normalized;
        }

        public float RayCheck(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, rayDistance, layer);
            if (hit.collider == null)
            {
                return 1;
            }
            else
            {
                return hit.distance / rayDistance;
            }
        }

        public void OnDeadzoneEntered()
        {
            InDeadzone = true;
        }

        public void OnDeadzoneExited()
        {
            InDeadzone = false;
        }

        public void PlayerGotAttacked()
        {
            if (player.IsBlocking)
            {
                agent.AddFitnees(Mathf.Abs(FitnessEarnForSuccesfullBlock));
            }
            else
            {
                agent.AddFitnees(-Mathf.Abs(FitnessLoseForFailedBlock));
            }
        }

        public void PlayerAttacked(bool attackLanded)
        {
            if (attackLanded == false)
            {
                agent.AddFitnees(-Mathf.Abs(FitnessLoseForAttackMiss));
            }
        }
    }
}
