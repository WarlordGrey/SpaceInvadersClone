using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoresTableRow : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _dateText;

    public void UpdateRow(int score, DateTime dateTime)
    {
        _scoreText.text = score.ToString("D4");
        _dateText.text = dateTime.ToString("MM/dd/yyyy HH:mm");
    }

}
