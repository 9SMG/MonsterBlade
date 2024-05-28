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

        protected float fraction;

        protected float limitTime = 0.3f;

        protected bool firstRecv = true;

        int pvDataLen = 2;

        protected Animator animatorPun;

        public bool IsMine
        {
            get
            {
                if (pv == null)
                    return true;
                return (!PhotonNetwork.inRoom) || pv.isMine;
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

        [ContextMenu("DebugLogIsMine")]
        void DebugLogIsMine()
        {
            Debug.Log(gameObject.name +" isMine? : " +IsMine);
        }

        

        protected virtual void InitSync()
        {
            InitSyncPosAndRot();
        }


        protected virtual void UpdateSync()
        {
            if (firstRecv)
                return;

            fraction += Time.deltaTime;
            UpdateSyncPosAndRot();
        }

        protected void UpdateSyncPos()
        {
            if (fraction < limitTime)
            {
                //fraction += Time.deltaTime;
                fraction = Mathf.Clamp(Time.deltaTime + fraction, 0f, limitTime - 0.01f);
                transform.position = Vector3.Lerp(onUpdatePos, latestCorrectPos, fraction / limitTime);
            }
            else
            {
                transform.position = latestCorrectPos;
            }
        }

        protected void UpdateSyncRot()
        {
            if (fraction < limitTime)
            {
                //fraction += Time.deltaTime;
                fraction = Mathf.Clamp(Time.deltaTime + fraction, 0f, limitTime - 0.01f);
                transform.rotation = Quaternion.Lerp(onUpdateRot, latestCorrectRot, fraction / limitTime);
            }
            else
            {
                transform.rotation = latestCorrectRot;
            }
        }

        protected void UpdateSyncPosAndRot()
        {
            if (fraction < limitTime)
            {
                //fraction += Time.deltaTime;
                fraction = Mathf.Clamp(Time.deltaTime + fraction, 0f, limitTime - 0.01f);
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

        public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                Vector3 pos = transform.position;
                Quaternion rot = transform.rotation;

                //stream.Serialize(ref pos);
                //stream.Serialize(ref rot);

                //stream.SendNext(pos);
                //stream.SendNext(rot);

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

                //latestCorrectPos = (Vector3)pvData[0];
                //latestCorrectRot = (Quaternion)pvData[1];
                PhotonSerializeViewData(false, pvData);

                onUpdatePos = transform.position;
                onUpdateRot = transform.rotation;

                fraction = 0;

                if(firstRecv)
                {
                    firstRecv = false;
                    InitSync();
                }
            }
        }

        protected virtual void PhotonSerializeViewData(bool bWrite, object[] pvData)
        {
            if(bWrite)
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

        protected void SetPhotonViewDataLen(int len = 2)
        {
            pvDataLen = len;
        }

        ////// Animator RPC /////
        [PunRPC]
        protected void SetBoolRPC(string name, bool value)
        {
            if (PhotonNetwork.inRoom && pv.isMine)
            {
                pv.RPC("SetBoolRPC", PhotonTargets.Others, name, value);
            }

            animatorPun.SetBool(name, value);
        }

        [PunRPC]
        protected void SetTriggerRPC(string name)
        {
            if (PhotonNetwork.inRoom && pv.isMine)
            {
                pv.RPC("SetTriggerRPC", PhotonTargets.Others, name);
            }

            animatorPun.SetTrigger(name);
        }

        [PunRPC]
        protected void SetFloatRPC(string name, float value, bool blend = false)
        {
            if (PhotonNetwork.inRoom && pv.isMine)
            {
                pv.RPC("SetFloatRPC", PhotonTargets.Others, name, value, blend);
            }
            if(blend)
                animatorPun.SetFloat(name, value, 0.1f, Time.deltaTime);
            else animatorPun.SetFloat(name, value);
        }
    }
}


