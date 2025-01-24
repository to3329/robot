using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Text;


public class CommandExecutor : MonoBehaviour
{

    ///////ロボット動作はできる


    private Queue<System.Func<IEnumerator>> commands = new Queue<System.Func<IEnumerator>>(); // コマンドキュー
    private Queue<CommandData> commandQueue = new Queue<CommandData>();
    List<RobotData> robotDataList = new List<RobotData>();
    private List<CommandData> commandList = new List<CommandData>();

    //public ScrollViewManager scrollViewManager;

    public TextObjectDisplayController value;
    //public TMP_Text fileNameText;

    [System.Serializable]
    public class RobotData
    {
        public float speed;
        public float rotationSpeed;
        public string action;
        public float duration;
    }

    [System.Serializable]
    public class CommandData
    {
        public string commandType;
        //public float speed;
        //public float rotationSpeed;
        //public float duration;
        public float parameter;
    }

    [System.Serializable]
    public class RobotDataWrapper
    {
        public List<RobotData> robots; // JSONに保存するリスト
    }


    public Button myButton;

    public float currentSpeed = 11.0f;
    public float currentRotationSpeed = 0.0f;
    public string currentAction = "";
    public float currentDuration = 0.0f;
    public float speed = 0.0f;
    public float rotationSpeed = 0.0f;
    public float duration = 0.0f;
    //private Queue<CommandData> commandQueue = new Queue<CommandData>();
    //private List<RobotData> robotDataList = new List<RobotData>();

    private bool isSpeedUpdated = false;
    


   


    //[Header("Parameters")]
    /*
    public float movetime = 0.8f;  // 前進・後進の移動距離
    public float rotateAngle = 90f;  // 回転角度
    public float moveSpeed = 1f;     // 移動速度
    public float rotateSpeed = 45f;  // 回転速度
    */
    //[Header("UI Elements")]
    public float movetime = 0.8f;  // 前進・後進の移動距離
    public float rotateAngle = 90f;  // 回転角度
    float moveSpeed = 1.0f;     // 移動速度
    public float rotateSpeed = 45f;  // 回転速度

    public Transform inputParent; // 親オブジェクト（Inspectorで割り当て）

    //public TMP_InputField speedInput;            // 速度入力フィールド
    public TMP_InputField timeInput;             // 時間入力フィールド
    public TMP_InputField rotationSpeedInput;    // 回転速度入力フィールド

    //public GameObject parentGameObject.transform;
    //Transform parentTransform = parentGameObject.transform; // テキストの親

    [Header("Save/Load Settings")]
    public string saveFileName = "commands.json"; // 保存ファイル名


    // スクリプト開始時に呼ばれる
    private void Start()
    {
        //float moveSpeed = 1f;     // 移動速度
        

        if (inputParent != null)
        {
            
            TMP_InputField speedInputField = FindInputFieldByName(inputParent, "SpeedInput");
            
            if (speedInputField != null)
            {
                
                //speedInputField.onEndEdit.AddListener(delegate { UpdateSpeed(speedInputField); });
                //speedInputField.onEndEdit.AddListener(delegate { AddMoveForward(speedInputField); });
                //myButton.onclick.AddListener
                Debug.Log("SpeedInput が見つかりました。リスナーを登録しました。");
            }
            else
            {
                Debug.LogError("SpeedInput が見つかりませんでした！");
            }
        }
        else
        {
            Debug.LogError("InputParentが設定されていません！");
        }


        /*
        if (timeInput != null)
        {
            timeInput.onEndEdit.AddListener(delegate { UpdateTime(); });
            Debug.Log("ある");
        }

        if (speedInput != null)
            speedInput.onEndEdit.AddListener(delegate { UpdateSpeed(); });

        if (rotationSpeedInput != null)
            rotationSpeedInput.onEndEdit.AddListener(delegate { UpdaterotationSpeed(); });
        */



    }


    private TMP_InputField FindInputFieldByName(Transform parent, string fieldName)
    {
        Debug.Log("インプットフィールド");
        foreach (TMP_InputField inputField in parent.GetComponentsInChildren<TMP_InputField>())
        {
            if (inputField.name == fieldName)
            {
                return inputField;
            }
        }
        return null;
    }



    // 入力値の反映

    public void UpdateTime()
    {


        Debug.Log("できた");

        timeInput = timeInput.GetComponent<TMP_InputField>();


        Debug.Log("できた");
        //////
        if (float.TryParse(timeInput.text, out currentDuration))
            Debug.Log("Time: " + currentDuration);
        else
            Debug.LogWarning("Invalid input for time");
        /////
    }


    private bool isExecuting = false; // 実行中かどうかのフラグ
    
    public void UpdateSpeed(TMP_InputField inputField)
    {
        Debug.Log("UpdateSpeed が呼ばれました。");
        //moveSpeed = 1f;     // 移動速度
        ///////
        if (float.TryParse(inputField.text, out moveSpeed))
        {
            Debug.Log("Move Speed: " + moveSpeed);
            //RecordRobotData(moveSpeed, 1, "MoveForward", 2);

            isSpeedUpdated = true;
            Debug.Log("Mo" + isSpeedUpdated);

        }

        else
        {
            Debug.LogWarning("Invalid input for speed");
            isSpeedUpdated = false;
        }

        RobotData data = new RobotData
        {
            speed = moveSpeed,
            rotationSpeed = rotationSpeed,
            action = "MoveForward",
            duration = duration
        };

        robotDataList.Add(data);


        //SaveToJson();

        Debug.Log($"UpdateSpeed: isSpeedUpdated = {isSpeedUpdated}"); // 状態をログで確認
        
    }
    
    public void UpdaterotationSpeed()
    {
        float.TryParse(rotationSpeedInput.text, out rotateAngle);
        //////
        if (float.TryParse(rotationSpeedInput.text, out currentRotationSpeed))
            Debug.Log("Rotate Angle: " + currentRotationSpeed);
        else
            Debug.LogWarning("Invalid input for angle");
        //////
        ///
    }


    // 「前進」コマンドをキューに追加
    //////////////////////////////////////////////////////////////////////////////////////////
    //public void AddMoveForward(string inputSpeed) TMP_InputField inputFieldStartCoroutine

    public void AddMoveForward()
    {
        float a = value.GetInputValue();
        Debug.Log("大事" + a);
        //RecordRobotData(moveSpeed, rotationSpeed, "MoveForward", currentDuration);
        Debug.Log(robotDataList.Count);
        RobotData lastData = robotDataList[robotDataList.Count - 1];

        // 最後の speed を moveSpeed に代入
        moveSpeed = lastData.speed;
        // 入力された値を float に変換


        // moveSpeed を更新

        Debug.Log($"Move Speed が更新されました: {moveSpeed}");

            // キューに MoveForward コマンドを追加
        commands.Enqueue(MoveForward);

        Debug.Log("MoveForward コマンドがキューに追加されました。");
        
       
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////
    
    /*
    public void AddMoveForward()
    {

        

        foreach (var data in robotDataList)
        {
            Debug.Log($"リストのデータ: Action: {data.action}, Speed: {data.speed}, Rotation Speed: {data.rotationSpeed}, Duration: {data.duration}");
        }
        RecordRobotData(currentSpeed, rotationSpeed, "MoveForward", currentDuration);

        SaveToJson();

        //この部分にUpdateSpeedの処理が終わるまで待機するプログラムを書いて
        StartCoroutine(WaitForUpdateSpeedAndEnqueue());
        
        //commands.Enqueue(MoveForward);
        //string queueContents = string.Join(", ", commands);

    }

    /// <summary>
    private IEnumerator WaitForUpdateSpeedAndEnqueue()
    {
        Debug.Log("UpdateSpeed処理を待機中...");



        // フラグがtrueになるまで待機
        
        //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        
        yield return new WaitUntil(() =>
        {
            Debug.Log($"Checking isSpeedUpdated: {isSpeedUpdated}"); // 条件を毎フレームログ出力
            return isSpeedUpdated;
        });
        

    

    Debug.Log("UpdateSpeedが完了しました。MoveForwardコマンドをキューに追加します。");
        Debug.Log("Upd" + moveSpeed);


        // MoveForwardコマンドをキューに追加
        commands.Enqueue(MoveForward);

        string queueContents = string.Join(", ", commands);
        Debug.Log($"コマンドキューの内容: {queueContents}");
    }Add
    */
    /// </summary>

    // 「後進」コマンドをキューに追加
    public void AddMoveBackward()
    {
        Debug.Log("Move Speed: 後ろ" + moveSpeed);
        RecordRobotData(currentSpeed, rotationSpeed, "MoveBackward", currentDuration);
        commands.Enqueue(MoveBackward);
        SaveToJson();
    }

    // 「右回転」コマンドをキューに追加
    public void AddRotateRight()
    {
        RecordRobotData(currentSpeed, rotationSpeed, "RotateRight", currentDuration);
        commands.Enqueue(() => RotateObject(Vector3.up));
        SaveToJson();
    }

    // 「左回転」コマンドをキューに追加
    public void AddRotateLeft()
    {
        RecordRobotData(currentSpeed, rotationSpeed, "RotateLeft", currentDuration);
        commands.Enqueue(() => RotateObject(Vector3.down));
        SaveToJson();

    }





    // 実行ボタンが押されたときにキューのコマンドを順次実行
    public void StartExecution()
    {
        if (!isExecuting)
        {
            isExecuting = true;
            StartCoroutine(ExecuteCommands());
        }
    }





    // 前進処理（コルーチン）
    private IEnumerator MoveForward()
    {
        float distanceMoved = 0f;
        while (distanceMoved < movetime * moveSpeed)
        {
            float moveStep = moveSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * moveStep);
            distanceMoved += moveStep;
            yield return null;
        }
    }



    // 後進処理（コルーチン）
    private IEnumerator MoveBackward()
    {
        float distanceMoved = 0f;
        while (distanceMoved < movetime * moveSpeed)
        {
            float moveStep = moveSpeed * Time.deltaTime;
            transform.Translate(Vector3.back * moveStep);
            distanceMoved += moveStep;
            yield return null;
        }
    }

    // 回転処理（コルーチン）
    private IEnumerator RotateObject(Vector3 rotationDirection)
    {
        float angleRotated = 0f;
        while (angleRotated < rotateAngle)
        {
            float rotateStep = rotateSpeed * Time.deltaTime;
            transform.Rotate(rotationDirection, rotateStep);
            angleRotated += rotateStep;
            yield return null;
        }
    }

    // キューに追加されたコマンドを順次実行
    private IEnumerator ExecuteCommands()
    {
        while (commands.Count > 0)
        {
            var command = commands.Dequeue();
            yield return command(); // コマンドを実行し、完了まで待機
        }

        isExecuting = false; // 実行終了後にフラグをリセット
    }

    public void RecordRobotData(float speed, float rotationSpeed, string action, float duration)
    {

        RobotData data = new RobotData
        {
            speed = speed,
            rotationSpeed = rotationSpeed,
            action = action,
            duration = duration
        };
        robotDataList.Add(data);

    }


    public void SaveToJson()
    {
        string json = JsonConvert.SerializeObject(robotDataList, Formatting.Indented);
        File.WriteAllText("commands.json", json);
        Debug.Log($"生成されたJSON: {json}");

    }


}
