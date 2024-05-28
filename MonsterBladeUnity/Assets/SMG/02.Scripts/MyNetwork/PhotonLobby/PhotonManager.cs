using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

using ShowPopup = MonsterBlade.UI.ShowPopup;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace MonsterBlade.MyPhoton
{
    class RoomHashKey
    {
        public const string NAME = "n";
        public const string PASSWORD = "p";
        public const string HASPASSWORD = "hp";
    }

    [RequireComponent(typeof(ShowStatusWhenConnecting))]
    public class PhotonManager : PunSingleton<PhotonManager> //Photon.PunBehaviour
    {
        //public const string ROOM_NAME_KEY = "n";
        //public const string ROOM_PASSWORD_KEY = "p";
        //public const string ROOM_HASPASSWORD_KEY = "hp";

        string gameVersion = "0.1";

        public Canvas photonLobby;

        public GameObject roomPrefab;
        public GameObject roomListContent;

        public Room roomDesc;
        public Button joinRoom;
        public Button createRoom;
        public GameObject passwordPanel;

        public InputField playerNickName;

        public Image ServerConnectIcon;
        Sprite ConnectedIcon;
        Sprite DisconnectedIcon;
        

        List<Room> rooms = new List<Room>();


        protected override void Awake()
        {
            base.Awake();

            PhotonNetwork.logLevel = PhotonLogLevel.Informational;
            PhotonNetwork.autoJoinLobby = true;
            PhotonNetwork.automaticallySyncScene = true;

            ConnectedIcon = Resources.Load<Sprite>("MyPhoton/Connected Icon");
            DisconnectedIcon = Resources.Load<Sprite>("MyPhoton/Disconnected Icon");

            DebugLogPlayerInfo();
        }

        private void Start()
        {
            //photonLobby.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F8))
            {
                photonLobby.gameObject.SetActive(!Input.GetKey(KeyCode.LeftShift));
            }
        }

        public void SetConnect(bool bFail)
        {
            if(PhotonNetwork.connected)
                StartCoroutine(Disconnect());
            else
                StartCoroutine(Connect());
        }

        IEnumerator Connect()
        {
            if(PhotonNetwork.connected)
            {
                Debug.Log("Already Connected");
                yield break;
            }

            //Debug.Log("Wait for Connect Photon Online Server...");

            //yield return new WaitForSeconds(1.0f);

            Debug.Log("Try Connect Photon Online Server");
            
            if(!PhotonNetwork.ConnectUsingSettings(gameVersion))
            {
                Debug.Log("Failed ConnectUsingSettings()");
                //enabled = false;
                yield break;
            }
            Debug.Log("Connect !!");

            playerNickName.text = PhotonNetwork.playerName;
        }

        IEnumerator Disconnect()
        {
            if (!PhotonNetwork.connected)
            {
                Debug.Log("Already Disconnect");
                yield break;
            }

            Debug.Log("Try Disconnect Photon Online Server");
            //PhotonNetwork.lea
            PhotonNetwork.Disconnect();
            //yield return new WaitForSeconds(3.0f);

            if (PhotonNetwork.connected)
            {
                Debug.Log("Failed Disconnect()");
                //enabled = false;
                yield break;
            }
            Debug.Log("Disconnect !!");
        }


        public void CreateRoomTemp()
        {
            CreateRoom("1234qwer Title", "1234qwer");
        }
        public void CreateRoom(string name, string password = null)
        {
            RoomOptions _options = new RoomOptions();
            _options.MaxPlayers = 3;
            
            _options.CustomRoomProperties = new Hashtable{
                {RoomHashKey.NAME, name },
                {RoomHashKey.HASPASSWORD, (password==null) ? false : true },
                {RoomHashKey.PASSWORD, password }
            };

            _options.CustomRoomPropertiesForLobby = new string[] { 
                RoomHashKey.NAME, 
                RoomHashKey.HASPASSWORD, 
                RoomHashKey.PASSWORD};
            //_options.PublishUserId = true;
            GetComponent<ShowStatusWhenConnecting>().enabled = true;
            PhotonNetwork.CreateRoom(null, _options, null);
        }

        public override void OnReceivedRoomListUpdate()
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                Destroy(rooms[i].gameObject);
                //rooms.RemoveAt(0);
            }
            rooms.RemoveRange(0, rooms.Count);

            foreach (RoomInfo info in PhotonNetwork.GetRoomList())
            {
                Room _room = Instantiate(roomPrefab, roomListContent.transform).GetComponent<Room>();
                _room.RoomInfo = info;
                //_room.GetComponent<Button>().onClick.AddListener(() => PhotonNetwork.JoinRoom(info.Name));
                //_room.SetRoomDesc(roomDesc);
                //_room.SetJoinRoom(joinRoom);
                _room.OnClickAction += SetRoomClickAction;


                rooms.Add(_room);

                //Debug.Log((bool)room.CustomProperties[ROOM_HASPASSWORD_KEY]);
            }
        }

        void SetRoomClickAction(Room room)
        {
            roomDesc.RoomInfo = room.RoomInfo;
            joinRoom.onClick.RemoveAllListeners();
            joinRoom.onClick.AddListener(()=>JoinRoom(room.RoomInfo.Name));
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            //PhotonNetwork.LoadLevel("Map");

            if(!PhotonNetwork.isMasterClient)
            {
                //GameObject[] _objs = GameObject.FindGameObjectsWithTag("Player");
                //GameObject _obj = GameObject.FindGameObjectWithTag("Player");
                
                PhotonNetwork.Instantiate("Player/Player(Archer)", new Vector3(90, 0.2f, 350), Quaternion.identity, 0);

                // 본인 플레이어 생성
                // 플레이어 생성 위치
                // 그룹(선택)
                // 프리팹 위치(해당 플레이어 ctrl에 static으로?)
                // 세팅
                // Camera Setting
            }
        }

        public override void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
            base.OnFailedToConnectToPhoton(cause);
            SetServerConnectIcon(false);
        }

        public override void OnConnectionFail(DisconnectCause cause)
        {
            base.OnConnectionFail(cause);
            Debug.Log("OnConnectionFail()");
            SetServerConnectIcon(false);
        }

        public override void OnConnectedToPhoton()
        {
            base.OnConnectedToPhoton();
            Debug.Log("OnConnectedToPhoton()");

            DebugLogPlayerInfo();
            SetServerConnectIcon(true);
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Debug.Log("OnConnectedToMaster()");

        }
        public override void OnDisconnectedFromPhoton()
        {
            base.OnDisconnectedFromPhoton();
            Debug.Log("OnDisconnectedFromPhoton()");
            SetServerConnectIcon(false);
        }

        public override void OnLeftLobby()
        {
            base.OnLeftLobby();
            Debug.Log("OnLeftLobby()");
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            Debug.Log("OnJoinedLobby()");
        }

        public override void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
        {
            base.OnPhotonCustomRoomPropertiesChanged(propertiesThatChanged);
        }

        
        public void JoinRoom(string name, string pw = null)
        {
            if (name == null)
                return;
            Room _room = rooms.Find(x => x.RoomInfo.Name == name);
            if (_room != null)
            {
                RoomInfo _info = _room.RoomInfo;
                if ((bool)_info.CustomProperties[RoomHashKey.HASPASSWORD])
                {
                    Debug.Log("JoinRoom(): hasPassword");
                    if (pw == null)
                    {
                        if (passwordPanel != null)
                        {
                            passwordPanel.SetActive(true);
                            passwordPanel.GetComponent<PasswordPanel>().enterEventHandler += (x) => JoinRoom(name, x);
                        }
                        return;
                    }
                    else
                    {
                        if (_info.CustomProperties[RoomHashKey.PASSWORD].ToString() != pw)
                        {
                            // Failed password
                            GetComponent<ShowPopup>().enabled = true;
                            return;
                        }

                    }
                }
                GetComponent<ShowStatusWhenConnecting>().enabled = true;
                PhotonNetwork.JoinRoom(name);
            }
            else
            {
                GameObject _popup = new GameObject("-Popup-");
                _popup.transform.parent = joinRoom.transform.root;
                _popup.AddComponent<ShowPopup>();
            }

        }

        private void SetServerConnectIcon(bool connected)
        {
            if(connected)
            {
                ServerConnectIcon.color = Color.white;
                ServerConnectIcon.sprite = ConnectedIcon;
            }
            else
            {
                ServerConnectIcon.color = Color.gray;
                ServerConnectIcon.sprite = DisconnectedIcon;
            }
        }

        [ContextMenu("SetPlayerNickname")]
        void SetPlayerNickname()
        {
            PhotonNetwork.playerName = playerNickName.text;

            DebugLogPlayerInfo();
        }

        [ContextMenu("DebugLogPlayerInfo")]
        void DebugLogPlayerInfo()
        {
            PhotonPlayer player = PhotonNetwork.player;
            Debug.Log("UserId: " + player.UserId);
            Debug.Log("ID: " + player.ID);
            Debug.Log("NickName: " + player.NickName);
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            base.OnPhotonPlayerConnected(newPlayer);

            

        }

    }
}

