using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace BashOut.Player
{
    public class Player : MonoBehaviour
    {
        [HideInInspector] public bool CanMove = true;
        [HideInInspector] public bool CanAttack = true;
        [HideInInspector] public bool CanBlock = true;
        [HideInInspector] public bool IsBlocking = false;
        [HideInInspector] public bool IsInvincible = false;

        [HideInInspector] public string Name;

        [SerializeField] private PlayerInput playerInput;

        public UnityEvent<Player> PlayerDied;
        public UnityEvent PlayerGotAttacked;
        public UnityEvent PlayerAttacked;

        private bool isDead = false;

        [SerializeField] private TextMeshProUGUI textComponent;
        [SerializeField] private ControllerType type;
        [SerializeField] private GameObject DeadVFX;

        public PlayerInput PlayerInput { get { return playerInput; } }

        private bool isStunned;

        private float currentInvincibleTime;
        private float currentStunDuration;

        private void Update()
        {
            currentInvincibleTime -= Time.deltaTime;
            if (currentInvincibleTime < 0)
            {
                IsInvincible = false;
            }
            currentStunDuration -= Time.deltaTime;
            if (currentStunDuration < 0 && isStunned)
            {
                isStunned = false;
                CanMove = true;
                CanAttack = true;
                CanBlock = true;
            }
        }

        public void Stun(float _stunDuration)
        {
            currentStunDuration = _stunDuration;
            isStunned = true;
            CanMove = false;
            CanAttack = false;
            CanBlock = false;
        }

        public void MakeInvincible(float _timeInSec)
        {
            currentInvincibleTime = _timeInSec;
        }

        private void Awake()
        {
            CanMove = true;
            CanAttack = true;
            CanBlock = true;
            IsBlocking = false;
            IsInvincible = false;
        }

        public void KillPlayer()
        {
            if (isDead == true)
            {
                return;
            }
            isDead = true;
            PlayerDied.Invoke(this);
            CanMove = false;
            CanAttack = false;
            CanBlock = false;
            Destroy(this.gameObject, 1);
            Destroy(Instantiate(DeadVFX, transform.position, Quaternion.identity), 5);
        }

        public PlayerSaveData GetPlayerSaveData()
        {
            PlayerSaveData save = new PlayerSaveData();
            save.Name = Name;
            save.Type = type;
            return save;
        }

        public void Initalize(int health)
        {
            textComponent.text = health.ToString();
        }
    }
}
