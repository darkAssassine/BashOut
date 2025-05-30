using UnityEngine;
using UnityEngine.Events;

namespace BashOut.Player
{
    public class CheckForDeadzone : MonoBehaviour
    {
        [SerializeField] private UnityEvent DeadzoneEntered;
        [SerializeField] private UnityEvent DeadzoneExited;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            DeadzoneEntered?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            DeadzoneExited?.Invoke();
        }
    }
}
