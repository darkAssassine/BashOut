using UnityEngine;
using UnityEngine.Events;

namespace BashOut.HealthSystem
{
    public class Health : MonoBehaviour
    {
        public int MaxHealth;

        [SerializeField] private float damageRecoverTime;
        [SerializeField] private float healRecoverTime;

        [SerializeField] private UnityEvent damaged;
        [SerializeField] private UnityEvent died;
        [SerializeField] private UnityEvent healed;

        private int currentHealth;
        private float currentDamageRecoverTime;
        private float currentHealRecoverTime;

        public void TakeDamage(int _damage)
        {
            if (currentDamageRecoverTime > 0)
            {
                return;
            }
            currentDamageRecoverTime = damageRecoverTime;

            currentHealth -= Mathf.Abs(_damage);
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);

            if (currentHealth == 0)
            {
                Death();
            }
            else
            {
                damaged?.Invoke();
            }
        }

        public void Heal(int _healAmount)
        {
            if (currentHealRecoverTime > 0)
            {
                return;
            }
            currentHealRecoverTime = healRecoverTime;

            if (currentHealth != MaxHealth)
            {
                healed?.Invoke();
            }
            currentHealth += Mathf.Abs(_healAmount);
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        }

        public void Death()
        {
            died?.Invoke();
        }

        private void Update()
        {
            currentDamageRecoverTime -= Time.deltaTime;
            currentHealRecoverTime -= Time.deltaTime;
        }

        public void MakeInvincible(float _timeInSeconds)
        {

        }
    }
}
