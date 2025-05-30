using UnityEngine;
using UnityEngine.InputSystem;

namespace BashOut.Player
{
    public class UserInput : PlayerInput
    {
        [SerializeField, Header("InputAction")]
        private InputActionAsset playerActionAsset;

        private InputAction dodgeAction;
        private InputAction blockAction;
        private InputAction jumpAction;
        private InputAction attackAction;
        private InputAction moveAction;
        private InputAction attackDirAction;

        private void Awake()
        {
            InputActionMap inputActionMap = playerActionAsset.FindActionMap("Base");

            dodgeAction = inputActionMap.FindAction("Dodge");
            blockAction = inputActionMap.FindAction("Block");
            jumpAction = inputActionMap.FindAction("Jump");
            attackAction = inputActionMap.FindAction("Attack");
            moveAction = inputActionMap.FindAction("Movement");
            attackDirAction = inputActionMap.FindAction("AttackDir");
        }

        private void Update()
        {
            EvaluateInputs();
        }

        private void EvaluateInputs()
        {
            //Dodge = dodgeAction.WasPressedThisFrame();
            Block = blockAction.WasPressedThisFrame();
            Jump = jumpAction.WasPressedThisFrame();
            Attack = attackAction.WasPressedThisFrame();

            MoveDirection = moveAction.ReadValue<Vector2>();
            AttackDirection = attackDirAction.ReadValue<Vector2>();
            AttackDirection = RoundToNearestDirection(AttackDirection);
        }

        public Vector2 RoundToNearestDirection(Vector2 input)
        {
            if (input.sqrMagnitude < 0.01f)
                return Vector2.zero;

            float x = Mathf.Round(Mathf.Clamp(input.normalized.x, -1f, 1f));
            float y = Mathf.Round(Mathf.Clamp(input.normalized.y, -1f, 1f));

            return new Vector2(x, y).normalized;
        }

        private void OnEnable()
        {
            Enable();
        }

        private void OnDisable()
        {
            Disable();
        }

        public void Enable()
        {
            dodgeAction.Enable();
            blockAction.Enable();
            jumpAction.Enable();
            attackAction.Enable();
            moveAction.Enable();
            attackDirAction.Enable();
        }

        public void Disable()
        {
            dodgeAction.Disable();
            blockAction.Disable();
            jumpAction.Disable();
            attackAction.Disable();
            moveAction.Disable();
            attackDirAction.Disable();
        }
    }
}
