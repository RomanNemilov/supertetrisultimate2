using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIControllerGame : MonoBehaviour
{

    public Button buttonPlay;
    private Board game;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        buttonPlay = root.Q<Button>("ButtonPlay");

        buttonPlay.clicked += ButtonPlayClicked;

        //game = gameObject.AddComponent<Board>();
        game = GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void ButtonPlayClicked()
    {
        game.StartGame();
    }

}
