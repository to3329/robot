using System.Collections.Generic;
using UnityEngine;
using OfficeOpenXml;
using System.IO;

namespace RobotActions
{
    public class ActionData
    {
        public float Time;
        public int ActionType;
    }
}

public class ExcelDataLoader : MonoBehaviour
{
    public string excelFilePath;
    public MusicRobotController robotController;

    public void LoadAndExecuteExcel()
    {
        if (!File.Exists(excelFilePath))
        {
            Debug.LogError("Excelファイルが見つかりません: " + excelFilePath);
            return;
        }

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
        {
            var worksheet = package.Workbook.Worksheets[0];
            if (worksheet == null)
            {
                Debug.LogError("シートが見つかりません");
                return;
            }

            List<RobotActions.ActionData> actions = new List<RobotActions.ActionData>();
            int row = 2;

            while (true)
            {
                var timeCell = worksheet.Cells[row, 1];
                var actionCell = worksheet.Cells[row, 10];

                if (timeCell.Value == null || actionCell.Value == null)
                {
                    break;
                }

                float time = float.Parse(timeCell.Text);
                int actionCode = int.Parse(actionCell.Text);

                actions.Add(new RobotActions.ActionData { Time = time, ActionType = actionCode });
                row++;
            }

            ExecuteRobotActions(actions);
        }
    }

    private void ExecuteRobotActions(List<RobotActions.ActionData> actions)
    {
        foreach (var action in actions)
        {
            Debug.Log($"動作: {action.ActionType}, 時間: {action.Time}");
            //robotController.ExecuteAction((MusicRobotController.ActionType)action.ActionType, action.Time);
        }
    }
}
