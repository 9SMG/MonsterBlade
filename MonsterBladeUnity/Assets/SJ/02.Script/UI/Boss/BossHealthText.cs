using UnityEngine;
using UnityEngine.UI;

public class BossHealthText : MonoBehaviour
{
    public Text BossHpPercent; // 보스 체력을 표시할 UI Text

    private void Start()
    {
        // 보스의 체력 변경 이벤트에 UI 업데이트 함수를 구독
        FindObjectOfType<MonsterBlade.UI.Boss>().OnHealthChanged.AddListener(UpdateHealthText);
    }

    private void UpdateHealthText(float currentHealth)
    {
        // 보스의 현재 체력을 UI Text에 표시
        BossHpPercent.text = "Boss Health: " + currentHealth.ToString();
    }
}
