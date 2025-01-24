using UnityEngine;
using System.Collections;

public class RobotController : MonoBehaviour
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
        // 前進
        float moveDuration = 0.05f;
        float moveTime = 0.0f;
        while (moveTime < moveDuration)
        {
            transform.Translate(Vector3.forward * speed1 * Time.deltaTime);
            moveTime += Time.deltaTime;
            yield return null;
        }

        // 右に回転
        float rotationDuration = 2.0f;
        float rotationTime = 0.0f;
        while (rotationTime < rotationDuration)
        {
            transform.Rotate(Vector3.up * rotationSpeed1 * Time.deltaTime);
            rotationTime += Time.deltaTime;
            yield return null;
        }

        float circleDuration = 0.3f;  // 円を描くのにかかる時間（秒）
        float elapsedTime = 0.0f;
        while (elapsedTime < circleDuration)
        {
            // 前進しながら回転
            transform.Translate(Vector3.forward * speed3 * Time.deltaTime);
            transform.Rotate(Vector3.up * rotationSpeed3 * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;

        }
        // 後進
        moveDuration = 0.08f;
        moveTime = 0.0f;
        while (moveTime < moveDuration)
        {
            transform.Translate(Vector3.back * speed2 * Time.deltaTime);
            moveTime += Time.deltaTime;
            yield return null;
        }

        // 左に回転
        rotationDuration = 4.0f;
        rotationTime = 0.0f;
        while (rotationTime < rotationDuration)
        {
            transform.Rotate(Vector3.up * -rotationSpeed2 * Time.deltaTime); // 左回転は負の値
            rotationTime += Time.deltaTime;
            yield return null;
        }

        // 前進
        moveDuration = 0.07f;
        moveTime = 0.0f;
        while (moveTime < moveDuration)
        {
            transform.Translate(Vector3.forward * speed1 * Time.deltaTime);
            moveTime += Time.deltaTime;
            yield return null;
        }

        // 一旦停止
        yield return new WaitForSeconds(1.5f);

        // 左に小さく回転
        rotationDuration = 0.5f;
        rotationTime = 0.0f;
        while (rotationTime < rotationDuration)
        {
            transform.Rotate(Vector3.up * -rotationSpeed3 * Time.deltaTime);
            rotationTime += Time.deltaTime;
            yield return null;
        }
        moveDuration = 0.07f;
        moveTime = 0.0f;
        while (moveTime < moveDuration)
        {
            transform.Translate(Vector3.back * speed2 * Time.deltaTime);
            moveTime += Time.deltaTime;
            yield return null;
        }
        circleDuration = 0.5f;  // 円を描くのにかかる時間（秒）
        elapsedTime = 0.0f;
        while (elapsedTime < circleDuration)
        {
            // 前進しながら回転
            transform.Translate(Vector3.forward * speed3 * Time.deltaTime);
            transform.Rotate(Vector3.up * rotationSpeed3 * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;

        }

        rotationDuration = 2.0f;
        rotationTime = 0.0f;
        while (rotationTime < rotationDuration)
        {
            transform.Rotate(Vector3.up * rotationSpeed2 * Time.deltaTime);
            rotationTime += Time.deltaTime;
            yield return null;
        }

        moveDuration = 0.07f;
        moveTime = 0.0f;
        while (moveTime < moveDuration)
        {
            transform.Translate(Vector3.forward * speed3 * Time.deltaTime);
            moveTime += Time.deltaTime;
            yield return null;
        }

        rotationDuration = 0.7f;
        rotationTime = 0.0f;
        while (rotationTime < rotationDuration)
        {
            transform.Rotate(Vector3.up * rotationSpeed4 * Time.deltaTime);
            rotationTime += Time.deltaTime;
            yield return null;
        }

        moveDuration = 0.04f;
        moveTime = 0.0f;
        while (moveTime < moveDuration)
        {
            transform.Translate(Vector3.forward * speed4 * Time.deltaTime);
            moveTime += Time.deltaTime;
            yield return null;
        }

    }
}



