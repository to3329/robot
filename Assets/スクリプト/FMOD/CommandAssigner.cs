using System.Collections.Generic;
using System.IO;
using OfficeOpenXml; // EPPlus���C�u����
using UnityEngine;

/*
public class CommandAssigner : MonoBehaviour
{
    public Button loadButton; // Excel�t�@�C����ǂݍ��ރ{�^��
    public string fileName = "RhythmAnalysis-10.xlsx"; // �G�N�Z���t�@�C����
    public MusicRobotController robotController; // ���{�b�g����X�N���v�g

    private List<(float time, int rating)> commands = new List<(float time, int rating)>(); // �R�}���h���X�g

    private void Start()
    {
        // �{�^���ɃN���b�N�C�x���g��ǉ�
        loadButton.onClick.AddListener(ReadExcelAndAssignCommands);
    }

    private void ReadExcelAndAssignCommands()
    {
        string filePath = Path.Combine(Application.dataPath, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            return;
        }

        // EPPlus���C�Z���X�ݒ�i�񏤗p���p�̏ꍇ�j
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[0]; // �ŏ��̃V�[�g���擾
            int rowCount = worksheet.Dimension.Rows; // ���s�����擾

            // �R�}���h���X�g���N���A
            commands.Clear();

            // A��iTime�j�AJ��i�i���o�����O�j�̃f�[�^���擾���Ė��߂�ۑ�
            for (int row = 2; row <= rowCount; row++) // 2�s�ڂ���n�߂�i1�s�ڂ̓w�b�_�[�j
            {
                float time = worksheet.Cells[row, 1].GetValue<float>(); // A��
                int rating = worksheet.Cells[row, 10].GetValue<int>();  // J��

                Debug.Log($"Time: {time}, Rating: {rating}");

                // �R�}���h���X�g�ɕۑ�
                commands.Add((time, rating));
            }

            Debug.Log("Excel�f�[�^���ǂݍ��܂�܂����B�{�^���������ă��{�b�g�𓮍삳���Ă��������B");
        }
    }

    public void ExecuteCommands()
    {
        if (commands.Count == 0)
        {
            Debug.LogWarning("�R�}���h������܂���B�܂�Excel��ǂݍ���ł��������B");
            return;
        }

        foreach (var command in commands)
        {
            AssignCommand(command.rating, command.time);
        }
    }

    private void AssignCommand(int rating, float time)
    {
        // �i���o�����O�l�ɉ���������
        switch (rating)
        {
            case 4: // �O�i
                Debug.Log($"Time {time}: Execute Command for Max Rating (4)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.Forward, time);
                break;

            case 3: // ��i
                Debug.Log($"Time {time}: Execute Command for Good Rating (3)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.Backward, time);
                break;

            case 2: // �E��]
                Debug.Log($"Time {time}: Execute Command for Bad Rating (2)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.RotateRight, time);
                break;

            case 1: // ����]
                Debug.Log($"Time {time}: Execute Command for Min Rating (1)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.RotateLeft, time);
                break;

            default:
                Debug.LogWarning($"Unknown Rating {rating} at Time {time}");
                break;
        }
    }
}

*/
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml; // EPPlus���C�u����
using UnityEngine;
using System.Runtime.InteropServices;
using FMODUnity;



public class CommandAssigner : MonoBehaviour
{
    public EventReference musicEvent; // FMOD Studio�̃C�x���g�Q��
    private FMOD.Studio.EventInstance musicInstance;
    private FMOD.DSP fftDSP;


    public string fileName = "RhythmAnalysis.xlsx"; // �G�N�Z���t�@�C����
    public MusicRobotController robotController; // ���{�b�g����X�N���v�g

    private List<(float time, int rating, float bpm)> commands = new List<(float time, int rating, float bpm)>(); // �R�}���h���X�g

    private float speed = 0.05f; // �����X�s�[�h
    private float angularSpeed = 5f; // �����p���x

    private void Start()
    {
        ReadExcelAndAssignCommands();
    }

    private void ReadExcelAndAssignCommands()
    {
        string filePath = Path.Combine(Application.dataPath, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            return;
        }

        // EPPlus���C�Z���X�ݒ�i�񏤗p���p�̏ꍇ�j
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[0]; // �ŏ��̃V�[�g���擾
            int rowCount = worksheet.Dimension.Rows; // ���s�����擾

            // �R�}���h���X�g���N���A
            commands.Clear();

            // A��iTime�j�AB��iBPM�j�AJ��i�i���o�����O�j�̃f�[�^���擾���Ė��߂�ۑ�
            for (int row = 2; row <= rowCount; row++) // 2�s�ڂ���n�߂�i1�s�ڂ̓w�b�_�[�j
            {
                float time = worksheet.Cells[row, 1].GetValue<float>(); // A��
                float bpm = worksheet.Cells[row, 2].GetValue<float>(); // B��
                int rating = worksheet.Cells[row, 10].GetValue<int>();  // J��

                Debug.Log($"Time: {time}, BPM: {bpm}, Rating: {rating}");

                // �R�}���h���X�g�ɕۑ�
                commands.Add((time, rating, bpm));
            }

            // BPM�Ɋ�Â��ăX�s�[�h����
            AdjustSpeedBasedOnBPM();

            Debug.Log("Excel�f�[�^���ǂݍ��܂�A�X�s�[�h����������܂����B");
        }
    }

    private void AdjustSpeedBasedOnBPM()
    {
        for (int i = 1; i < commands.Count; i++)
        {
            float currentBPM = commands[i].bpm;
            float previousBPM = commands[i - 1].bpm;

            if (currentBPM > previousBPM)
            {
                // BPM���㏸���Ă���ꍇ
                speed += 0.01f;
                angularSpeed += 3f;
            }
            else if (currentBPM < previousBPM)
            {
                // BPM�����~���Ă���ꍇ
                speed = Mathf.Max(0.05f, speed - 0.01f); // �X�s�[�h��0�����ɂȂ�Ȃ��悤�ɂ���
                angularSpeed = Mathf.Max(1f, angularSpeed - 10f); // �p���x��10�����ɂȂ�Ȃ��悤�ɂ���
            }


            Debug.Log($"Adjusted Speed: {speed}, Angular Speed: {angularSpeed}");
        }
        Debug.Log($"Adj" + commands.Count);
    }

    public void ExecuteCommands()
    {
        /////////////////////////////////////////////////////////////////////
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        // ���y�C���X�^���X�쐬�ƍĐ�
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();

        // FFT�ݒ�
        //SetupFFT();
        //////////////////////////////////////////////////////////////////


        if (commands.Count == 0)
        {
            Debug.LogWarning("�R�}���h������܂���B�܂�Excel��ǂݍ���ł��������B");
            return;
        }

        foreach (var command in commands)
        {
            AssignCommand(command.rating, command.time);
        }
    }

    private void AssignCommand(int rating, float time)
    {
        // �i���o�����O�l�ɉ���������
        switch (rating)
        {
            case 4: // �O�i
                Debug.Log($"Time {time}: Execute Command for Max Rating (4)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.Forward, time, speed);
                break;

            case 3: // ��i
                Debug.Log($"Time {time}: Execute Command for Good Rating (3)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.Backward, time, speed);
                break;

            case 2: // �E��]
                Debug.Log($"Time {time}: Execute Command for Bad Rating (2)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.RotateRight, time, angularSpeed);
                break;

            case 1: // ����]
                Debug.Log($"Time {time}: Execute Command for Min Rating (1)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.RotateLeft, time, angularSpeed);
                break;

            default:
                Debug.LogWarning($"Unknown Rating {rating} at Time {time}");
                break;
        }
    }
}



/*
public class CommandAssigner : MonoBehaviour
{
    public string fileName = "RhythmAnalysis-10.xlsx"; // �G�N�Z���t�@�C����
    public MusicRobotController robotController; // ���{�b�g����X�N���v�g

    private List<(float time, int rating, float bpm)> commands = new List<(float time, int rating, float bpm)>(); // �R�}���h���X�g
    private float speed = 0.1f; // �����X�s�[�h
    private float angularSpeed = 45f; // �����p���x


    private void Start()
    {
        // �{�^���Ȃ��Ȃ̂ŁA�����I�ɓǂݍ��ޏꍇ�͈ȉ��̃��\�b�h���Ăяo���܂�
        ReadExcelAndAssignCommands();
    }
    

    private void ReadExcelAndAssignCommands()
    {
        string filePath = Path.Combine(Application.dataPath, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            return;
        }

        // EPPlus���C�Z���X�ݒ�i�񏤗p���p�̏ꍇ�j
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[0]; // �ŏ��̃V�[�g���擾
            int rowCount = worksheet.Dimension.Rows; // ���s�����擾

            // A��iTime�j�AJ��i�i���o�����O�j�̃f�[�^���擾���Ė��߂����蓖��
            for (int row = 2; row <= rowCount; row++) // 2�s�ڂ���n�߂�i1�s�ڂ̓w�b�_�[�j
            {
                float time = worksheet.Cells[row, 1].GetValue<float>(); // A��
                int rating = worksheet.Cells[row, 10].GetValue<int>();  // J��
                float bpm = worksheet.Cells[row, 2].GetValue<float>(); // B��

                Debug.Log($"Time: {time}, Rating: {rating}");

                // �i���o�����O�l�ɉ��������߂����s
                AssignCommand(rating, time);

            }
        }
    }

    private void AssignCommand(int rating, float time)
    {
        // �i���o�����O�l�ɉ���������
        switch (rating)
        {
            case 4: // �O�i
                Debug.Log($"Time {time}: Execute Command for Max Rating (4)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.Forward, time);
                break;

            case 3: // ��i
                Debug.Log($"Time {time}: Execute Command for Good Rating (3)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.Backward, time);
                break;

            case 2: // �E��]
                Debug.Log($"Time {time}: Execute Command for Bad Rating (2)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.RotateRight, time);
                break;

            case 1: // ����]
                Debug.Log($"Time {time}: Execute Command for Min Rating (1)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.RotateLeft, time);
                break;

            default:
                Debug.LogWarning($"Unknown Rating {rating} at Time {time}");
                break;
        }
    }
}
*/
