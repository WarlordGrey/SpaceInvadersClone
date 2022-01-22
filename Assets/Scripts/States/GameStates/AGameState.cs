using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AGameState
{

    private AGameState _nextState = null;

    public void Update()
    {

    }

    public virtual bool CanUseAbilities()
    {
        return false;
    }

    public abstract void Init();

}
