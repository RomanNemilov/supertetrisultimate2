using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIControllerGame : MonoBehaviour
{

    public Button buttonBack;
    public Button buttonPlay;
    public Label labelScore;
    private Game _game;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        buttonBack = root.Q<Button>("ButtonBack");
        buttonPlay = root.Q<Button>("ButtonPlay");
        labelScore = root.Q<Label>("LabelScore");

        buttonBack.clicked += ButtonBackClicked;
        buttonPlay.clicked += ButtonPlayClicked;

        //game = gameObject.AddComponent<Board>();
        _game = GetComponent<Game>();

        _game.ScoreChanged += UpdateScore;
    }

    private void UpdateScore()
    {
        labelScore.text = _game.Score.ToString();
    }

    private void ButtonPlayClicked()
    {
        _game.StartGame();
    }

    private void ButtonBackClicked()
    {
        SceneManager.LoadSceneAsync("Start menu");
    }
}
