using UnityEngine;

namespace BashOut.Player
{
    public class PlayerMovementStateMachine : StateMachine.StateMachine
    {
        public bool CanJump { get; private set; }
        public bool CanDodge { get; private set; }
        public bool OnLeftWall { get; private set; }
        public bool OnRightWall { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool OnWall { get { return OnLeftWall || OnRightWall; } }

        public PlayerMovementStateOnGound GroundState;
        public PlayerMovementStateInAir InAirState;
        public PlayerMovementStateOnWall OnWallState;

        [SerializeField] private Player player;

        [SerializeField, Header("Shared Stats")] private float jumpCooldown;
        [SerializeField] private float jumpCount;
        [SerializeField] private float dodgeCooldown;
        [SerializeField] public float invincibleDuration;

        private float currentJumpCount;
        private float currentJumpCooldown;
        private float currentDodgeCooldown;
        public float InvincibleDuration { get { return invincibleDuration; } }
        private void Awake()
        {
            currentJumpCount = jumpCount;
        }

        private void Update()
        {
            currentJumpCooldown -= Time.deltaTime;
            currentDodgeCooldown -= Time.deltaTime;

            if (currentJumpCooldown < 0 && currentJumpCount > 0)
            {
                CanJump = true;
            }

            if (currentDodgeCooldown < 0)
            {
                CanDodge = true;
            }
            if (currentState != null)
            {
                if (player.CanMove)
                {
                    currentState.UpdateLogic();
                }
            }
        }

        private void FixedUpdate()
        {
            if (currentState != null)
            {
                if (player.CanMove)
                {
                    currentState.UpdatePhysics();
                }
            }
        }

        public void Jumped()
        {
            currentJumpCount--;
            currentJumpCooldown = jumpCooldown;
            CanJump = false;
        }

        public void Dodged()
        {
            CanDodge = false;
            currentDodgeCooldown = dodgeCooldown;
        }

        public void SetIsOnLeftWall(bool _isOnLeftWall)
        {
            OnLeftWall = _isOnLeftWall;
        }

        public void SetIsOnRightWall(bool _isOnRightWall)
        {
            OnRightWall = _isOnRightWall;
        }

        public void SetOnGround(bool _isOnGround)
        {
            if (_isOnGround)
            {
                currentJumpCount = jumpCount;
                IsGrounded = true;
            }
            else
            {
                IsGrounded = false;
            }
        }
    }
}
