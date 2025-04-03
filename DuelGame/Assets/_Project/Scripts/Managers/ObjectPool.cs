using System.Collections.Generic;
using UnityEngine;

namespace DuelGame
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private readonly Queue<T> _pool = new Queue<T>();
        private readonly T _prefab;
        private readonly Transform _parent;
        
        public ObjectPool(T prefab, int initSize, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;

            for(var i = 0; i< initSize; i++)
            {
                var x = Object.Instantiate(_prefab, parent);
                x.gameObject.SetActive(false);
                _pool.Enqueue(x);
            }
        }

        public T Get()
        {
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
            obj.transform.SetParent(_parent);    
            obj.gameObject.SetActive(false);            
            _pool.Enqueue(obj);
        }
    }
}

