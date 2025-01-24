using System.Collections.Generic;
using System.IO;
using OfficeOpenXml; // EPPlus���C�u����
using UnityEngine;
using UnityEngine.UI;

public class ExcelDataReader : MonoBehaviour
{
    public Button loadButton; // �{�^��
    public string fileName = "RhythmAnalysis.xlsx"; // �ǂݎ��Excel�t�@�C����

    private void Start()
    {
        // �{�^���ɃN���b�N�C�x���g��ǉ�
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
            var worksheet = package.Workbook.Worksheets[0]; // �ŏ��̃V�[�g���擾

            // **A��̃f�[�^���擾**
            List<float> columnA = new List<float>();
            int rowCount = worksheet.Dimension.Rows; // �V�[�g�̑��s��
            for (int row = 2; row <= rowCount; row++) // A���2�s�ڈȍ~��ǂݎ��
            {
                float value = worksheet.Cells[row, 1].GetValue<float>();
                columnA.Add(value);
            }
            Debug.Log($"A��̃f�[�^: {string.Join(", ", columnA)}");

            // **E2�Z���̃f�[�^���擾**
            float e2Value = worksheet.Cells[2, 5].GetValue<float>();
            Debug.Log($"E2�Z���̃f�[�^: {e2Value}");

            // **G��̃f�[�^���擾**
            List<float> columnG = new List<float>();
            for (int row = 2; row <= rowCount; row++) // G���2�s�ڈȍ~��ǂݎ��
            {
                float value = worksheet.Cells[row, 7].GetValue<float>();
                columnG.Add(value);
            }
            Debug.Log($"G��̃f�[�^: {string.Join(", ", columnG)}");

            // **I2�Z���̃f�[�^���擾**
            float i2Value = worksheet.Cells[2, 9].GetValue<float>();
            Debug.Log($"I2�Z���̃f�[�^: {i2Value}");
        }
    }
}
