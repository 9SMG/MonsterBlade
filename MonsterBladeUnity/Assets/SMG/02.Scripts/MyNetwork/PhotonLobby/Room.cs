using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace MonsterBlade.MyPhoton
{
    public class Room : MonoBehaviour
    {
        public Text roomID;

        [Header("Room View")]
        public Text roomName;
        public Text playerCount;
        public Image passwordIcon;

        Sprite lockCloseIcon;
        Sprite lockOpenIcon;

        RoomInfo roomInfo;

        /// <summary>
        /// Set될 때마다, Room 게임 오브젝트의 출력 데이터 갱신
        /// </summary>
        public RoomInfo RoomInfo
        {
            get { return roomInfo; }
            
            set
            {
                roomInfo = value;
                if (roomInfo != null)
                {
                    SetRoomID(roomInfo.Name);
                    SetRoomName(roomInfo.CustomProperties[RoomHashKey.NAME].ToString());
                    SetPlayerCount(roomInfo.PlayerCount, roomInfo.MaxPlayers);
                    SetPasswordIcon((bool)roomInfo.CustomProperties[RoomHashKey.HASPASSWORD]);
                }
            }
        }

        Button button;
        Button joinRoom;
        Room roomDesc;

        public event UnityAction<Room> OnClickAction;
        void OnClickActionFunc() { OnClickAction(this); }

        int currPlayer;
        int maxPlayer;

        protected virtual void Awake()
        {
            //Debug.Log("Room Awake");
            button = GetComponent<Button>();
            if(button != null)
            {
                var _onClick = button.onClick;
                _onClick.AddListener(() => Debug.Log("Room [" + gameObject.GetInstanceID() + "] Click"));
                _onClick.AddListener(OnClickActionFunc);
                //_onClick.AddListener(OnClickSetDesc);
                //_onClick.AddListener(OnClickSetJoinRoom);
            }
            //OnClickRoom += () => Debug.Log("Room [" + gameObject.GetInstanceID() + "] Click");
            //OnClickRoom += () => OnClickSetDesc();
            //OnClickRoom += () => OnClickSetJoinRoom();

            //button?.onClick.AddListener(OnClickRoom);

            //button.onClick.AddListener(OnClickSetJoinRoom);

            lockCloseIcon = Resources.Load<Sprite>("MyPhoton/Lock Closed Icon");
            lockOpenIcon = Resources.Load<Sprite>("MyPhoton/Lock Open Icon");


        }


        protected virtual void Start()
        {
            //SetRoomID("123");
            //SetRoomName("Test");
        }

        protected virtual void Update()
        {
            if(Input.GetKeyDown(KeyCode.F8))
            {
                if(Input.GetKey(KeyCode.LeftControl))
                    ShowDebugMode(!Input.GetKey(KeyCode.LeftShift));
            }
        }

        public void SetRoomID(string id)
        {
            roomID.text = id;
        }

        public void SetRoomName(string name)
        {
            roomName.text = name;
        }

        public void SetPlayerCount(int current, int max)
        {
            string _str = string.Format("{0} / {1}", current, max);
            playerCount.text = _str;
        }

        void SetPasswordIcon(bool usePassword)
        {
            if (usePassword)
            {
                passwordIcon.color = Color.white;
                passwordIcon.sprite = lockCloseIcon;
            }
            else
            {
                passwordIcon.color = Color.gray;
                passwordIcon.sprite = lockOpenIcon;
            }
        }

        public void SetRoomDesc(Room desc)
        {
            if (desc == null && roomDesc == null)
                return;

            //Debug.Log("SetRoomDesc(): desc[" + desc + "] roomDesc[" + roomDesc + "]");

            if (roomDesc == null)
                button.onClick.AddListener(OnClickSetDesc);
            else if (desc == null)
                button.onClick.RemoveListener(OnClickSetDesc);

            roomDesc = desc;
        }

        public void SetJoinRoom(Button join)
        {
            if (join == null && joinRoom == null)
                return;

            //Debug.Log("SetJoinRoom(): join[" + join + "] joinRoom[" + joinRoom + "]");

            if (joinRoom == null)
                button.onClick.AddListener(OnClickSetJoinRoom);
            else if (join == null)
                button.onClick.RemoveListener(OnClickSetJoinRoom);

            joinRoom = join;
        }

        void OnClickSetDesc()
        {
            //if (roomDesc == null)
            //    return;
            roomDesc.RoomInfo = RoomInfo;
        }

        void OnClickSetJoinRoom()
        {
            //if (joinRoom == null)
            //    return;

            joinRoom.onClick.RemoveAllListeners();
            joinRoom.onClick.AddListener(() => Debug.Log(roomInfo.Name));
            joinRoom.onClick.AddListener(() => PhotonManager.Instance.JoinRoom(roomInfo.Name));
        }

        void ShowDebugMode(bool show)
        {
            roomID.gameObject.SetActive(show);
            //GameObject _roomID = roomID.gameObject;
            //_roomID.SetActive(!_roomID.activeSelf);
        }

        [ContextMenu("SetRoomDesc(null)")]
        void DebugSetDescNull()
        {
            SetRoomDesc(null);
        }

        [ContextMenu("SetJoinRoom(null)")]
        void DebugSetJoinNull()
        {
            SetJoinRoom(null);
        }
    }
}

