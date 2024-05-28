using UnityEngine;
using UnityEngine.UI;

public class NickNameManager : MonoBehaviour
{
    public InputField PlayerNickName;  
    public Text nicknameDisplayText;       

    private const string PlayerPrefsKey = "PlayerNickname";

    void Start()
    {
        // 이전에 저장된 닉네임 불러오기
        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            string savedNickname = PlayerPrefs.GetString(PlayerPrefsKey);
            PlayerNickName.text = savedNickname;
            DisplayNickname(savedNickname);
        }
    }

    public void SaveNickname()
    {
        string nickname = PlayerNickName.text;
        PlayerPrefs.SetString(PlayerPrefsKey, nickname);
        PlayerPrefs.Save();
        DisplayNickname(nickname);
    }

    private void DisplayNickname(string nickname)
    {
        if (nicknameDisplayText != null)
        {
            nicknameDisplayText.text = $"Welcome, {nickname}!";
        }
    }
}