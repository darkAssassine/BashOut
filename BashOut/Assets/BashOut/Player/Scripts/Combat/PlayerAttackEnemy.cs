using UnityEngine;
using UnityEngine.Events;

namespace BashOut.Player
{
    public class PlayerAttackEnemy : MonoBehaviour
    {
        [SerializeField] private PlayerCombat combat;
        [SerializeField] private Rigidbody2D ownRB;
        [SerializeField] private float OwnKnockback;
        [SerializeField] private float OwnKnockbackWhenBlocked;
        [SerializeField] private float EnemyKnockback;
        [SerializeField] private float OwnKnockBackCooldown;
        [SerializeField] private float stunDuration;
        [SerializeField] private Player player;
        [SerializeField] private PlayerMovementStateMachine movement;
        [SerializeField] private float ownStun;
        [SerializeField] private UnityEvent attackedPlayer;

        private float currentOwnKnockBackCooldown;

        private void Update()
        {
            currentOwnKnockBackCooldown -= Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Player>() != null)
            {
                if (currentOwnKnockBackCooldown < 0)
                {
                    currentOwnKnockBackCooldown = OwnKnockBackCooldown;
                }
                else
                {
                    return;
                }
                if (collision.GetComponent<Player>().IsBlocking == false && collision.GetComponent<Player>().IsInvincible == false)
                {
                    collision.GetComponent<Rigidbody2D>().AddForce(combat.AttackDir * EnemyKnockback, ForceMode2D.Impulse);
                    collision.GetComponent<Player>().Stun(stunDuration);
                }


                ownRB.linearVelocity = new Vector2(0, 0);


                if (collision.GetComponent<Player>().IsBlocking == false)
                {
                    ownRB.AddForce(combat.AttackDir * -1 * OwnKnockback);
                }
                else
                {
                    ownRB.AddForce(combat.AttackDir * -1 * OwnKnockbackWhenBlocked, ForceMode2D.Impulse);
                }
                attackedPlayer?.Invoke();
                player.Stun(ownStun);
                combat.PlayerHitSomething = true;
                player.PlayerGotAttacked?.Invoke();
            }
        }
    }
}
