using UnityEngine;

namespace BashOut.Player
{
    public class PlayerMovementStateOnGound : PlayerMovementState
    {
        [SerializeField] private float jumpForce;
        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            CheckForTransition();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }

        private void CheckForTransition()
        {
            if (stateMachine.IsGrounded == false)
            {
                stateMachine.ChangeState(stateMachine.InAirState);
            }
            else if (stateMachine.OnWall)
            {
                stateMachine.ChangeState(stateMachine.OnWallState);
            }
        }

        protected override void Jump()
        {
            rb.linearVelocityY = 0;
            rb.AddForceY(jumpForce, ForceMode2D.Impulse);
        }
    }
}
