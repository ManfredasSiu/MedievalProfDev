using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TransformUtilities
{
    public static Vector3 TransformPositionWithOffset(this GameObject gameObject)
    {
        var renderer = gameObject.GetComponent<SpriteRenderer>();
        var transformPosition = gameObject.transform.position;
        
        if (renderer == null)
        {
            return transformPosition;
        }

        var yDelta = new Vector3(0, renderer.bounds.extents.y);

        return transformPosition - yDelta;
    }
    
    public static Vector3 GetYDeltaForTransform(this GameObject gameObject)
    {
        var renderer = gameObject.GetComponent<SpriteRenderer>();
        if (renderer == null)
        {
            return Vector3.zero;
        }
        
        var yDelta = new Vector3(0, renderer.bounds.extents.y);

        return yDelta;
    }

    public static void GetUpperAndLowerPos(this Vector3 currentPos, SpriteRenderer[] renderers, out Vector3 lowerLeftPos, out Vector3 upperRightPos)
    {
        var bounds = renderers.Select(x => x.bounds);

        var center = Vector3.zero;

        foreach (var child in bounds)
        {
            center += child.center;   
        }
        center /= renderers.Length;

        var tempBounds = new Bounds(center,Vector3.zero); 

        foreach (var child in bounds)
        {
            tempBounds.Encapsulate(child);   
        }
        
        lowerLeftPos = currentPos;
        upperRightPos = currentPos+tempBounds.size;
    }
}
