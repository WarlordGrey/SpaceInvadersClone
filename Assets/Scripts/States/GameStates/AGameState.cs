using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AGameState
{

    private AGameState _nextState = null;

    public void Update()
    {

    }

    protected void UpdateNextState(AGameState nextState)
    {
        _nextState = nextState;
    }
    
    public AGameState GetNextState()
    {
        if(_nextState == null)
        {
            return GetStandardNextState();
        }
        return _nextState;
    }

    protected abstract AGameState GetStandardNextState();

    public abstract void Init();

}
