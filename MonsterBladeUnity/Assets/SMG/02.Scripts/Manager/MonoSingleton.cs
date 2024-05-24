using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MonsterBlade.Manager
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        _instance = new GameObject(
                            string.Format("{0}_Singleton", typeof(T).ToString()), typeof(T)).GetComponent<T>();

                        DontDestroyOnLoad(_instance);
                    }
                }
                return _instance;
            }
        }

        public static T Exists()
        {
            if (_instance)
                return _instance;

            return null;
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;

            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance != null)
            {
                _instance = null;
            }
        }

        protected void OnApplicationQuit()
        {
            _instance = null;
        }
    }
}