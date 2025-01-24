using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using FMODUnity;
using OfficeOpenXml; // EPPlus���C�u����
using UnityEngine;

public class RhythmAnalyzer : MonoBehaviour
{
    public EventReference musicEvent; // FMOD Studio�̃C�x���g�Q��
    private FMOD.Studio.EventInstance musicInstance;
    private FMOD.DSP fftDSP;
    private float[] spectrum = new float[512]; // �X�y�N�g�����f�[�^���i�[
    private float lastPeakTime = 0f;
    private float bpm = 0f;

    private float startTime = 0f;

    
    // �f�[�^���W�p���X�g
    private List<LogData> logDataList = new List<LogData>();

    public void OnButtonClick()
    {
        // EPPlus�ݒ�
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        // ���y�C���X�^���X�쐬�ƍĐ�
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();

        // FFT�ݒ�
        SetupFFT();
    }

    void Update()
    {
        

        if (fftDSP.hasHandle())
        {
            // FFT�f�[�^�擾
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

                // �ő�U���Ƃ��̃C���f�b�N�X���擾
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

                // BPM�v�Z
                float currentTime = Time.time;
                float interval = currentTime - lastPeakTime;
                if (maxAmplitude > 0.1f && interval > 0.2f) // �U����臒l�𒴂����ꍇ
                {
                    lastPeakTime = currentTime;
                    bpm = 60f / interval;
                }
                float elapsedTime = Time.time - startTime;

                // �f�[�^���L�^
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
        // ���y�C���X�^���X��FFT DSP�����
        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicInstance.release();
        }

        if (fftDSP.hasHandle())
        {
            fftDSP.release();
        }

        // �f�[�^��Excel�ɕۑ�
        SaveDataToExcel("RhythmAnalysis.xlsx");
    }

    private void SaveDataToExcel(string fileName)
    {
        string path = Path.Combine(Application.dataPath, fileName);

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Rhythm Data");

            // �w�b�_�[�s
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
            int validBPMCount = 0; // BPM��0�łȂ��f�[�^�̃J�E���g
            float averageBPM = 0f;
            float averageMaxAmplitude = 0f;
            // �f�[�^�s
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

                if (log.BPM > 0) // �L����BPM�̂݌v�Z
                {
                    totalBPM += log.BPM;
                    validBPMCount++;
                }
                totalMaxAmplitude += log.MaxAmplitude;


                // ���ϒl���v�Z
                averageBPM = validBPMCount > 0 ? totalBPM / validBPMCount : 0;
                averageMaxAmplitude = logDataList.Count > 0 ? totalMaxAmplitude / logDataList.Count : 0;

            }

            // ���ϒl���G�N�Z����E2, F2�ɕۑ�
            worksheet.Cells[2, 5].Value = averageBPM;
            worksheet.Cells[2, 6].Value = averageMaxAmplitude;

            float baseValue = worksheet.Cells[2, 5].GetValue<float>(); // B2�̒l���擾
            float baseAmplitude = worksheet.Cells[2, 6].GetValue<float>(); // B2�̒l���擾

            List<float> bpmDifferences = new List<float>();

            for (int i = 0; i < logDataList.Count; i++)
            {
                var log = logDataList[i];

                float bpmDifference = log.BPM - baseValue; // B��̒l����B2�̒l������
                float ampDifference = log.MaxAmplitude - baseAmplitude;
                if (log.BPM == 0)
                {
                    bpmDifference = 0;
                    ampDifference = 0;

                    continue;


                }
                bpmDifferences.Add(bpmDifference);

                worksheet.Cells[i + 2, 7].Value = bpmDifference; // G��Ɍ��ʂ�}��
                worksheet.Cells[i + 2, 8].Value = ampDifference;
            }

            // aveBPM - BPM �̕W���΍����v�Z
            float standardDeviation = CalculateStandardDeviation(bpmDifferences);

            // �W���΍���I2�ɕۑ�
            worksheet.Cells[2, 9].Value = standardDeviation;

            /*
            float excellentThreshold = averageBPM + standardDeviation; // �ő�]��
            float goodThreshold = averageBPM;                         // �ǂ��]��
            float badThreshold = averageBPM - standardDeviation;      // �����]��

            
            for (int i = 0; i < logDataList.Count; i++)
            {
                var log = logDataList[i];
                if (log.BPM == 0)
                {
                    worksheet.Cells[i + 2, 10].Value = 1; // �Œ�]��
                    continue;
                }

                int rating;
                if (log.BPM >= excellentThreshold)
                    rating = 4; // �ő�]��
                else if (log.BPM >= goodThreshold)
                    rating = 3; // �ǂ��]��
                else if (log.BPM >= badThreshold)
                    rating = 2; // �����]��
                else
                    rating = 1; // �Œ�]��

                worksheet.Cells[i + 2, 10].Value = rating; // J��
              
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



            // �ۑ�
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
