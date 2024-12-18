using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeInGame : MonoBehaviour
{
    TextMeshProUGUI timeDisplay;
    void Start()
    {
        timeDisplay = GetComponent<TextMeshProUGUI>();
        GameManager.instance.Subscribe<GameOverState>(TypeOfEvent.GameOver, state => DisplayTimeInEndGame(state));
        StartCoroutine(Timing());
    }

    int secondCount, minuteCount;
    IEnumerator Timing()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            secondCount++;
            if (secondCount > 59)
            {
                secondCount = 0;
                minuteCount++;
            }
            timeDisplay.text = $"{minuteCount}m{secondCount}s";
        }
    }

    void DisplayTimeInEndGame(GameOverState state)
    {
        GamePlayUIManager.instance.resultText.text += " after " + timeDisplay.text;
        StopAllCoroutines();
        timeDisplay.text = state.ToString() + " at\n" + DateTime.Now.ToString();
    }
}
