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

    ///////���{�b�g����͂ł���


    private Queue<System.Func<IEnumerator>> commands = new Queue<System.Func<IEnumerator>>(); // �R�}���h�L���[
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
        public List<RobotData> robots; // JSON�ɕۑ����郊�X�g
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
    public float movetime = 0.8f;  // �O�i�E��i�̈ړ�����
    public float rotateAngle = 90f;  // ��]�p�x
    public float moveSpeed = 1f;     // �ړ����x
    public float rotateSpeed = 45f;  // ��]���x
    */
    //[Header("UI Elements")]
    public float movetime = 0.8f;  // �O�i�E��i�̈ړ�����
    public float rotateAngle = 90f;  // ��]�p�x
    float moveSpeed = 1.0f;     // �ړ����x
    public float rotateSpeed = 45f;  // ��]���x

    public Transform inputParent; // �e�I�u�W�F�N�g�iInspector�Ŋ��蓖�āj

    //public TMP_InputField speedInput;            // ���x���̓t�B�[���h
    public TMP_InputField timeInput;             // ���ԓ��̓t�B�[���h
    public TMP_InputField rotationSpeedInput;    // ��]���x���̓t�B�[���h

    //public GameObject parentGameObject.transform;
    //Transform parentTransform = parentGameObject.transform; // �e�L�X�g�̐e

    [Header("Save/Load Settings")]
    public string saveFileName = "commands.json"; // �ۑ��t�@�C����


    // �X�N���v�g�J�n���ɌĂ΂��
    private void Start()
    {
        //float moveSpeed = 1f;     // �ړ����x
        

        if (inputParent != null)
        {
            
            TMP_InputField speedInputField = FindInputFieldByName(inputParent, "SpeedInput");
            
            if (speedInputField != null)
            {
                
                //speedInputField.onEndEdit.AddListener(delegate { UpdateSpeed(speedInputField); });
                //speedInputField.onEndEdit.AddListener(delegate { AddMoveForward(speedInputField); });
                //myButton.onclick.AddListener
                Debug.Log("SpeedInput ��������܂����B���X�i�[��o�^���܂����B");
            }
            else
            {
                Debug.LogError("SpeedInput ��������܂���ł����I");
            }
        }
        else
        {
            Debug.LogError("InputParent���ݒ肳��Ă��܂���I");
        }


        /*
        if (timeInput != null)
        {
            timeInput.onEndEdit.AddListener(delegate { UpdateTime(); });
            Debug.Log("����");
        }

        if (speedInput != null)
            speedInput.onEndEdit.AddListener(delegate { UpdateSpeed(); });

        if (rotationSpeedInput != null)
            rotationSpeedInput.onEndEdit.AddListener(delegate { UpdaterotationSpeed(); });
        */



    }


    private TMP_InputField FindInputFieldByName(Transform parent, string fieldName)
    {
        Debug.Log("�C���v�b�g�t�B�[���h");
        foreach (TMP_InputField inputField in parent.GetComponentsInChildren<TMP_InputField>())
        {
            if (inputField.name == fieldName)
            {
                return inputField;
            }
        }
        return null;
    }



    // ���͒l�̔��f

    public void UpdateTime()
    {


        Debug.Log("�ł���");

        timeInput = timeInput.GetComponent<TMP_InputField>();


        Debug.Log("�ł���");
        //////
        if (float.TryParse(timeInput.text, out currentDuration))
            Debug.Log("Time: " + currentDuration);
        else
            Debug.LogWarning("Invalid input for time");
        /////
    }


    private bool isExecuting = false; // ���s�����ǂ����̃t���O
    
    public void UpdateSpeed(TMP_InputField inputField)
    {
        Debug.Log("UpdateSpeed ���Ă΂�܂����B");
        //moveSpeed = 1f;     // �ړ����x
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

        Debug.Log($"UpdateSpeed: isSpeedUpdated = {isSpeedUpdated}"); // ��Ԃ����O�Ŋm�F
        
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


    // �u�O�i�v�R�}���h���L���[�ɒǉ�
    //////////////////////////////////////////////////////////////////////////////////////////
    //public void AddMoveForward(string inputSpeed) TMP_InputField inputFieldStartCoroutine

    public void AddMoveForward()
    {
        float a = value.GetInputValue();
        Debug.Log("�厖" + a);
        //RecordRobotData(moveSpeed, rotationSpeed, "MoveForward", currentDuration);
        Debug.Log(robotDataList.Count);
        RobotData lastData = robotDataList[robotDataList.Count - 1];

        // �Ō�� speed �� moveSpeed �ɑ��
        moveSpeed = lastData.speed;
        // ���͂��ꂽ�l�� float �ɕϊ�


        // moveSpeed ���X�V

        Debug.Log($"Move Speed ���X�V����܂���: {moveSpeed}");

            // �L���[�� MoveForward �R�}���h��ǉ�
        commands.Enqueue(MoveForward);

        Debug.Log("MoveForward �R�}���h���L���[�ɒǉ�����܂����B");
        
       
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////
    
    /*
    public void AddMoveForward()
    {

        

        foreach (var data in robotDataList)
        {
            Debug.Log($"���X�g�̃f�[�^: Action: {data.action}, Speed: {data.speed}, Rotation Speed: {data.rotationSpeed}, Duration: {data.duration}");
        }
        RecordRobotData(currentSpeed, rotationSpeed, "MoveForward", currentDuration);

        SaveToJson();

        //���̕�����UpdateSpeed�̏������I���܂őҋ@����v���O������������
        StartCoroutine(WaitForUpdateSpeedAndEnqueue());
        
        //commands.Enqueue(MoveForward);
        //string queueContents = string.Join(", ", commands);

    }

    /// <summary>
    private IEnumerator WaitForUpdateSpeedAndEnqueue()
    {
        Debug.Log("UpdateSpeed������ҋ@��...");



        // �t���O��true�ɂȂ�܂őҋ@
        
        //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        
        yield return new WaitUntil(() =>
        {
            Debug.Log($"Checking isSpeedUpdated: {isSpeedUpdated}"); // �����𖈃t���[�����O�o��
            return isSpeedUpdated;
        });
        

    

    Debug.Log("UpdateSpeed���������܂����BMoveForward�R�}���h���L���[�ɒǉ����܂��B");
        Debug.Log("Upd" + moveSpeed);


        // MoveForward�R�}���h���L���[�ɒǉ�
        commands.Enqueue(MoveForward);

        string queueContents = string.Join(", ", commands);
        Debug.Log($"�R�}���h�L���[�̓��e: {queueContents}");
    }Add
    */
    /// </summary>

    // �u��i�v�R�}���h���L���[�ɒǉ�
    public void AddMoveBackward()
    {
        Debug.Log("Move Speed: ���" + moveSpeed);
        RecordRobotData(currentSpeed, rotationSpeed, "MoveBackward", currentDuration);
        commands.Enqueue(MoveBackward);
        SaveToJson();
    }

    // �u�E��]�v�R�}���h���L���[�ɒǉ�
    public void AddRotateRight()
    {
        RecordRobotData(currentSpeed, rotationSpeed, "RotateRight", currentDuration);
        commands.Enqueue(() => RotateObject(Vector3.up));
        SaveToJson();
    }

    // �u����]�v�R�}���h���L���[�ɒǉ�
    public void AddRotateLeft()
    {
        RecordRobotData(currentSpeed, rotationSpeed, "RotateLeft", currentDuration);
        commands.Enqueue(() => RotateObject(Vector3.down));
        SaveToJson();

    }





    // ���s�{�^���������ꂽ�Ƃ��ɃL���[�̃R�}���h���������s
    public void StartExecution()
    {
        if (!isExecuting)
        {
            isExecuting = true;
            StartCoroutine(ExecuteCommands());
        }
    }





    // �O�i�����i�R���[�`���j
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



    // ��i�����i�R���[�`���j
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

    // ��]�����i�R���[�`���j
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

    // �L���[�ɒǉ����ꂽ�R�}���h���������s
    private IEnumerator ExecuteCommands()
    {
        while (commands.Count > 0)
        {
            var command = commands.Dequeue();
            yield return command(); // �R�}���h�����s���A�����܂őҋ@
        }

        isExecuting = false; // ���s�I����Ƀt���O�����Z�b�g
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
        Debug.Log($"�������ꂽJSON: {json}");

    }


}
