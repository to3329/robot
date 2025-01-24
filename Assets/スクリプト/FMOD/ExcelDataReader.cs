using System.Collections.Generic;
using System.IO;
using OfficeOpenXml; // EPPlusライブラリ
using UnityEngine;
using UnityEngine.UI;

public class ExcelDataReader : MonoBehaviour
{
    public Button loadButton; // ボタン
    public string fileName = "RhythmAnalysis.xlsx"; // 読み取るExcelファイル名

    private void Start()
    {
        // ボタンにクリックイベントを追加
        loadButton.onClick.AddListener(ReadExcelData);
    }

    private void ReadExcelData()
    {
        string filePath = Path.Combine(Application.dataPath, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            return;
        }

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 最初のシートを取得

            // **A列のデータを取得**
            List<float> columnA = new List<float>();
            int rowCount = worksheet.Dimension.Rows; // シートの総行数
            for (int row = 2; row <= rowCount; row++) // A列の2行目以降を読み取る
            {
                float value = worksheet.Cells[row, 1].GetValue<float>();
                columnA.Add(value);
            }
            Debug.Log($"A列のデータ: {string.Join(", ", columnA)}");

            // **E2セルのデータを取得**
            float e2Value = worksheet.Cells[2, 5].GetValue<float>();
            Debug.Log($"E2セルのデータ: {e2Value}");

            // **G列のデータを取得**
            List<float> columnG = new List<float>();
            for (int row = 2; row <= rowCount; row++) // G列の2行目以降を読み取る
            {
                float value = worksheet.Cells[row, 7].GetValue<float>();
                columnG.Add(value);
            }
            Debug.Log($"G列のデータ: {string.Join(", ", columnG)}");

            // **I2セルのデータを取得**
            float i2Value = worksheet.Cells[2, 9].GetValue<float>();
            Debug.Log($"I2セルのデータ: {i2Value}");
        }
    }
}
