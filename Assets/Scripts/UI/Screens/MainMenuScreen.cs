using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UniRx;

namespace UI.Screens
{

    public class MainMenuScreen : BaseScreen
    {

        public void OnStartGameClick()
        {
            EventsHub.StartGame.OnNext(Unit.Default);
        }

        public void OnHighScoresClick()
        {
            EventsHub.GoToHighScores.OnNext(Unit.Default);
        }

    }

}