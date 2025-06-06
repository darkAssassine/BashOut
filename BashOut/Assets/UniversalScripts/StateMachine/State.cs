using UnityEngine;

namespace StateMachine
{
    public class State : MonoBehaviour
    {
        [HideInInspector] public bool IsActive;

        public virtual void Enter() { }
        public virtual void UpdateLogic() { }
        public virtual void UpdatePhysics() { }
        public virtual void Exit() { }
    }
}
