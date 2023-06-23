using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIControllerConfirmation : MonoBehaviour
{

    public Button buttonYes;
    public Button buttonNo;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        buttonYes = root.Q<Button>("ButtonYes");
        buttonNo = root.Q<Button>("ButtonNo");

        buttonYes.clicked += ButtonYesClicked;
        buttonNo.clicked += ButtonNoClicked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ButtonYesClicked()
    {
        Application.Quit();
        Debug.Log("Game closed");
    }
    private void ButtonNoClicked()
    {
        SceneManager.LoadSceneAsync("Start menu");
    }
}
