using UnityEngine;

namespace BashOut.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public bool Dodge { get; protected set; }
        public bool Block { get; protected set; }
        public bool Jump { get; protected set; }
        public bool Attack { get; protected set; }
        public Vector2 MoveDirection { get; protected set; }
        public Vector2 AttackDirection { get; protected set; }
    }
}
