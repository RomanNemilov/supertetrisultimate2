using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ListController
{
    private VisualTreeAsset _listEntryTemplate;
    private ListView _highScoresList;
    private List<HighScore> _highScores;

    public void InitializeList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        _highScores = TetrisHelper.GetHighScores();

        // Store a reference to the template for the list entries
        _listEntryTemplate = listElementTemplate;

        // Store a reference to the list element
        _highScoresList = root.Q<ListView>("ListViewHighScores");

        FillHighScoresList();
    }

    void FillHighScoresList()
    {
        // Set up a make item function for a list entry
        _highScoresList.makeItem = () =>
        {
            // Instantiate the UXML template for the entry
            VisualElement newListEntry = _listEntryTemplate.Instantiate();

            // Instantiate a controller for the data
            ListEntryController newListEntryLogic = new ListEntryController();

            // Assign the controller script to the visual element
            newListEntry.userData = newListEntryLogic;

            // Initialize the controller script
            newListEntryLogic.SetVisualElement(newListEntry);

            // Return the root of the instantiated visual tree
            return newListEntry;
        };

        // Set up bind function for a specific list entry
        _highScoresList.bindItem = (item, index) =>
        {
            (item.userData as ListEntryController).SetHighScoreData(_highScores[index]);
        };

        // Set a fixed item height
        _highScoresList.fixedItemHeight = 45;

        // Set the actual item's source list/array
        _highScoresList.itemsSource = _highScores;
    }
}
