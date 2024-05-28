using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MonsterBlade.UI
{
    [System.Serializable]
    [SMG.Utility.ClassTooltip("Ctrl + F4: 강제 종료")]
    public class DoModal : MonoBehaviour
    {
        public int test;
        public string teasd1;

        private void OnEnable()
        {
            //gameObject.transform.SetAsLastSibling();

        }



        private void Update()
        {
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                && Input.GetKey(KeyCode.F4))
            {
                OnClose();
            }
        }

        private void LateUpdate()
        {
            gameObject.transform.SetAsLastSibling();
        }

        public void OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}



