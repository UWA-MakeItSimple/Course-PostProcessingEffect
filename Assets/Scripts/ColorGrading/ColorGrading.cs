using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ColorGrading : MonoBehaviour
{
    Material ColorGradingMaterial;
    [Range(0, 360)]
    public int Hue = 0;

    private void OnEnable()
    {
        ColorGradingMaterial = new Material(Shader.Find("ImageEffect/HSV"));
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, ColorGradingMaterial, 0);
    }
}
