using System.Collections;
using System.Collections.Generic;
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
}
