using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace MonsterBlade.MyPhoton
{
    public class CreateRoomCtrl : MonoBehaviour
    {
        public Button switchButton;
        public Button createButton;
        public Button joinButton;

        public GameObject viewForm;
        public GameObject SettingForm;

        bool isSetting = false;

        private void Awake()
        {
            switchButton?.onClick.AddListener(SwitchMode);
        }

        private void Start()
        {
            isSetting = true;
            SwitchMode();
        }

        public void SwitchMode()
        {
            isSetting = !isSetting;

            if(isSetting)
            {
                switchButton.GetComponentInChildren<Text>().text = "Close Setting";
                createButton.gameObject.SetActive(true);
                joinButton.gameObject.SetActive(false);

                viewForm.SetActive(false);
                SettingForm.SetActive(true);


            }
            else
            {
                switchButton.GetComponentInChildren<Text>().text = "Create Room";
                createButton.gameObject.SetActive(false);
                joinButton.gameObject.SetActive(true);

                viewForm.SetActive(true);
                SettingForm.SetActive(false);
            }

        }
    }

}

