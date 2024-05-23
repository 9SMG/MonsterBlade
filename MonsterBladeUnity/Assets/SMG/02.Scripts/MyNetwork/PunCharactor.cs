using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PunCharactor: Photon.PunBehaviour, IPunObservable //PunCharactor<T> : Photon.PunBehaviour, IPunObservable where T : PunCharactor<T>
{
    protected PhotonView pv;

    protected object[] pvData;

    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;

    private Quaternion latestCorrectRot = Quaternion.identity;
    private Quaternion onUpdateRot = Quaternion.identity;

    float fraction;

    public float limitTime;

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

    }


    protected virtual void SyncUpdate()
    {
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

            pvData[0] = pos;
            pvData[1] = rot;

            foreach (object obj in pvData)
            {
                stream.SendNext(obj);
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

            for (int i = 0; i < 2; i++)
            {
                pvData[i] = stream.ReceiveNext();
            }

            pos = (Vector3)pvData[0];
            rot = (Quaternion)pvData[1];


            onUpdatePos = transform.position;
            onUpdateRot = transform.rotation;

            latestCorrectPos = pos;
            latestCorrectRot = rot;

            fraction = 0;
        }
    }
}
