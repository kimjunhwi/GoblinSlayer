using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace junkaki
{
    public class GenericMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = GameObject.FindObjectOfType<T>();

                    if (null == instance)
                    {
                        var go = new GameObject(typeof(T).ToString() + "Singleton");
                        instance = go.AddComponent<T>();
                        DontDestroyOnLoad(go);
                    }
                }

                return instance;
            }
        }

        private void Awake()
        {
            if (null != instance && this != instance)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                instance = this as T;
                DoAwake();
            }
        }

        protected virtual void DoAwake()
        {
            // nothing
        }

        public void Myname()
        {
            Debug.Log("GenericMonoSingleton");
        }
    }
}
