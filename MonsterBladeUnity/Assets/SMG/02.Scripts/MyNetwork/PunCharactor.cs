using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterBlade.MyPhoton
{
    [RequireComponent(typeof(PhotonView))]
    public class PunCharactor : Photon.PunBehaviour, IPunObservable //PunCharactor<T> : Photon.PunBehaviour, IPunObservable where T : PunCharactor<T>
    {
        protected PhotonView pv;

        protected object[] pvData;

        protected Vector3 latestCorrectPos;
        protected Vector3 onUpdatePos;

        protected Quaternion latestCorrectRot = Quaternion.identity;
        protected Quaternion onUpdateRot = Quaternion.identity;

        float fraction;

        float limitTime = 0.2f;

        bool firstRecv = true;

        protected int pvDataLen = 2;

        protected bool IsMine
        {
            get
            {
                if (pv == null)
                    return true;
                return !PhotonNetwork.inRoom || pv.isMine;
            }
        }

        protected virtual void Awake()
        {
            pv = GetComponent<PhotonView>();
            if (pv != null)
            {
                List<Component> pvList = pv.ObservedComponents;

                if (pvList.Find(
                    (x) => x != null
                    && x.GetInstanceID() == this.GetInstanceID()
                    ) == null)
                {
                    //pv.ObservedComponents.RemoveAll((x) => true);

                    if (pvList.Count == 1)
                        pvList[0] = this;
                    else
                        pv.ObservedComponents.Add(this);


                    pv.synchronization = ViewSynchronization.UnreliableOnChange;
                }

                pvData = new object[10];    // Max Serialize Queue
            }
        }

        protected virtual void InitSync()
        {
            InitSyncPosAndRot();
        }


        protected virtual void SyncUpdate()
        {
            if (firstRecv)
                return;
            SyncPosAndRotUpdate();
        }


        //protected virtual void Update()
        //{
        //    if (!PhotonNetwork.inRoom)
        //        return;

        //    Debug.Log("PunCharactor<T>: Update()");
        //}

        protected void SyncPosAndRotUpdate()
        {
            if (fraction < limitTime)
            {
                //fraction += Time.deltaTime;
                fraction = Mathf.Clamp(Time.deltaTime + fraction, 0f, limitTime - 0.01f);
                //Debug.Log("fraction: " + fraction);
                transform.position = Vector3.Lerp(onUpdatePos, latestCorrectPos, fraction / limitTime);
                transform.rotation = Quaternion.Lerp(onUpdateRot, latestCorrectRot, fraction / limitTime);
            }
            else
            {
                transform.position = latestCorrectPos;
                transform.rotation = latestCorrectRot;
            }
        }

        protected void InitSyncPosAndRot()
        {
            transform.position = latestCorrectPos;
            transform.rotation = latestCorrectRot;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                Vector3 pos = transform.position;
                Quaternion rot = transform.rotation;

                //stream.Serialize(ref pos);
                //stream.Serialize(ref rot);

                //stream.SendNext(pos);
                //stream.SendNext(rot);

                //Debug.Log("Send pos: " + pos);
                //Debug.Log("Send rot: " + rot);

                //pvData[0] = pos;
                //pvData[1] = rot;
                PhotonSerializeViewData(true, pvData);

                //foreach (object obj in pvData)
                //{
                //    stream.SendNext(obj);
                //}
                for(int i = 0; i < pvDataLen; i++)
                {
                    stream.SendNext(pvData[i]);
                }
            }
            else
            {
                Vector3 pos = Vector3.zero;
                Quaternion rot = Quaternion.identity;

                //stream.Serialize(ref pos);
                //stream.Serialize(ref rot);

                //pos = (Vector3)stream.ReceiveNext();
                //rot = (Quaternion)stream.ReceiveNext();

                //pvData[0] = stream.ReceiveNext();
                //pvData[1] = stream.ReceiveNext();

                for (int i = 0; i < pvDataLen; i++)
                {
                    pvData[i] = stream.ReceiveNext();
                }

                //pos = (Vector3)pvData[0];
                //rot = (Quaternion)pvData[1];

                //Debug.Log("recv pos: " + pos);
                //Debug.Log("recv rot: " + rot);

                onUpdatePos = transform.position;
                onUpdateRot = transform.rotation;

                //latestCorrectPos = pos;
                //latestCorrectRot = rot;

                PhotonSerializeViewData(false, pvData);

                fraction = 0;

                if(firstRecv)
                {
                    firstRecv = false;
                    InitSync();
                }
            }
        }

        protected virtual void PhotonSerializeViewData(bool bSend, object[] pvData)
        {
            if(bSend)
            {
                pvData[0] = transform.position;
                pvData[1] = transform.rotation;
            }
            else
            {
                latestCorrectPos = (Vector3)pvData[0];
                latestCorrectRot = (Quaternion)pvData[1];
            }
        }
    }
}


