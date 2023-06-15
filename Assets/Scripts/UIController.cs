using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    public Button buttonStart;
    public Button buttonExit;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        buttonStart = root.Q<Button>("ButtonStart");
        buttonExit = root.Q<Button>("ButtonExit");

        buttonStart.clicked += ButtonStartClicked;
        buttonExit.clicked += ButtonExitClicked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ButtonStartClicked()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }
    private void ButtonExitClicked()
    {
        Application.Quit();
        Debug.Log("Game closed");
    }
}
