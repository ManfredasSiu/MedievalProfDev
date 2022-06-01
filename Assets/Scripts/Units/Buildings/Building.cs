using System.Collections;
using System.Collections.Generic;
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
    private BuildingPlacement _placement;
    private List<Material> _materials;
    //building manager
    
    public GameObject BuildingSprite { get; }
    
    public Transform Transform { get; }
    public BuildingData Data { get; }

    public Building(BuildingData data)
    {
        Data = data;
        _currentHealth = data.hp;

        var g = GameObject.Instantiate(data.prefab);
        Transform = g.transform;
        //set building manager
        BuildingSprite = Transform.Find("Sprite").gameObject;
        _placement = BuildingPlacement.VALID;

        _materials = new List<Material>();
        foreach (var material in Transform.Find("Mesh").GetComponent<SpriteRenderer>().materials)
        {
            _materials.Add(new Material(material));
        }
        
    }

    public void SetMaterials()
    {
        SetMaterials(_placement);
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
        Transform.Find("Mesh").GetComponent<Renderer>().materials = materials.ToArray();
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
