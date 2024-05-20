using UnityEngine;
using UnityEngine.Events;

namespace MonsterBlade.UI
{
    public class Boss : MonoBehaviour
    {
        public float maxHealth = 100f;
        private float currentHealth;

        public UnityEvent<float> OnHealthChanged = new UnityEvent<float>();

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            // 체력 변경 이벤트 발생
            OnHealthChanged.Invoke(currentHealth);
        }
    }
}
