using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : MonoBehaviour
{
    public Transform quickSlot;
    public List<Slot> slots = new List<Slot>();
    const int SLOTSIZE = 4; // ΩΩ∑‘ ªÁ¿Ã¡Ó

	private void Awake()
	{
        quickSlot = GameObject.FindGameObjectWithTag("QuickSlot").transform;

        for(int i = 0; i < SLOTSIZE; i++)
		{
            slots.Add(quickSlot.GetChild(i).GetComponent<Slot>());
        }

    }

	// Start is called before the first frame update
	void Start()
    {
        QuickSlotInit();
    }

    // Update is called once per frame
    void Update()
	{
        SelectQuickSlot();
    }

    // ΩΩ∑‘ ªÛ≈¬ √ ±‚»≠
	private void QuickSlotInit()
	{
        for (int i = 0; i < SLOTSIZE; i++)
        {
            slots[i].ActiveImg.SetActive(false);
        }

        slots[0].ActiveSlotState();
    }

    // ƒ¸ΩΩ∑‘ ¡∂¿€
    void SelectQuickSlot()
	{
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            for (int i = 0; i < SLOTSIZE; i++)
            {
                slots[i].ActiveImg.SetActive(false);
            }

            slots[0].ActiveSlotState();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            for (int i = 0; i < SLOTSIZE; i++)
            {
                slots[i].ActiveImg.SetActive(false);
            }
            slots[1].ActiveSlotState();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            for (int i = 0; i < SLOTSIZE; i++)
            {
                slots[i].ActiveImg.SetActive(false);
            }

            slots[2].ActiveSlotState();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            for (int i = 0; i < SLOTSIZE; i++)
            {
                slots[i].ActiveImg.SetActive(false);
            }

            slots[3].ActiveSlotState();
        }

#elif UNITY_STANDALONE_WIN
		if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
		{
            for(int i = 0; i < SLOTSIZE; i++)
			{
                slots[i].ActiveImg.SetActive(false);
			}

            slots[0].ActiveSlotState();
		}
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            for (int i = 0; i < SLOTSIZE; i++)
            {
                slots[i].ActiveImg.SetActive(false);
            }
            slots[1].ActiveSlotState();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            for (int i = 0; i < SLOTSIZE; i++)
            {
                slots[i].ActiveImg.SetActive(false);
            }

            slots[2].ActiveSlotState();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            for (int i = 0; i < SLOTSIZE; i++)
            {
                slots[i].ActiveImg.SetActive(false);
            }

            slots[3].ActiveSlotState();
        }
#endif
    }
}

