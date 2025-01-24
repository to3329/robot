using UnityEngine;

public class SpringFollowRobot : MonoBehaviour
{
    public Transform robot;  // ロボットのTransform
    public Transform springModel;  // バネの3Dモデル
    public Transform connectionPoint;  // バネの接続ポイント（固定点）

    public float restLength = 1.0f;  // バネの自然長
    public float springConstant = 5.0f;  // バネ定数（硬さ）
    public float damping = 0.1f;  // 減衰係数

    private float velocity = 0.0f;  // バネの速度

    void Update()
    {
        // ロボットと接続ポイント間の距離を計算
        float distance = Vector3.Distance(robot.position, connectionPoint.position);

        // バネの変位量を計算（自然長との差）
        float displacement = distance - restLength;
        float springForce = -springConstant * displacement;  // バネの力を計算

        // 減衰力を計算
        float dampingForce = -damping * velocity;

        // 合力を計算
        float totalForce = springForce + dampingForce;

        // 加速度を計算し、速度を更新
        float acceleration = totalForce;
        velocity += acceleration * Time.deltaTime;

        // バネの長さを計算（最小長さを0.1に制限）
        float newLength = Mathf.Max(0.1f, restLength + displacement);

        // バネの位置をロボットの位置に合わせて更新
        springModel.position = Vector3.Lerp(connectionPoint.position, robot.position, 0.5f); // バネの位置をロボットと接続ポイントの中間に設定

        // バネのスケールをY軸方向にのみ変更して伸縮を再現
        springModel.localScale = new Vector3(springModel.localScale.x, newLength, springModel.localScale.z);

        // バネの位置を調整してY座標が0以下にならないようにする
        if (springModel.position.y < 0)
        {
            springModel.position = new Vector3(springModel.position.x, 0, springModel.position.z);
        }

        // バネの向きをロボットの方向に合わせる
        Vector3 direction = robot.position - connectionPoint.position; // バネの向き
        if (direction != Vector3.zero) // ゼロベクトルでないことを確認
        {
            springModel.rotation = Quaternion.LookRotation(direction); // ロボットの向きに合わせる
        }
    }
}
