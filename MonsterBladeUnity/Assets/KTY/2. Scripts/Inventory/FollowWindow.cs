using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 슬롯에 마우스를 갖다 대면 호출되는 "해당 아이템 정보가 뜨는 스크립트"
 */

public class FollowWindow : MonoBehaviour
{
    /* 싱글톤 */
    public static FollowWindow instance; 

    RectTransform rec; // 자기 위치

    Vector3 mousePos; // 마우스 위치 좌표

    Transform hidePivot; // 숨어있을 장소

    Transform defTr; // 기본 위치

    bool isShow; // 보이기 여부(쓸데없이 이동하는 거 방지를 위함)

	#region *아이템 정보
	ItemData _data;
    public ItemData Data
	{
        // 정보를 얻어오기만 할 테니 get은 구현x
		set { _data = value; }
	}

    [SerializeField] Image img;
    [SerializeField] Text itmName;
    [SerializeField] Text itmTxt;
	#endregion

	private void Awake()
	{
        instance = this;
        rec = GetComponent<RectTransform>();

        hidePivot = GameObject.FindGameObjectWithTag("PopupPivot").transform;
	}

	private void Start()
	{
        defTr = transform.parent;
        transform.SetParent(hidePivot);
	}


	// Update is called once per frame
	void Update()
    {
		if (_data != null)
		{
            img.sprite = _data.itemSpr;
            itmName.text = _data.itemName;
            itmTxt.text = _data.description;
        }

		if (isShow)
		{
            mousePos = Input.mousePosition;
            rec.position = mousePos;
        }
    }

    // 창 띄우기
    public void ShowWindow()
	{
        transform.SetParent(defTr);
        isShow = true;
    }

    // 창 닫기
    public void CloseWindow()
	{
        transform.SetParent(hidePivot);
        isShow = false;
    }
}
