using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Utils
{

    public interface IPoolable
    {

        void Hide();
        void Show();
        GameObject GetGameObject();
        int GetIdType();
        void SetIdType(int newID);

    }

}