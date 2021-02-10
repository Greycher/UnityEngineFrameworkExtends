using System.Collections;
using System.Collections.Generic;
using Mobge;
using UnityEngine;

public static class ExtendedAReusableItem
{
    public static void PlayAll(this AReusableItem[] effects)
    {
        foreach (var effect in effects)
        {
            effect.Play();
        }
    }
    
    public static void PlayAll(this AReusableItem[] effects, Transform parent)
    {
        foreach (var effect in effects)
        {
            effect.transform.SetParent(parent);
            effect.Play();
        }
    }
}
