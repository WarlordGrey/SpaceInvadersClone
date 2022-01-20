using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States.GameStates
{

    public class FinishedState : AGameState
    {
        public override void Init()
        {
            
        }

        protected override AGameState GetStandardNextState()
        {
            return new WaitGameState();
        }

    }

}