using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 事件渗透
/// </summary>
public class GuidanceEventPenetrate : MonoBehaviour, ICanvasRaycastFilter
{
    private RectTransform targetImage;
    public void SetTargetImage(RectTransform target)
    {
        targetImage = target;
    }
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (targetImage == null)
        {
            return true;
        }
       // return !RectTransformUtility.RectangleContainsScreenPoint(targetImage, sp, eventCamera);
        return true;
    }
}
