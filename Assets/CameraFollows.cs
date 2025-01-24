/*
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // 追従するターゲット（ロボット）
    public float distance = 10.0f;  // ターゲットからの距離
    public float height = 5.0f;     // ターゲットからの高さ
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;

    void LateUpdate()
    {
        if (!target)
            return;

        // ターゲットの回転角度と高さを計算
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;
        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // 回転と高さをスムーズに補間
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // 回転を適用
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // カメラの位置を設定
        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // ターゲットを向く
        transform.LookAt(target);
    }
}
*/

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    public Transform target;  // ロボット（ターゲット）のTransform
    public float smoothSpeed = 0.125f;  // カメラの追従速度
    public Vector3 offset;  // カメラのオフセット位置

    void LateUpdate()
    {
        // ターゲットの位置にオフセットを適用
        Vector3 desiredPosition = target.position + offset;

        // スムーズにカメラを移動
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // ターゲットを注視するように回転
        transform.LookAt(target);
    }
    /*
    public Transform target;  // 固定する対象オブジェクト（TurtleBot3など）
    public Vector3 offset;    // オフセット位置

    void LateUpdate()
    {
        // カメラの位置を対象オブジェクトの位置+オフセットに固定
        transform.position = target.position + offset;

        // カメラが対象オブジェクトを常に見るようにする
        transform.LookAt(target);
    }
    */
}