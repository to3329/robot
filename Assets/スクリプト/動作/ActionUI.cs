using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ActionUI : MonoBehaviour
{
    public Button registerButton; // �o�^�{�^��
    public Button deleteButton;   // �폜�{�^��
    public TMP_InputField speedInput, timeInput, angleInput, angularSpeedInput; // ���̓t�B�[���h

    private string actionType;
    private ActionManager actionManager;

    // �����ݒ�
    public void Setup(string actionType, ActionManager manager)
    {
        this.actionType = actionType;
        this.actionManager = manager;

        // �o�^�{�^���ɃN���b�N�C�x���g��ݒ�
        registerButton.onClick.AddListener(RegisterAction);

        // �폜�{�^���ɃN���b�N�C�x���g��ݒ�
        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(DeleteAction);
        }
    }

    // ���͒l���擾����ActionManager�ɓo�^
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

    // �폜�{�^���̃N���b�N����
    private void DeleteAction()
    {
        Debug.Log($"�폜: {gameObject.name}");
        Destroy(gameObject); // �e�̃v���n�u���폜
    }
}


/*
public class ActionUI : MonoBehaviour
{
    public Button registerButton; // �e�{�^��
    public TMP_InputField speedInput, timeInput, angleInput, angularSpeedInput; // ���̓t�B�[���h

    private string actionType;
    private ActionManager actionManager;

    // �����ݒ�
    public void Setup(string actionType, ActionManager manager)
    {
        this.actionType = actionType;
        this.actionManager = manager;

        // �o�^�{�^���ɃN���b�N�C�x���g��ݒ�
        registerButton.onClick.AddListener(RegisterAction);
    }

    // ���͒l���擾����ActionManager�ɓo�^
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
        Debug.Log($"�����o�^: {speed}");
        Debug.Log($"�����o�^: {actionType}");
    }
}


*/