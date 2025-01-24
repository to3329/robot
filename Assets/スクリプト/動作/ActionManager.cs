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
    public string ActionType; // �O�i�A��i�A�E��]�A����]
    public float Speed;
    public float Time;
    public float Angle;
    public float AngularSpeed;
}


[System.Serializable]
public class ActionDataListWrapper
{
    public List<ActionData> actions; // JSON�ɕۑ����郊�X�g
}

public class ActionManager : MonoBehaviour
{
    private Queue<ActionData> actionQueue = new Queue<ActionData>();
    private Queue<ActionData> recordQueue = new Queue<ActionData>();

    public TMP_InputField inputField; // TMP_InputField ��ݒ�
    public GameObject inputFieldObject; // TMP_InputField ���܂� GameObject
    public ScrollViewButtonManager scrollViewA;
    public Transform contentParent;


    public ScrollViewButtonManager scrollViewManager; // �{�^���Ǘ��X�N���v�g

    private void Start()
    {
        // ������Ԃł� TMP_InputField ���\���ɂ���
        if (inputFieldObject != null)
        {
            inputFieldObject.SetActive(false);
        }
    }
    


    // ������L���[�ɓo�^
    public void RegisterAction(ActionData action)
    {
        actionQueue.Enqueue(action);
        recordQueue.Enqueue(action);
        Debug.Log($"������L���[�ɓo�^: {action.ActionType}");
    }

    // ��������s
    public void ExecuteActions()
    {
        if (actionQueue.Count > 0)
        {

            ActionData action = actionQueue.Dequeue();
            Debug.Log("����" + action.Speed);
            StartCoroutine(ExecuteActionCoroutine(action));
        }
        else
        {
            Debug.Log("�L���[�ɓ��삪����܂���");
        }
    }

    // �R���[�`���œ�������s
    private IEnumerator ExecuteActionCoroutine(ActionData action)
    {

        float distanceMoved = 0f;
        float totalDistance = action.Time * action.Speed;
        switch(action.ActionType)
            {
            case "�O�i":
                while (distanceMoved < totalDistance)
                {
                    float moveStep = action.Speed * Time.deltaTime;

                    // �I�u�W�F�N�g��O�i������
                    transform.Translate(Vector3.forward * moveStep);
                    distanceMoved += moveStep;

                    // ���̃t���[���܂őҋ@
                    yield return null;
                }
                break;
                
            case "��i":
                while (distanceMoved < totalDistance)
                {
                    float moveStep = action.Speed * Time.deltaTime;

                    // �I�u�W�F�N�g��O�i������
                    transform.Translate(Vector3.back * moveStep);
                    distanceMoved += moveStep;

                    // ���̃t���[���܂őҋ@
                    yield return null;
                }
                break;
                
            case "�E��]"://///Vector3.up
                float rightangleRotated = 0f;
                
                while (rightangleRotated < action.Angle)
                {
                    float rotateStep = action.AngularSpeed * Time.deltaTime;
                    transform.Rotate(Vector3.up, rotateStep);
                    rightangleRotated += rotateStep;
                    yield return null;
                }
                break;
            case "����]"://///Vector3.up
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

        

        Debug.Log($"���슮��: {action.ActionType}");

        // ���̓�������s
        if (actionQueue.Count > 0)
        {
            ExecuteActions();
        }
        else
        {
            Debug.Log("���ׂĂ̓��삪�������܂���");
        }
    }


    

   
    public void SaveActions()
    {
        // TMP_InputField ��\�����ăt�@�C��������͂ł���悤�ɂ���
        if (inputFieldObject != null)
        {
            inputFieldObject.SetActive(true);
        }
    }

    /*
    // ���͊�����ɕۑ������s
    public void OnSaveFileNameEntered()
    {
        if (inputField == null)
        {
            Debug.LogError("TMP_InputField ���ݒ肳��Ă��܂���I");
            return;
        }

        // ���͂��ꂽ�t�@�C�������擾
        string fileName = inputField.text;

        if (string.IsNullOrWhiteSpace(fileName))
        {
            Debug.LogError("�t�@�C���������͂���Ă��܂���I");
            return;
        }

        // �t�@�C���p�X�𐶐�
        string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");

        // �L���[���烊�X�g�ɕϊ����ĕۑ�
        List<ActionData> actions = new List<ActionData>(recordQueue);

        foreach (var action in actions)
        {
            Debug.Log($"ActionType: {action.ActionType}, Speed: {action.Speed}, Time: {action.Time}, Angle: {action.Angle}, AngularSpeed: {action.AngularSpeed}");
        }

        // JSON �ɃV���A���C�Y
        string json = JsonConvert.SerializeObject(actions, Formatting.Indented);
        File.WriteAllText(filePath, json);

        Debug.Log($"�����ۑ����܂���: {filePath}");



        scrollViewA.AddButtonToScrollView(fileName);

        // TMP_InputField ���\���ɂ��ă��Z�b�g
        inputField.text = "";
        inputFieldObject.SetActive(false);
        
    }
    */


    public void OnSaveFileNameEntered()
    {
        if (inputField == null)
        {
            Debug.LogError("TMP_InputField ���ݒ肳��Ă��܂���I");
            return;
        }

        string fileName = inputField.text;

        if (string.IsNullOrWhiteSpace(fileName))
        {
            Debug.LogError("�t�@�C���������͂���Ă��܂���I");
            return;
        }

        string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");

        // �L���[���烊�X�g�ɕϊ����ĕۑ�
        List<ActionData> actions = new List<ActionData>(recordQueue);

        string json = JsonConvert.SerializeObject(actions, Formatting.Indented);
        File.WriteAllText(filePath, json);

        Debug.Log($"�����ۑ����܂���: {filePath}");

        // �X�N���[���r���[A�Ƀ{�^����ǉ�
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
            Destroy(child.gameObject); // �q�I�u�W�F�N�g���폜
        }
        Debug.Log($"�S�폜����܂���");
    }



    // JSON�t�@�C�����瓮���ǂݍ���
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
            Debug.Log($"�����ǂݍ���: {filePath}");
        }
        else
        {
            Debug.LogError($"�t�@�C����������܂���: {filePath}");
        }
    }
}
