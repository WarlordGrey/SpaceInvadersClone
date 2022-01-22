using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Player
{
    public class HighScoreDataComparer : IComparer<HighScoreData>
    {

        public int Compare(HighScoreData x, HighScoreData y)
        {
            return x.score.CompareTo(y.score);
        }

    }

}