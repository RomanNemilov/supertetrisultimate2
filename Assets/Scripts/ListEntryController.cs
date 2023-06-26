using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ListEntryController
{
    Label ScoreLabel;
    Label TimeLabel;

    //This function retrieves a reference to the 
    //character name label inside the UI element.

    public void SetVisualElement(VisualElement visualElement)
    {
        ScoreLabel = visualElement.Q<Label>("ScoreLabel");
        TimeLabel = visualElement.Q<Label>("TimeLabel");
    }

    //This function receives the character whose name this list 
    //element displays. Since the elements listed 
    //in a `ListView` are pooled and reused, it's necessary to 
    //have a `Set` function to change which character's data to display.

    public void SetHighScoreData(HighScore highScore)
    {
        ScoreLabel.text = highScore.score.ToString();
        TimeLabel.text = highScore.dateTime;
        Debug.Log("High score added to listview");
    }
}
