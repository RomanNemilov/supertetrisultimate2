using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    public Button buttonStart;
    public Button buttonExit;
    public Button buttonHighScores;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        buttonStart = root.Q<Button>("ButtonStart");
        buttonExit = root.Q<Button>("ButtonExit");
        buttonHighScores = root.Q<Button>("ButtonHighScores");

        buttonStart.clicked += ButtonStartClicked;
        buttonExit.clicked += ButtonExitClicked;
        buttonHighScores.clicked += ButtonHighScoresClicked;
    }


    private void ButtonStartClicked()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }

    private void ButtonExitClicked()
    {
        SceneManager.LoadSceneAsync("ConfirmationScene");
    }

    private void ButtonHighScoresClicked()
    {
        SceneManager.LoadSceneAsync("HighScoreScene");
    }
}
