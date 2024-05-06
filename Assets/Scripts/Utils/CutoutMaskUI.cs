using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CutoutMaskUI : Image
{
    [SerializeField] private float timeReFresh = 0.1f;
    private float _timeReFreshRun = 0;
    public override Material materialForRendering
    {
        get
        {
            Material mat = new Material(base.materialForRendering);
            mat.SetInt("_StencilComp",(int)CompareFunction.NotEqual);
            return mat;
        }
    }

    private void Update()
    {
        _timeReFreshRun += Time.fixedDeltaTime;
        if(_timeReFreshRun >= timeReFresh)
        {
            SetMaterialDirty();
            _timeReFreshRun = 0;
        }
    }
}
