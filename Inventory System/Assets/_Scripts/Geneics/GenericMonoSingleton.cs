using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopAndInventory
{
    public class GenericMonoSingleton<T> : MonoBehaviour where T : GenericMonoSingleton<T>
    {
        public static T Instance { get { return instance; } }

        private static T instance;

        protected virtual void Awake()
        {
            if(instance == null)
            {
                instance = (T)this;
            }
            else
            {
                Debug.Log("Object Instance cannot be created");
                Destroy(gameObject);
            }
        }
    }
}