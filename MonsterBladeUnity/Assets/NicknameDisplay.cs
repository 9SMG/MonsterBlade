using UnityEngine;
using UnityEngine.UI;

public class NicknameDisplay : MonoBehaviour
{
    public Text nicknameText; // 플레이어 닉네임을 표시할 UI Text 객체

    private void Start()
    {
        // NicknameManager 스크립트에 접근하여 플레이어 닉네임 가져오기
        string playerNickname = PlayerPrefs.GetString("PlayerNickname");

        // 가져온 닉네임을 UI Text에 표시
        nicknameText.text = playerNickname.ToString();
    }
}
