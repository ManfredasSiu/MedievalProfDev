using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private BuildingPlacer _buildingPlacer;
    
    public Transform buildingMenu;
    public GameObject buildingButtonPrefab;
    public Transform resourcesUIParent;
    public GameObject gameResourceDisplayPrefab;

    private Dictionary<GameResourceEnum, TMP_Text> _resourceTexts = new Dictionary<GameResourceEnum, TMP_Text>();
    private Dictionary<BuildingEnum, Button> _buildingButtons = new Dictionary<BuildingEnum, Button>();

    private void Awake()
    {
        _buildingPlacer = GetComponent<BuildingPlacer>();
        GameResources.OnResourcesModified += UpdateUIOnPlacedBuilding;
        foreach (var resource in GameResources.GAME_RESOURCES)
        {
            var display = Instantiate(gameResourceDisplayPrefab, resourcesUIParent);
            display.name = resource.Value.code.ToString();
            _resourceTexts[resource.Value.code] = display.transform.Find("ResourceName").GetComponent<TMP_Text>();
            _SetResourceText(resource.Value.code, resource.Value.code.ToString());
            _resourceTexts[resource.Value.code] = display.transform.Find("Amount").GetComponent<TMP_Text>();
            _SetResourceText(resource.Value.code, resource.Value.amount.ToString());
        }

        for (var i = 0; i < Globals.BUILDING_DATA.Length; i++)
        {
            var button = Instantiate(buildingButtonPrefab, buildingMenu);
            var code = Globals.BUILDING_DATA[i].code;
            var unitName = Globals.BUILDING_DATA[i].unitName;
            button.transform.Find("Text").GetComponent<TMP_Text>().text = unitName;
            var b = button.GetComponent<Button>();
            _AddBuildingButtonListener(b,i);

            _buildingButtons[code] = b;
            if (Globals.BUILDING_DATA[i].CanBuy())
            {
                b.interactable = true;
            }
        }
        _RefreshBuildingsUi();
    }
    
    private void _SetResourceText(GameResourceEnum resource, string value)
    {
        _resourceTexts[resource].text = value;
    }
    
    private void UpdateUIOnPlacedBuilding()
    {
        _RefreshBuildingsUi();
    }
    
    private void _AddBuildingButtonListener(Button b, int i)
    {
        b.onClick.AddListener(() => _buildingPlacer.SelectPlacedBuilding(i));
    }
    
    private void _RefreshBuildingsUi()
    {
        UpdateResourceTexts();
        CheckBuildingButtons();
    }
    
    private void CheckBuildingButtons()
    {
        foreach (var data in Globals.BUILDING_DATA)
        {
            _buildingButtons[data.code].interactable = data.CanBuy();
        }
    }
    
    private void UpdateResourceTexts()
    {
        foreach (var pair in GameResources.GAME_RESOURCES)
        {
            _SetResourceText(pair.Key, pair.Value.amount.ToString());
        }
    }
}
