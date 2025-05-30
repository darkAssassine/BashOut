using UnityEngine;

namespace BashOut
{
    public class Deadzone : MonoBehaviour
    {
        [SerializeField] private Vector2 knockBack;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Player.Player player = collision.GetComponent<Player.Player>();
            if (player != null)
            {
                if (player.IsBlocking)
                {
                    collision.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                    collision.GetComponent<Rigidbody2D>().AddForce(knockBack, ForceMode2D.Impulse);
                    player.Stun(0.3f);
                }
                else
                {
                    player.KillPlayer();
                }
                player.PlayerGotAttacked?.Invoke();
            }
        }
    }
}
