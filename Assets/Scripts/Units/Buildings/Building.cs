using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public enum BuildingPlacement
{
    VALID,
    INVALID,
    FIXED
}

public class Building
{
    private int _currentHealth;
    public BuildingPlacement _placement;
    private List<Material> _materials;
    public BuildingManager BuildingManager { get; }
    public bool IsFixed => _placement == BuildingPlacement.FIXED;
    public bool HasValidPlacement => _placement == BuildingPlacement.VALID;
    
    public GameObject BuildingSprite { get; }
    public Transform Transform { get; }
    public BuildingData Data { get; }

    public Building(BuildingData data)
    {
        Data = data;
        _currentHealth = data.hp;

        var g = GameObject.Instantiate(data.prefab);
        Transform = g.transform;
        BuildingManager = Transform.GetComponent<BuildingManager>();
        BuildingSprite = Transform.Find("Sprite").gameObject;
        _placement = BuildingPlacement.VALID;

        _materials = new List<Material>();
        foreach (var material in Transform.Find("Sprite").GetComponent<SpriteRenderer>().materials)
        {
            _materials.Add(new Material(material));
        }
        
        SetMaterials();
        
    }

    public void CheckValidPlacement(Vector3 mousePos)
    {
        if (_placement == BuildingPlacement.FIXED) return;
        _placement = BuildingManager.CheckPlacement(mousePos) ? BuildingPlacement.VALID : BuildingPlacement.INVALID;
    }

    public void SetMaterials()
    {
        SetMaterials(_placement);
    }

    public void Place()
    {
        _placement = BuildingPlacement.FIXED;
        Transform.GetComponent<BoxCollider2D>().isTrigger = false;
        SetMaterials();

        foreach (var resource in Data.cost)
        {
            GameResources.GAME_RESOURCES[resource.code].AddOrRemove(-resource.amount);
        }
    }

    public void SetPosition(Vector3 position)
    {
        Transform.position = position;
    }

    public void SetMaterials(BuildingPlacement placement)
    {
        List<Material> materials;

        switch (placement)
        {
            case BuildingPlacement.VALID:
                materials = _LoadMaterial("Materials/Valid");
                break;
            case BuildingPlacement.INVALID:
                materials = _LoadMaterial("Materials/Invalid");
                break;
            case BuildingPlacement.FIXED:
                materials = _materials;
                break;
            default:
                return;
        }
        Transform.Find("Sprite").GetComponent<Renderer>().materials = materials.ToArray();
    }
    
    public bool CanBuy()
    {
        return Data.CanBuy();
    }

    
    public int DataIndex
    {
        get
        {
            for (var i = 0; i < Globals.BUILDING_DATA.Length; i++)
            {
                if (Globals.BUILDING_DATA[i].code == Data.code)
                {
                    return i;
                }
            }

            return -1;
        }
    }
    
    private List<Material> _LoadMaterial(string path)
    {
        var refMat = Resources.Load(path) as Material;
        var materials = new List<Material>();
            
        for (var i = 0; i < _materials.Count; i++)
        {
            materials.Add(refMat);
        }

        return materials;
    }
}
