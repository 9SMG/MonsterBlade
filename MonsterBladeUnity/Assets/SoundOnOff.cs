using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnOff : MonoBehaviour
{
    [SerializeField] private GameObject sSoundUi;
    public bool OnCheck = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (!OnCheck)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                sSoundUi.SetActive(true);
                OnCheck = true;
            }
            else if (OnCheck)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                sSoundUi.SetActive(false);
                OnCheck = false;
            }
        }
    }
}
