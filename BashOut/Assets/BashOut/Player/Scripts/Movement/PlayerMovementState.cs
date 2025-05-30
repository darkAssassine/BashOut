using StateMachine;
using UnityEngine;

namespace BashOut.Player
{
    public class PlayerMovementState : State
    {
        [SerializeField] protected PlayerMovementStateMachine stateMachine;
        [SerializeField] protected Player player;
        [SerializeField] protected PlayerInput input;
        [SerializeField] protected Rigidbody2D rb;
        [SerializeField] protected float speed;
        [SerializeField] protected float dodgeSpeed;
        [SerializeField] protected float acceleration;
        [SerializeField] protected float deceleration;

        public override void UpdatePhysics()
        {
            Accelerate();
            Deccelerate();
        }

        public override void UpdateLogic()
        {
            TryJump();
            TryDodge();
        }

        private void Accelerate()
        {
            if (Mathf.Abs(rb.linearVelocityX) < speed)
            {
                rb.AddForceX(input.MoveDirection.x * Time.fixedDeltaTime * acceleration, ForceMode2D.Impulse);
            }
        }

        private void Deccelerate()
        {
            float velocityX = rb.linearVelocity.x;

            if (Mathf.Abs(input.MoveDirection.x) < 0.1f)
            {
                if (Mathf.Abs(velocityX) > 0.01f)
                {
                    float counterForceX = -Mathf.Sign(velocityX) * deceleration * Time.fixedDeltaTime;

                    rb.AddForce(new Vector2(counterForceX, 0f), ForceMode2D.Impulse);
                }
            }
            else if ((rb.linearVelocityX < 0 && input.MoveDirection.x > 0) || (rb.linearVelocityX > 0 && input.MoveDirection.x < 0))
            {
                if (Mathf.Abs(velocityX) > 0.01f)
                {
                    float counterForceX = -Mathf.Sign(velocityX) * deceleration * Time.fixedDeltaTime;

                    rb.AddForce(new Vector2(counterForceX, 0f), ForceMode2D.Impulse);
                }
            }
        }

        protected void TryDodge()
        {
            if (input.Dodge)
            {
                Dodge();
            }
        }

        protected virtual void Dodge()
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(input.MoveDirection * dodgeSpeed, ForceMode2D.Impulse);
            player.MakeInvincible(stateMachine.InvincibleDuration);
        }

        protected void TryJump()
        {
            if (input.Jump)
            {

                if (stateMachine.CanJump)
                {
                    Jump();
                    stateMachine.Jumped();
                }
            }
        }

        protected virtual void Jump()
        {

        }
    }
}
