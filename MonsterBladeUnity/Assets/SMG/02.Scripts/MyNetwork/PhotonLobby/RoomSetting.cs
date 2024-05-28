using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace MonsterBlade.MyPhoton
{
    public class RoomSetting : Room
    {
        [Header("Room Setting")]
        public InputField inputRoomName;
        public Toggle togglePassword;
        public InputField inputPassword;

        [Space]
        public Button switchButton;
        public Button createButton;
        public Button joinButton;

        public GameObject viewForm;
        public GameObject SettingForm;

        bool isSetting = false;

        protected override void Awake()
        {
            base.Awake();
            //Debug.Log("RoomSetting Awake");

            switchButton?.onClick.AddListener(OnClickSwitchMode);
            createButton?.onClick.AddListener(OnClickCreate);
            togglePassword?.onValueChanged.AddListener(OnClickTogglePassword);
        }

        protected override void Start()
        {   
            base.Start();

            isSetting = true;
            OnClickSwitchMode();
            OnClickTogglePassword(false);
        }

        void OnClickSwitchMode()
        {
            isSetting = !isSetting;

            if (isSetting)
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

        void OnClickTogglePassword(bool isOn)
        {
            inputPassword.interactable = isOn;
        }

        void OnClickCreate()
        {
            string _roomName = inputRoomName.text;
            string _password = (togglePassword.isOn) ? inputPassword.text : null;
            
            PhotonManager.Instance.CreateRoom(_roomName, _password);
        }

    }
}
