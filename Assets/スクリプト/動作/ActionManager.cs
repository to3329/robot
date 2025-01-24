using System.Collections.Generic;
using UnityEngine;
using System.IO;


using UnityEngine.UI;
using System.Collections;

using TMPro;

using Newtonsoft.Json;
using System;
using System.Text;


[System.Serializable]
public class ActionData
{
    public string ActionType; // 前進、後進、右回転、左回転
    public float Speed;
    public float Time;
    public float Angle;
    public float AngularSpeed;
}


[System.Serializable]
public class ActionDataListWrapper
{
    public List<ActionData> actions; // JSONに保存するリスト
}

public class ActionManager : MonoBehaviour
{
    private Queue<ActionData> actionQueue = new Queue<ActionData>();
    private Queue<ActionData> recordQueue = new Queue<ActionData>();

    public TMP_InputField inputField; // TMP_InputField を設定
    public GameObject inputFieldObject; // TMP_InputField を含む GameObject
    public ScrollViewButtonManager scrollViewA;
    public Transform contentParent;


    public ScrollViewButtonManager scrollViewManager; // ボタン管理スクリプト

    private void Start()
    {
        // 初期状態では TMP_InputField を非表示にする
        if (inputFieldObject != null)
        {
            inputFieldObject.SetActive(false);
        }
    }
    


    // 動作をキューに登録
    public void RegisterAction(ActionData action)
    {
        actionQueue.Enqueue(action);
        recordQueue.Enqueue(action);
        Debug.Log($"動作をキューに登録: {action.ActionType}");
    }

    // 動作を実行
    public void ExecuteActions()
    {
        if (actionQueue.Count > 0)
        {

            ActionData action = actionQueue.Dequeue();
            Debug.Log("動作" + action.Speed);
            StartCoroutine(ExecuteActionCoroutine(action));
        }
        else
        {
            Debug.Log("キューに動作がありません");
        }
    }

    // コルーチンで動作を実行
    private IEnumerator ExecuteActionCoroutine(ActionData action)
    {

        float distanceMoved = 0f;
        float totalDistance = action.Time * action.Speed;
        switch(action.ActionType)
            {
            case "前進":
                while (distanceMoved < totalDistance)
                {
                    float moveStep = action.Speed * Time.deltaTime;

                    // オブジェクトを前進させる
                    transform.Translate(Vector3.forward * moveStep);
                    distanceMoved += moveStep;

                    // 次のフレームまで待機
                    yield return null;
                }
                break;
                
            case "後進":
                while (distanceMoved < totalDistance)
                {
                    float moveStep = action.Speed * Time.deltaTime;

                    // オブジェクトを前進させる
                    transform.Translate(Vector3.back * moveStep);
                    distanceMoved += moveStep;

                    // 次のフレームまで待機
                    yield return null;
                }
                break;
                
            case "右回転"://///Vector3.up
                float rightangleRotated = 0f;
                
                while (rightangleRotated < action.Angle)
                {
                    float rotateStep = action.AngularSpeed * Time.deltaTime;
                    transform.Rotate(Vector3.up, rotateStep);
                    rightangleRotated += rotateStep;
                    yield return null;
                }
                break;
            case "左回転"://///Vector3.up
                float leftangleRotated = 0f;

                while (leftangleRotated < action.Angle)
                {
                    float rotateStep = action.AngularSpeed * Time.deltaTime;
                    transform.Rotate(Vector3.down, rotateStep);
                    leftangleRotated += rotateStep;
                    yield return null;
                }
                break;
            default:
                break;

            }

        

        Debug.Log($"動作完了: {action.ActionType}");

        // 次の動作を実行
        if (actionQueue.Count > 0)
        {
            ExecuteActions();
        }
        else
        {
            Debug.Log("すべての動作が完了しました");
        }
    }


    

   
    public void SaveActions()
    {
        // TMP_InputField を表示してファイル名を入力できるようにする
        if (inputFieldObject != null)
        {
            inputFieldObject.SetActive(true);
        }
    }

    /*
    // 入力完了後に保存を実行
    public void OnSaveFileNameEntered()
    {
        if (inputField == null)
        {
            Debug.LogError("TMP_InputField が設定されていません！");
            return;
        }

        // 入力されたファイル名を取得
        string fileName = inputField.text;

        if (string.IsNullOrWhiteSpace(fileName))
        {
            Debug.LogError("ファイル名が入力されていません！");
            return;
        }

        // ファイルパスを生成
        string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");

        // キューからリストに変換して保存
        List<ActionData> actions = new List<ActionData>(recordQueue);

        foreach (var action in actions)
        {
            Debug.Log($"ActionType: {action.ActionType}, Speed: {action.Speed}, Time: {action.Time}, Angle: {action.Angle}, AngularSpeed: {action.AngularSpeed}");
        }

        // JSON にシリアライズ
        string json = JsonConvert.SerializeObject(actions, Formatting.Indented);
        File.WriteAllText(filePath, json);

        Debug.Log($"動作を保存しました: {filePath}");



        scrollViewA.AddButtonToScrollView(fileName);

        // TMP_InputField を非表示にしてリセット
        inputField.text = "";
        inputFieldObject.SetActive(false);
        
    }
    */


    public void OnSaveFileNameEntered()
    {
        if (inputField == null)
        {
            Debug.LogError("TMP_InputField が設定されていません！");
            return;
        }

        string fileName = inputField.text;

        if (string.IsNullOrWhiteSpace(fileName))
        {
            Debug.LogError("ファイル名が入力されていません！");
            return;
        }

        string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");

        // キューからリストに変換して保存
        List<ActionData> actions = new List<ActionData>(recordQueue);

        string json = JsonConvert.SerializeObject(actions, Formatting.Indented);
        File.WriteAllText(filePath, json);

        Debug.Log($"動作を保存しました: {filePath}");

        // スクロールビューAにボタンを追加
        scrollViewA.AddButtonToScrollView(fileName);

        inputField.text = "";
        inputFieldObject.SetActive(false);
    }




    public void Delete()
    {
        actionQueue.Clear();
        recordQueue.Clear();
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject); // 子オブジェクトを削除
        }
        Debug.Log($"全削除されました");
    }



    // JSONファイルから動作を読み込み
    public void LoadActions(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            List<ActionData> actions = JsonUtility.FromJson<List<ActionData>>(json);
            foreach (var action in actions)
            {
                RegisterAction(action);
            }
            Debug.Log($"動作を読み込み: {filePath}");
        }
        else
        {
            Debug.LogError($"ファイルが見つかりません: {filePath}");
        }
    }
}
