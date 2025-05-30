using UnityEngine;

namespace BashOut.Player
{
    public class PlayerMovementStateOnWall : PlayerMovementState
    {
        [SerializeField] private Vector2 WallJumpForce;
        public override void Enter()
        {
            base.Enter();
            if (rb.linearVelocityY < 0)
            {
                rb.linearVelocity = Vector2.zero;
            }
            rb.linearVelocityX = 0;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            CheckForTransition();

            if (stateMachine.OnRightWall)
            {
                if (rb.linearVelocityX > 0)
                {
                    rb.linearVelocityX = 0;
                }
            }
            else
            {
                if (rb.linearVelocityX < 0)
                {
                    rb.linearVelocityX = 0;
                }
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            if (stateMachine.OnRightWall)
            {
                if (rb.linearVelocityX > 0)
                {
                    rb.linearVelocityX = 0;
                }
            }
            else
            {
                if (rb.linearVelocityX < 0)
                {
                    rb.linearVelocityX = 0;
                }
            }
        }

        private void CheckForTransition()
        {
            if (stateMachine.OnWall == false)
            {
                if (stateMachine.IsGrounded)
                {
                    stateMachine.ChangeState(stateMachine.GroundState);
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.InAirState);
                }
            }
        }
    }
}
