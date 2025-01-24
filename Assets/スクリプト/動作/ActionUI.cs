using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ActionUI : MonoBehaviour
{
    public Button registerButton; // 登録ボタン
    public Button deleteButton;   // 削除ボタン
    public TMP_InputField speedInput, timeInput, angleInput, angularSpeedInput; // 入力フィールド

    private string actionType;
    private ActionManager actionManager;

    // 初期設定
    public void Setup(string actionType, ActionManager manager)
    {
        this.actionType = actionType;
        this.actionManager = manager;

        // 登録ボタンにクリックイベントを設定
        registerButton.onClick.AddListener(RegisterAction);

        // 削除ボタンにクリックイベントを設定
        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(DeleteAction);
        }
    }

    // 入力値を取得してActionManagerに登録
    private void RegisterAction()
    {
        float.TryParse(speedInput.text, out float speed);
        float.TryParse(timeInput.text, out float time);
        float.TryParse(angleInput.text, out float angle);
        float.TryParse(angularSpeedInput.text, out float angularSpeed);

        actionManager.RegisterAction(new ActionData
        {
            ActionType = actionType,
            Speed = speed,
            Time = time,
            Angle = angle,
            AngularSpeed = angularSpeed
        });
    }

    // 削除ボタンのクリック処理
    private void DeleteAction()
    {
        Debug.Log($"削除: {gameObject.name}");
        Destroy(gameObject); // 親のプレハブを削除
    }
}


/*
public class ActionUI : MonoBehaviour
{
    public Button registerButton; // 親ボタン
    public TMP_InputField speedInput, timeInput, angleInput, angularSpeedInput; // 入力フィールド

    private string actionType;
    private ActionManager actionManager;

    // 初期設定
    public void Setup(string actionType, ActionManager manager)
    {
        this.actionType = actionType;
        this.actionManager = manager;

        // 登録ボタンにクリックイベントを設定
        registerButton.onClick.AddListener(RegisterAction);
    }

    // 入力値を取得してActionManagerに登録
    private void RegisterAction()
    {
        float.TryParse(speedInput.text, out float speed);
        float.TryParse(timeInput.text, out float time);
        float.TryParse(angleInput.text, out float angle);
        float.TryParse(angularSpeedInput.text, out float angularSpeed);
        Debug.Log(speed + time + angle);

        actionManager.RegisterAction(new ActionData
        {
            ActionType = actionType,
            Speed = speed,
            Time = time,
            Angle = angle,
            AngularSpeed = angularSpeed
        });
        Debug.Log($"動作を登録: {speed}");
        Debug.Log($"動作を登録: {actionType}");
    }
}


*/