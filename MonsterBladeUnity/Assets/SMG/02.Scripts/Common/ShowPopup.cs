using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterBlade.UI
{
    /// <summary>
    /// 중앙에 팝업형 UI 생성, 일정 시간 후 제거
    /// </summary>
    public class ShowPopup : MonoBehaviour
    {
        public float width = 400;
        public float height = 100;

        public string title;
        public string desc;

        public bool desctroy = true;
        public float activeTime = 3f;
        float timer;


        // Start is called before the first frame update
        void Start()
        {
            if (desctroy)
            {
                Destroy(this.gameObject, activeTime);
            }
            else
                timer = activeTime;
        }

        private void OnEnable()
        {
            timer = activeTime;
        }

        // Update is called once per frame
        void Update()
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                enabled = false;
            }
        }

        private void OnGUI()
        {
            Rect centeredRect = new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height);

            GUILayout.BeginArea(centeredRect, GUI.skin.box);
            {
                GUILayout.BeginVertical();

                GUILayout.Label(title);
                GUILayout.Label(desc);

                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }
    }
}