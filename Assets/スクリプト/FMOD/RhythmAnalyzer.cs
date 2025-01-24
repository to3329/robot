using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using FMODUnity;
using OfficeOpenXml; // EPPlusライブラリ
using UnityEngine;

public class RhythmAnalyzer : MonoBehaviour
{
    public EventReference musicEvent; // FMOD Studioのイベント参照
    private FMOD.Studio.EventInstance musicInstance;
    private FMOD.DSP fftDSP;
    private float[] spectrum = new float[512]; // スペクトラムデータを格納
    private float lastPeakTime = 0f;
    private float bpm = 0f;

    private float startTime = 0f;

    
    // データ収集用リスト
    private List<LogData> logDataList = new List<LogData>();

    public void OnButtonClick()
    {
        // EPPlus設定
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        // 音楽インスタンス作成と再生
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();

        // FFT設定
        SetupFFT();
    }

    void Update()
    {
        

        if (fftDSP.hasHandle())
        {
            // FFTデータ取得
            fftDSP.getParameterData((int)FMOD.DSP_FFT.SPECTRUMDATA, out var dataPtr, out _);
            var spectrumData = (FMOD.DSP_PARAMETER_FFT)Marshal.PtrToStructure(dataPtr, typeof(FMOD.DSP_PARAMETER_FFT));

            if (spectrumData.spectrum != null && spectrumData.spectrum.Length > 0)
            {
                for (int i = 0; i < spectrumData.spectrum[0].Length && i < spectrum.Length; i++)
                {
                    spectrum[i] = spectrumData.spectrum[0][i];
                }




                ////////////////////////////////////////////////////AnalyzeRhythm

                float maxAmplitude = 0f;
                int maxIndex = 0;

                // 最大振幅とそのインデックスを取得
                for (int i = 0; i < spectrum.Length; i++)
                {
                    //Debug.Log($"Detected BPM1");
                    if (spectrum[i] > maxAmplitude)
                    {
                        //Debug.Log($"Detected BPM2");
                        maxAmplitude = spectrum[i];
                        maxIndex = i;
                    }
                }

                // BPM計算
                float currentTime = Time.time;
                float interval = currentTime - lastPeakTime;
                if (maxAmplitude > 0.1f && interval > 0.2f) // 振幅が閾値を超えた場合
                {
                    lastPeakTime = currentTime;
                    bpm = 60f / interval;
                }
                float elapsedTime = Time.time - startTime;

                // データを記録
                logDataList.Add(new LogData
                {
                    Time = elapsedTime,
                    BPM = bpm,
                    MaxAmplitude = maxAmplitude,
                    MaxIndex = maxIndex
                });
            }
        }



    }

    private void SetupFFT()
    {
        var coreSystem = RuntimeManager.CoreSystem;
        coreSystem.createDSPByType(FMOD.DSP_TYPE.FFT, out fftDSP);
        fftDSP.setParameterInt((int)FMOD.DSP_FFT.WINDOWSIZE, 1024);

        if (coreSystem.getMasterChannelGroup(out FMOD.ChannelGroup masterChannelGroup) == FMOD.RESULT.OK)
        {
            masterChannelGroup.addDSP(FMOD.CHANNELCONTROL_DSP_INDEX.TAIL, fftDSP);
        }
    }

    private void OnDestroy()
    {
        // 音楽インスタンスとFFT DSPを解放
        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicInstance.release();
        }

        if (fftDSP.hasHandle())
        {
            fftDSP.release();
        }

        // データをExcelに保存
        SaveDataToExcel("RhythmAnalysis.xlsx");
    }

    private void SaveDataToExcel(string fileName)
    {
        string path = Path.Combine(Application.dataPath, fileName);

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Rhythm Data");

            // ヘッダー行
            worksheet.Cells[1, 1].Value = "Time (s)";
            worksheet.Cells[1, 2].Value = "BPM";
            worksheet.Cells[1, 3].Value = "Max Amplitude";
            worksheet.Cells[1, 4].Value = "Max Index";
            worksheet.Cells[1, 5].Value = "BPM Average";
            worksheet.Cells[1, 6].Value = "MaxAmplitude Average";
            worksheet.Cells[1, 7].Value = "aveBPM -BPM";
            worksheet.Cells[1, 8].Value = "ampBPM -BPM";
            worksheet.Cells[1, 9].Value = "BPM-StandardDeviation";


            float totalBPM = 0f;
            float totalMaxAmplitude = 0f;
            int validBPMCount = 0; // BPMが0でないデータのカウント
            float averageBPM = 0f;
            float averageMaxAmplitude = 0f;
            // データ行
            for (int i = 0; i < logDataList.Count; i++)
            {

                var log = logDataList[i];
                if (log.BPM == 0)
                {
                    log.MaxAmplitude = 0;

                    continue;


                }

                worksheet.Cells[i + 2, 1].Value = log.Time;
                worksheet.Cells[i + 2, 2].Value = log.BPM;
                worksheet.Cells[i + 2, 3].Value = log.MaxAmplitude;
                worksheet.Cells[i + 2, 4].Value = log.MaxIndex;

                if (log.BPM > 0) // 有効なBPMのみ計算
                {
                    totalBPM += log.BPM;
                    validBPMCount++;
                }
                totalMaxAmplitude += log.MaxAmplitude;


                // 平均値を計算
                averageBPM = validBPMCount > 0 ? totalBPM / validBPMCount : 0;
                averageMaxAmplitude = logDataList.Count > 0 ? totalMaxAmplitude / logDataList.Count : 0;

            }

            // 平均値をエクセルのE2, F2に保存
            worksheet.Cells[2, 5].Value = averageBPM;
            worksheet.Cells[2, 6].Value = averageMaxAmplitude;

            float baseValue = worksheet.Cells[2, 5].GetValue<float>(); // B2の値を取得
            float baseAmplitude = worksheet.Cells[2, 6].GetValue<float>(); // B2の値を取得

            List<float> bpmDifferences = new List<float>();

            for (int i = 0; i < logDataList.Count; i++)
            {
                var log = logDataList[i];

                float bpmDifference = log.BPM - baseValue; // B列の値からB2の値を引く
                float ampDifference = log.MaxAmplitude - baseAmplitude;
                if (log.BPM == 0)
                {
                    bpmDifference = 0;
                    ampDifference = 0;

                    continue;


                }
                bpmDifferences.Add(bpmDifference);

                worksheet.Cells[i + 2, 7].Value = bpmDifference; // G列に結果を挿入
                worksheet.Cells[i + 2, 8].Value = ampDifference;
            }

            // aveBPM - BPM の標準偏差を計算
            float standardDeviation = CalculateStandardDeviation(bpmDifferences);

            // 標準偏差をI2に保存
            worksheet.Cells[2, 9].Value = standardDeviation;

            /*
            float excellentThreshold = averageBPM + standardDeviation; // 最大評価
            float goodThreshold = averageBPM;                         // 良い評価
            float badThreshold = averageBPM - standardDeviation;      // 悪い評価

            
            for (int i = 0; i < logDataList.Count; i++)
            {
                var log = logDataList[i];
                if (log.BPM == 0)
                {
                    worksheet.Cells[i + 2, 10].Value = 1; // 最低評価
                    continue;
                }

                int rating;
                if (log.BPM >= excellentThreshold)
                    rating = 4; // 最大評価
                else if (log.BPM >= goodThreshold)
                    rating = 3; // 良い評価
                else if (log.BPM >= badThreshold)
                    rating = 2; // 悪い評価
                else
                    rating = 1; // 最低評価

                worksheet.Cells[i + 2, 10].Value = rating; // J列
              
            }
            */
            for (int i = 0; i < logDataList.Count; i++)
            {
                var log = logDataList[i];
                int rating;
                if (log.BPM == 0)
                {
                  
                    continue;


                }

                if (averageBPM + standardDeviation <=log.BPM )
                {
                rating = 4;
                }
                else if (averageBPM < log.BPM && log.BPM <= averageBPM + standardDeviation)
                {
                rating = 3;
                }
                else if (averageBPM - standardDeviation <= log.BPM && log.BPM < averageBPM)
                {
                    rating = 2;
                }
                else
                {
                rating = 1;
                }
                worksheet.Cells[i + 2, 10].Value = rating;
            }



            // 保存
            FileInfo fileInfo = new FileInfo(path);
            package.SaveAs(fileInfo);

            Debug.Log($"Data saved to {path}");
        }
    }

    private float CalculateStandardDeviation(List<float> values)
    {
        if (values == null || values.Count == 0) return 0;

        float mean = 0;
        foreach (var value in values)
        {
            mean += value;
        }
        mean /= values.Count;

        float sumSquaredDifferences = 0;
        foreach (var value in values)
        {
            float difference = value - mean;
            sumSquaredDifferences += difference * difference;
        }

        return Mathf.Sqrt(sumSquaredDifferences / values.Count);
    }


    private class LogData
    {
        public float Time { get; set; }
        public float BPM { get; set; }
        public float MaxAmplitude { get; set; }
        public int MaxIndex { get; set; }
    }
}
