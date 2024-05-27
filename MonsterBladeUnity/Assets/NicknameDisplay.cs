using UnityEngine;
using UnityEngine.UI;

public class NicknameDisplay : MonoBehaviour
{
    public Text nicknameText; // �÷��̾� �г����� ǥ���� UI Text ��ü

    private void Start()
    {
        // NicknameManager ��ũ��Ʈ�� �����Ͽ� �÷��̾� �г��� ��������
        string playerNickname = PlayerPrefs.GetString("PlayerNickname");

        // ������ �г����� UI Text�� ǥ��
        nicknameText.text = playerNickname.ToString();
    }
}
