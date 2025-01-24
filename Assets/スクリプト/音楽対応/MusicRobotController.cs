using UnityEngine;
using System.Collections;




using UnityEngine;
using System.Collections;

public class MusicRobotController : MonoBehaviour
{
    public enum ActionType
    {
        Forward,     // ëOêi
        Backward,    // å„êi
        RotateRight, // âEâÒì]
        RotateLeft   // ç∂âÒì]
    }

    public void ExecuteAction(ActionType actionType, float duration, float parameter)
    {
        Debug.Log($"Executing {actionType} for {duration} seconds with parameter {parameter}");

        switch (actionType)
        {
            case ActionType.Forward:
                StartCoroutine(Move(Vector3.forward, duration, parameter));
                break;
            case ActionType.Backward:
                StartCoroutine(Move(Vector3.back, duration, parameter));
                break;
            case ActionType.RotateRight:
                StartCoroutine(Rotate(Vector3.up, duration, parameter));
                break;
            case ActionType.RotateLeft:
                StartCoroutine(Rotate(Vector3.down, duration, parameter));
                break;
        }
    }

    private IEnumerator Move(Vector3 direction, float duration, float speed)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Rotate(Vector3 axis, float duration, float angularSpeed)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.Rotate(axis * angularSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}




/*

public class MusicRobotController : MonoBehaviour
{
    public enum ActionType
    {
        Forward,     // ëOêi
        Backward,    // å„êi
        RotateRight, // âEâÒì]
        RotateLeft   // ç∂âÒì]
    }
    public float speed = 1.0f;
    public float anglespeed = 45.0f;

    public void ExecuteAction(ActionType actionType, float duration)
    {
        Debug.Log($"Executing {actionType} for {duration} seconds");

        switch (actionType)
        {
            case ActionType.Forward:
                StartCoroutine(Move(Vector3.forward, duration));
                break;
            case ActionType.Backward:
                StartCoroutine(Move(Vector3.back, duration));
                break;
            case ActionType.RotateRight:
                StartCoroutine(Rotate(Vector3.up, duration));
                break;
            case ActionType.RotateLeft:
                StartCoroutine(Rotate(Vector3.down, duration));
                break;
        }
    }

    private IEnumerator Move(Vector3 direction, float duration)
    {
        float elapsedMove = 0f;

        while (elapsedMove < duration * speed)
        {
            Debug.Log("Ç†Ç†Ç†Ç†Ç†Ç†Ç†Ç†");
            transform.Translate(direction  *  Time.deltaTime);
            elapsedMove += speed * Time.deltaTime;
            yield return null;
        }
    }

    
   


    private IEnumerator Rotate(Vector3 axis, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.Rotate(axis * 45f * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}

*/

/*
public class MusicRobotController : MonoBehaviour
{
    public enum ActionType
    {
        Forward = 4, // ëOêi
        Backward = 3, // å„ëﬁ
        RotateRight = 2, // âEâÒì]
        RotateLeft = 1 // ç∂âÒì]
    }

    public void ExecuteAction(ActionType actionType, float duration)
    {
        Debug.Log($"ìÆçÏ: {actionType}, éûä‘: {duration}ïb");

        switch (actionType)
        {
            case ActionType.Forward:
                StartCoroutine(MoveForward(duration));
                break;
            case ActionType.Backward:
                StartCoroutine(MoveBackward(duration));
                break;
            case ActionType.RotateRight:
                StartCoroutine(RotateRight(duration));
                break;
            case ActionType.RotateLeft:
                StartCoroutine(RotateLeft(duration));
                break;
        }
    }

    private IEnumerator MoveForward(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.Translate(Vector3.forward  * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator MoveBackward(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.Translate(Vector3.back  *Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RotateRight(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 90f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RotateLeft(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.Rotate(Vector3.down * Time.deltaTime * 90f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
*/