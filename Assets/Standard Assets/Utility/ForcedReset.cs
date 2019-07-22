using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// Add by zx 18.1.15
/// </summary>
[RequireComponent(typeof (GUITexture))]
public class ForcedReset : MonoBehaviour
{
    private void Update()
    {
        // if we have forced a reset ...
        if (CrossPlatformInputManager.GetButtonDown("ResetObject"))
        {
            //... reload the scene
            //Application.LoadLevelAsync(Application.loadedLevelName);
        }
    }
}
