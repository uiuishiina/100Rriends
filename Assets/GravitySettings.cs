using UnityEngine;


[CreateAssetMenu(fileName = "GravitySettings", menuName = "Gravity Settings")]
public class GravitySettings : ScriptableObject
{
    [Header("重力設定")]
    [Tooltip("重力の強さ")]
    public float gravityStrength = 9.81f;

    [Tooltip("重力の方向")]
    public Vector3 gravityDirection = Vector3.down;

    [Tooltip("最大落下速度")]
    public float maxFallSpeed = 50f;

    // 重力ベクトルを取得
    public Vector3 GetGravityVector()
    {
        return gravityDirection.normalized * gravityStrength;
    }
}
