﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{

    public interface IPoolableCreator
    {

        void DestroyPoolable(IPoolable poolable);

    }

}