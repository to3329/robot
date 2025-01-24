using UnityEngine;
using System.Collections;

public class RobotMove : MonoBehaviour
{
    /*
    public float speed = 5.0f;           // ロボットの移動速度
    public float rotationSpeed = 90.0f;  // ロボットの回転速度
    public float maxActionDuration = 3.0f; // 最大動作時間

    private enum ActionType
    {
        MoveForward,
        MoveBackward,
        RotateRight,
        RotateLeft
    }

    void Start()
    {
        StartCoroutine(DanceRoutine());
    }

    IEnumerator DanceRoutine()
    {
        float totalDuration = 20.0f; // ダンス全体の時間
        float elapsedTime = 0.0f;

        while (elapsedTime < totalDuration)
        {
            // ランダムな動作を選択
            ActionType action = (ActionType)Random.Range(0, 4);
            float actionDuration = Random.Range(0.5f, maxActionDuration); // 最小0.5秒から最大maxActionDurationの範囲

            // 選択した動作を実行
            switch (action)
            {
                case ActionType.MoveForward:
                    yield return StartCoroutine(MoveForward(actionDuration));
                    break;
                case ActionType.MoveBackward:
                    yield return StartCoroutine(MoveBackward(actionDuration));
                    break;
                case ActionType.RotateRight:
                    yield return StartCoroutine(RotateRight(actionDuration));
                    break;
                case ActionType.RotateLeft:
                    yield return StartCoroutine(RotateLeft(actionDuration));
                    break;
            }

            // 経過時間を更新
            elapsedTime += actionDuration;
        }
    }

    IEnumerator MoveForward(float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator MoveBackward(float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator RotateRight(float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator RotateLeft(float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    */



    public float speed = 5.0f;           // ロボットの移動速度
    public float rotationSpeed = 90.0f;  // ロボットの回転速度
    public float maxActionDuration = 3.0f; // 最大動作時間

    private enum ActionType
    {
        MoveForward,
        MoveBackward,
        RotateRight,
        RotateLeft,
        MoveDiagonalForwardRight,
        MoveDiagonalForwardLeft,
        MoveDiagonalBackwardRight,
        MoveDiagonalBackwardLeft,
        CurveRight,
        CurveLeft
    }

    void Start()
    {
        StartCoroutine(DanceRoutine());
    }

    IEnumerator DanceRoutine()
    {
        float totalDuration = 30.0f; // ダンス全体の時間
        float elapsedTime = 0.0f;

        while (elapsedTime < totalDuration)
        {
            // ランダムな動作を選択
            ActionType action = (ActionType)Random.Range(0, 10);
            float actionDuration = Random.Range(0.5f, maxActionDuration); // 0.5秒から最大3秒の範囲

            // 選択した動作を実行
            switch (action)
            {
                case ActionType.MoveForward:
                    yield return StartCoroutine(Move(Vector3.forward, actionDuration));
                    break;
                case ActionType.MoveBackward:
                    yield return StartCoroutine(Move(Vector3.back, actionDuration));
                    break;
                case ActionType.RotateRight:
                    yield return StartCoroutine(Rotate(Vector3.up, actionDuration));
                    break;
                case ActionType.RotateLeft:
                    yield return StartCoroutine(Rotate(Vector3.up * -1, actionDuration));
                    break;
                case ActionType.MoveDiagonalForwardRight:
                    yield return StartCoroutine(Move(new Vector3(1, 0, 1).normalized, actionDuration));
                    break;
                case ActionType.MoveDiagonalForwardLeft:
                    yield return StartCoroutine(Move(new Vector3(-1, 0, 1).normalized, actionDuration));
                    break;
                case ActionType.MoveDiagonalBackwardRight:
                    yield return StartCoroutine(Move(new Vector3(1, 0, -1).normalized, actionDuration));
                    break;
                case ActionType.MoveDiagonalBackwardLeft:
                    yield return StartCoroutine(Move(new Vector3(-1, 0, -1).normalized, actionDuration));
                    break;
                case ActionType.CurveRight:
                    yield return StartCoroutine(Curve(true, actionDuration));
                    break;
                case ActionType.CurveLeft:
                    yield return StartCoroutine(Curve(false, actionDuration));
                    break;
            }

            // 経過時間を更新
            elapsedTime += actionDuration;
        }
    }

    IEnumerator Move(Vector3 direction, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator Rotate(Vector3 axis, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.Rotate(axis * rotationSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator Curve(bool right, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            Vector3 moveDirection = right ? Vector3.forward + Vector3.right : Vector3.forward + Vector3.left;
            transform.Translate(moveDirection.normalized * speed * Time.deltaTime);
            transform.Rotate(Vector3.up * (right ? rotationSpeed : -rotationSpeed) * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}

