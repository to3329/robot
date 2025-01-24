using UnityEngine;
using System.Collections;

public class RobotCombination : MonoBehaviour
{
    public float speed1 = 5.0f;           // ロボットの移動速度
    public float speed2 = 1.0f;          // ロボットの移動速度
    public float speed3 = 3.0f;           // ロボットの移動速度
    public float speed4 = 2.0f;           // ロボットの移動速度
    public float rotationSpeed1 = 90.0f;  // ロボットの回転速度
    public float rotationSpeed2 = 180.0f; // ロボットの回転速度
    public float rotationSpeed3 = 45.0f;  // ロボットの回転速度
    public float rotationSpeed4 = 120.0f; // ロボットの回転速度


    void Start()
    {
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        yield return new WaitForSeconds(1.5f);

        // 前進
        float moveDuration = 1.0f;
        float moveTime = 0.0f;
        while (moveTime < moveDuration)
        {
            transform.Translate(Vector3.forward * speed2 * Time.deltaTime);
            moveTime += Time.deltaTime;
            yield return null;
        }
        //　左右回転
        float rotationDuration = 1.0f;
        float rotationTime = 0.0f;
        while (rotationTime < rotationDuration)
        {
            transform.Rotate(Vector3.up * rotationSpeed1 * Time.deltaTime);
            rotationTime += Time.deltaTime;
            yield return null;
        }

        // 前進
        moveDuration = 1.0f;
        moveTime = 0.0f;
        while (moveTime < moveDuration)
        {
            transform.Translate(Vector3.forward * speed2 * Time.deltaTime);
            moveTime += Time.deltaTime;
            yield return null;
        }
        //後進
        moveDuration = 1.0f;
        moveTime = 0.0f;
        while (moveTime < moveDuration)
        {
            transform.Translate(Vector3.back * speed2 * Time.deltaTime);
            moveTime += Time.deltaTime;
            yield return null;
        }
    }
}
