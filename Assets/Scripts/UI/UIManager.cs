using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Transform resourcesUIParent;
    public GameObject gameResourceDisplayPrefab;

    private Dictionary<string, TMP_Text> _resourceTexts = new Dictionary<string, TMP_Text>();

    private void Awake()
    {
        foreach (var resource in GameResources.GAME_RESOURCES)
        {
            var display = Instantiate(gameResourceDisplayPrefab, resourcesUIParent);
            display.name = resource.Value.Name;
            _resourceTexts[resource.Value.Name] = display.transform.Find("ResourceName").GetComponent<TMP_Text>();
            _SetResourceText(resource.Value.Name, resource.Value.Name);
            _resourceTexts[resource.Value.Name] = display.transform.Find("Amount").GetComponent<TMP_Text>();
            _SetResourceText(resource.Value.Name, resource.Value.Amount.ToString());
        }
    }
    
    private void _SetResourceText(string resource, string value)
    {
        _resourceTexts[resource].text = value;
    }
}
