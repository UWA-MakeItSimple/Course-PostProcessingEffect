using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DepthOfField : MonoBehaviour
{
    [SerializeField] Shader _shader;
    Material _material;

    #region PixelSize
    [SerializeField]
    float _pixelSize = 0.2f;
    public float PixelSize
    {
        get { return _pixelSize; }
        set { _pixelSize = value; }
    }
    #endregion

    #region FocalLength
    [SerializeField]
    float _focalLength = 1.4f;
    public float focalLength
    {
        get { return _focalLength; }
        set { _focalLength = value; }
    }
    #endregion

    #region Aperture Diameter
    [SerializeField]
    float _Aperture = 1.4f;
    public float Aperture
    {
        get { return _Aperture; }
        set { _Aperture = value; }
    }
    #endregion

    #region FocusDistance
    [SerializeField]
    Transform _pointOfFocus;
    public Transform pointOfFocus
    {
        get { return _pointOfFocus; }
        set { _pointOfFocus = value; }
    }

    [SerializeField]
    float _focusDistance;
    public float focusDistance
    {
        get { return _focusDistance; }
        set { _focusDistance = value; }
    }

    float CalculateFocusDistance()
    {
        if (_pointOfFocus == null) return focusDistance;
        var cam = TargetCamera.transform;
        return Vector3.Dot(_pointOfFocus.position - cam.position, cam.forward);
    }
    #endregion

    Camera TargetCamera
    {
        get { return GetComponent<Camera>(); }
    }

    void SetUpShaderParameters(RenderTexture source)
    {
        if (_material == null)
        {
            _material = new Material(_shader);
            _material.hideFlags = HideFlags.HideAndDontSave;
        }
        var focusDistanceTmp = CalculateFocusDistance();
        var MaxBgdCoC = (Aperture * focalLength) / (focusDistanceTmp - focalLength);
        //var f = CalculateFocalLength();
        //s1 = Mathf.Max(s1, f);
        Debug.Log("_FocusDistance:" + focusDistanceTmp + "|_MaxBgdCoc:" + MaxBgdCoC);
        _material.SetFloat("_FocusDistance", focusDistanceTmp);
        _material.SetFloat("_MaxBgdCoc", MaxBgdCoC);
        _material.SetFloat("_PixelSize", PixelSize);

        //var coeff = f * f / (_fNumber * (s1 - f) * kFilmHeight * 2);
        //_material.SetFloat("_LensCoeff", coeff);

        //var maxCoC = CalculateMaxCoCRadius(source.height);
        //_material.SetFloat("_MaxCoC", maxCoC);
        //_material.SetFloat("_RcpMaxCoC", 1 / maxCoC);

        //var rcpAspect = (float)source.height / source.width;
        //_material.SetFloat("_RcpAspect", rcpAspect);
    }

    void OnEnable()
    {
        // Check system compatibility.
        if (!_shader.isSupported) return;
        if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf)) return;

        // Initialize temporary objects (only when not set up yet).


        // Requires camera depth texture.
        TargetCamera.depthTextureMode |= DepthTextureMode.Depth;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SetUpShaderParameters(source);
        RenderTexture debug = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear);
        RenderTexture CocRT = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear);
        Graphics.Blit(source, CocRT, _material, 0);
        _material.SetTexture("_CocTex", CocRT);
        int width = source.width / 2;
        int height = source.height / 2;
        RenderTextureFormat format = source.format;
        RenderTexture tmpRT0 = RenderTexture.GetTemporary(width, height, 0, format);
        RenderTexture tmpRT1 = RenderTexture.GetTemporary(width, height, 0, format);
        Graphics.Blit(source, tmpRT0, _material, 1);
        Graphics.Blit(tmpRT0, tmpRT1, _material, 2);
        Graphics.Blit(tmpRT1, tmpRT0, _material, 3);
         _material.SetTexture("_BokehTexture", tmpRT0);
        //Graphics.Blit(source, debug, _material, 4);
        Graphics.Blit(source, destination, _material, 4);
        //Graphics.Blit(source, destination);
        RenderTexture.ReleaseTemporary(CocRT);
        RenderTexture.ReleaseTemporary(tmpRT0);
        RenderTexture.ReleaseTemporary(tmpRT1);
    }
}
