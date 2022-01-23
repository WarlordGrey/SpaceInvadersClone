using Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{

    public class Pool : MonoBehaviour
    {

        [SerializeField]
        protected int amountForCreate = 10;

        private List<Stack<IPoolable>> poolList = new List<Stack<IPoolable>>();

        public GameObject GetFromPool(IPoolable origin, Transform newParent)
        {
            CreateNewObjects(origin);
            if (poolList.Count <= 0)
            {
                return null;
            }
            IPoolable retVal = poolList[origin.GetIdType()].Pop();
            retVal.GetGameObject().transform.parent = newParent;
            retVal.Show();
            return retVal.GetGameObject();
        }

        public void ReturnToPool(IPoolable forPool)
        {
            forPool.Hide();
            forPool.GetGameObject().transform.parent = transform;
            poolList[forPool.GetIdType()].Push(forPool);
        }

        protected virtual void CreateNewObjects(IPoolable origin)
        {
            if (HasObjectsInPoolWithId(origin))
            {
                return;
            }
            for (int i = 0; i < amountForCreate; i++)
            {
                CreateSingleObject(origin);
            }
        }

        protected bool HasObjectsInPoolWithId(IPoolable origin)
        {
            while (origin.GetIdType() >= poolList.Count)
            {
                poolList.Add(new Stack<IPoolable>());
            }
            if (poolList[origin.GetIdType()].Count > 0)
            {
                return true;
            }
            return false;
        }

        protected virtual GameObject CreateSingleObject(IPoolable origin)
        {
            GameObject go = Instantiate(origin.GetGameObject(), gameObject.transform);
            IPoolable poolable = go.GetComponent<IPoolable>();
            poolable.SetIdType(origin.GetIdType());
            poolable.Hide();
            poolList[origin.GetIdType()].Push(poolable);
            return go;
        }

        internal BaseEnemyController GetFromPool()
        {
            throw new NotImplementedException();
        }
    }

}