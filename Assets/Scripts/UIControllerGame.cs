using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIControllerGame : MonoBehaviour
{

    public Button buttonPlay;
    private Game _game;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        buttonPlay = root.Q<Button>("ButtonPlay");

        buttonPlay.clicked += ButtonPlayClicked;

        //game = gameObject.AddComponent<Board>();
        _game = GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void ButtonPlayClicked()
    {
        _game.StartGame();
    }

}
