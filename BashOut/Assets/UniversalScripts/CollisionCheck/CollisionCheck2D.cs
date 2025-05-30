using UnityEngine;
using UnityEngine.Events;

namespace CollisionCheck
{
    public class CollisionCheck2D : MonoBehaviour
    {
        public bool IsColliding { get; private set; }

        [SerializeField] private UnityEvent onTriggerEnter;
        [SerializeField] private UnityEvent onTriggerExit;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IsColliding = true;
            onTriggerEnter?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            IsColliding = false;
            onTriggerExit?.Invoke();
        }
    }
}
