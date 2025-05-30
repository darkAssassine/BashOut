using System;
using UnityEngine;

namespace BashOut.Player
{
    public class PlayerMovementStateInAir : PlayerMovementState
    {
        [SerializeField] private float jumpForce;
        [SerializeField] private float downForce;
        [SerializeField, Range(-100, 0)] private float AdditionalDownForce;
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

        private void AddDownForce()
        {
            if (input.MoveDirection.y < 0)
            {
                rb.AddForceY(input.MoveDirection.y * downForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            if (rb.linearVelocityY < 0)
            {
                rb.AddForceY(AdditionalDownForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }

            AddDownForce();
        }

        private void CheckForTransition()
        {

            if (stateMachine.IsGrounded)
            {
                stateMachine.ChangeState(stateMachine.GroundState);
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
