using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIControllerHighScores : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset ListEntryTemplate;

    public Button buttonBack;

    void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        // Initialize the character list controller
        ListController listController = new ListController();
        listController.InitializeList(root, ListEntryTemplate);
    }

    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        buttonBack = root.Q<Button>("ButtonBack");

        buttonBack.clicked += ButtonBackClicked;
    }

    private void ButtonBackClicked()
    {
        SceneManager.LoadSceneAsync("Start menu");
    }
}
