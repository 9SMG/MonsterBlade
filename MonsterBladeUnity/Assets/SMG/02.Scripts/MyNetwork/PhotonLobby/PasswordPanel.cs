using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MonsterBlade.UI;

//using MonsterBlade.MyPhoton;


[RequireComponent(typeof(DoModal))]
public class PasswordPanel : MonoBehaviour
{
    public InputField inputField;
    public event System.Action<string> enterEventHandler;

    void ClearEvent() { enterEventHandler = null; }

    public void OnClickEnter()
    {
        string _pw = inputField.text;
        //PhotonManager.Instance.JoinRoom()

        enterEventHandler?.Invoke(_pw);
        GetComponent<DoModal>().OnClose();
    }

    private void OnDisable()
    {
        ClearEvent();
    }

    private void OnDestroy()
    {
        ClearEvent();
    }

    private void OnApplicationQuit()
    {
        ClearEvent();
    }

}
