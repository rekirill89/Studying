using System.Collections.Generic;
using UnityEngine;

namespace DuelGame
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private Queue<T> _pool = new Queue<T>();
        private T _prefab;
        private Transform _parent;
        public ObjectPool(T prefab, int initSize, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;

            for(int i = 0; i< initSize; i++)
            {
                var x = Object.Instantiate(_prefab, parent);
                x.gameObject.SetActive(false);
                _pool.Enqueue(x);
            }
        }

        public T Get()
        {
            //Debug.Log("Get " + _pool.Count);
            if(_pool.Count == 0)
            {
                Debug.Log("If invoked");
                var x = Object.Instantiate(_prefab, _parent);                
                return x;
            }

            T obj = _pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        public void ReturnToPool(T obj)
        {
            //Debug.Log("Return " + _pool.Count);
            obj.transform.SetParent(_parent);    
            obj.gameObject.SetActive(false);            
            _pool.Enqueue(obj);
        }
    }
}

