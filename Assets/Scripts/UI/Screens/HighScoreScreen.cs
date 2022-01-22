using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UniRx;

namespace UI.Screens
{

    public class HighScoreScreen : BaseScreen
    {

        public void OnBackClick()
        {
            EventsHub.GoToMainMenu.OnNext(Unit.Default);
        }

    }

}