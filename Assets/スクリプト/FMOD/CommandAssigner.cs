using System.Collections.Generic;
using System.IO;
using OfficeOpenXml; // EPPlusライブラリ
using UnityEngine;

/*
public class CommandAssigner : MonoBehaviour
{
    public Button loadButton; // Excelファイルを読み込むボタン
    public string fileName = "RhythmAnalysis-10.xlsx"; // エクセルファイル名
    public MusicRobotController robotController; // ロボット制御スクリプト

    private List<(float time, int rating)> commands = new List<(float time, int rating)>(); // コマンドリスト

    private void Start()
    {
        // ボタンにクリックイベントを追加
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

        // EPPlusライセンス設定（非商用利用の場合）
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 最初のシートを取得
            int rowCount = worksheet.Dimension.Rows; // 総行数を取得

            // コマンドリストをクリア
            commands.Clear();

            // A列（Time）、J列（ナンバリング）のデータを取得して命令を保存
            for (int row = 2; row <= rowCount; row++) // 2行目から始める（1行目はヘッダー）
            {
                float time = worksheet.Cells[row, 1].GetValue<float>(); // A列
                int rating = worksheet.Cells[row, 10].GetValue<int>();  // J列

                Debug.Log($"Time: {time}, Rating: {rating}");

                // コマンドリストに保存
                commands.Add((time, rating));
            }

            Debug.Log("Excelデータが読み込まれました。ボタンを押してロボットを動作させてください。");
        }
    }

    public void ExecuteCommands()
    {
        if (commands.Count == 0)
        {
            Debug.LogWarning("コマンドがありません。まずExcelを読み込んでください。");
            return;
        }

        foreach (var command in commands)
        {
            AssignCommand(command.rating, command.time);
        }
    }

    private void AssignCommand(int rating, float time)
    {
        // ナンバリング値に応じた処理
        switch (rating)
        {
            case 4: // 前進
                Debug.Log($"Time {time}: Execute Command for Max Rating (4)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.Forward, time);
                break;

            case 3: // 後進
                Debug.Log($"Time {time}: Execute Command for Good Rating (3)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.Backward, time);
                break;

            case 2: // 右回転
                Debug.Log($"Time {time}: Execute Command for Bad Rating (2)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.RotateRight, time);
                break;

            case 1: // 左回転
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
using OfficeOpenXml; // EPPlusライブラリ
using UnityEngine;
using System.Runtime.InteropServices;
using FMODUnity;



public class CommandAssigner : MonoBehaviour
{
    public EventReference musicEvent; // FMOD Studioのイベント参照
    private FMOD.Studio.EventInstance musicInstance;
    private FMOD.DSP fftDSP;


    public string fileName = "RhythmAnalysis.xlsx"; // エクセルファイル名
    public MusicRobotController robotController; // ロボット制御スクリプト

    private List<(float time, int rating, float bpm)> commands = new List<(float time, int rating, float bpm)>(); // コマンドリスト

    private float speed = 0.05f; // 初期スピード
    private float angularSpeed = 5f; // 初期角速度

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

        // EPPlusライセンス設定（非商用利用の場合）
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 最初のシートを取得
            int rowCount = worksheet.Dimension.Rows; // 総行数を取得

            // コマンドリストをクリア
            commands.Clear();

            // A列（Time）、B列（BPM）、J列（ナンバリング）のデータを取得して命令を保存
            for (int row = 2; row <= rowCount; row++) // 2行目から始める（1行目はヘッダー）
            {
                float time = worksheet.Cells[row, 1].GetValue<float>(); // A列
                float bpm = worksheet.Cells[row, 2].GetValue<float>(); // B列
                int rating = worksheet.Cells[row, 10].GetValue<int>();  // J列

                Debug.Log($"Time: {time}, BPM: {bpm}, Rating: {rating}");

                // コマンドリストに保存
                commands.Add((time, rating, bpm));
            }

            // BPMに基づいてスピード調整
            AdjustSpeedBasedOnBPM();

            Debug.Log("Excelデータが読み込まれ、スピードが調整されました。");
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
                // BPMが上昇している場合
                speed += 0.01f;
                angularSpeed += 3f;
            }
            else if (currentBPM < previousBPM)
            {
                // BPMが下降している場合
                speed = Mathf.Max(0.05f, speed - 0.01f); // スピードは0未満にならないようにする
                angularSpeed = Mathf.Max(1f, angularSpeed - 10f); // 角速度は10未満にならないようにする
            }


            Debug.Log($"Adjusted Speed: {speed}, Angular Speed: {angularSpeed}");
        }
        Debug.Log($"Adj" + commands.Count);
    }

    public void ExecuteCommands()
    {
        /////////////////////////////////////////////////////////////////////
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        // 音楽インスタンス作成と再生
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();

        // FFT設定
        //SetupFFT();
        //////////////////////////////////////////////////////////////////


        if (commands.Count == 0)
        {
            Debug.LogWarning("コマンドがありません。まずExcelを読み込んでください。");
            return;
        }

        foreach (var command in commands)
        {
            AssignCommand(command.rating, command.time);
        }
    }

    private void AssignCommand(int rating, float time)
    {
        // ナンバリング値に応じた処理
        switch (rating)
        {
            case 4: // 前進
                Debug.Log($"Time {time}: Execute Command for Max Rating (4)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.Forward, time, speed);
                break;

            case 3: // 後進
                Debug.Log($"Time {time}: Execute Command for Good Rating (3)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.Backward, time, speed);
                break;

            case 2: // 右回転
                Debug.Log($"Time {time}: Execute Command for Bad Rating (2)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.RotateRight, time, angularSpeed);
                break;

            case 1: // 左回転
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
    public string fileName = "RhythmAnalysis-10.xlsx"; // エクセルファイル名
    public MusicRobotController robotController; // ロボット制御スクリプト

    private List<(float time, int rating, float bpm)> commands = new List<(float time, int rating, float bpm)>(); // コマンドリスト
    private float speed = 0.1f; // 初期スピード
    private float angularSpeed = 45f; // 初期角速度


    private void Start()
    {
        // ボタンなしなので、自動的に読み込む場合は以下のメソッドを呼び出します
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

        // EPPlusライセンス設定（非商用利用の場合）
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 最初のシートを取得
            int rowCount = worksheet.Dimension.Rows; // 総行数を取得

            // A列（Time）、J列（ナンバリング）のデータを取得して命令を割り当て
            for (int row = 2; row <= rowCount; row++) // 2行目から始める（1行目はヘッダー）
            {
                float time = worksheet.Cells[row, 1].GetValue<float>(); // A列
                int rating = worksheet.Cells[row, 10].GetValue<int>();  // J列
                float bpm = worksheet.Cells[row, 2].GetValue<float>(); // B列

                Debug.Log($"Time: {time}, Rating: {rating}");

                // ナンバリング値に応じた命令を実行
                AssignCommand(rating, time);

            }
        }
    }

    private void AssignCommand(int rating, float time)
    {
        // ナンバリング値に応じた処理
        switch (rating)
        {
            case 4: // 前進
                Debug.Log($"Time {time}: Execute Command for Max Rating (4)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.Forward, time);
                break;

            case 3: // 後進
                Debug.Log($"Time {time}: Execute Command for Good Rating (3)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.Backward, time);
                break;

            case 2: // 右回転
                Debug.Log($"Time {time}: Execute Command for Bad Rating (2)");
                robotController?.ExecuteAction(MusicRobotController.ActionType.RotateRight, time);
                break;

            case 1: // 左回転
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
