using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CutoutMaskUI : Image
{
    // taken from a really cool tutorial: https://www.youtube.com/watch?v=XJJl19N2KFM

    public override Material materialForRendering {
        get {
            Material material = new Material(base.materialForRendering);
            material.SetInt("_StencilComp", (int) CompareFunction.NotEqual);
            return material;
        }
    }
}
