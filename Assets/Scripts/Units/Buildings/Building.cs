using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private SpriteRenderer _spriteRenderer;
    public BuildingManager BuildingManager { get; }
    public bool IsFixed => _placement == BuildingPlacement.FIXED;
    public bool HasValidPlacement => _placement == BuildingPlacement.VALID;
    
    public GameObject BuildingObject { get; }
    public GameObject BuildingSprite { get; }
    public Transform Transform { get; }
    public BuildingData Data { get; }

    public Building(BuildingData data)
    {
        Data = data;
        _currentHealth = data.hp;

        BuildingObject = GameObject.Instantiate(data.prefab);
        Transform = BuildingObject.transform;
        BuildingManager = Transform.GetComponent<BuildingManager>();
        BuildingSprite = Transform.Find("Sprite").gameObject;
        _spriteRenderer = BuildingSprite.GetComponent<SpriteRenderer>();
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

    public virtual void Place()
    {
        _placement = BuildingPlacement.FIXED;
        Utils.AddToBuildingsDict(Data.code, BuildingObject);
        
        //Transform.GetComponent<Collider2D>().isTrigger = false;
        SetMaterials();
        
        GameResources.ModifyResources(Data.cost, true);
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
        _spriteRenderer.materials = materials.ToArray();
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
