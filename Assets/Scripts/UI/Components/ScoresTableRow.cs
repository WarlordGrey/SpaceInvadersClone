using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Data.Player;

public class ScoresTableRow : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _dateText;

    public void UpdateRow(HighScoreData scoreData)
    {
        _scoreText.text = scoreData.score.ToString("D4");
        _dateText.text = scoreData.scoreDate.ToString("MM/dd/yyyy HH:mm");
    }

}
