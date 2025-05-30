using UnityEngine;
using UnityEngine.Events;

namespace BashOut.Player
{
    public class PlayerAttackEnviroment : MonoBehaviour
    {
        [SerializeField] private PlayerCombat combat;
        [SerializeField] private Rigidbody2D ownRB;
        [SerializeField] private float OwnKnockback;
        [SerializeField] private float OwnKnockBackCooldown;
        [SerializeField] private Player player;
        [SerializeField] private PlayerMovementStateMachine movement;
        [SerializeField] private float ownStun;
        [SerializeField] private UnityEvent enviromentAttacked;
        private float currentOwnKnockBackCooldown;

        private void Update()
        {
            currentOwnKnockBackCooldown -= Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (currentOwnKnockBackCooldown < 0)
            {
                currentOwnKnockBackCooldown = OwnKnockBackCooldown;
            }

            ownRB.linearVelocity = new Vector2(0, 0);
            ownRB.AddForce(combat.AttackDir * -1 * OwnKnockback, ForceMode2D.Impulse);
            player.Stun(ownStun);
            enviromentAttacked?.Invoke();
            player.PlayerGotAttacked?.Invoke();
            combat.PlayerHitSomething = true;
        }
    }
}
