using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    private static Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 toIso(this Vector3 input) => isoMatrix.MultiplyPoint3x4(input);
}
