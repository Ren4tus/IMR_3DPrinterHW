using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Text;
using UnityEngine;
using SBFrameworkCore;
using System.IO;

public class SBFrameworkSequence
{
    public Dictionary<string, string> SequenceName;
    public Dictionary<string, List<string>> Scripts;
    public List<SBJob> Jobs;
    
    public string GetSequenceName()
    {
        return SequenceName[CL_CommonFunctionManager.Instance.GetProgramLanguage()];
    }

    public void GetJob(int index, out SBJob job)
    {
        if (index >= Jobs.Count || index < 0)
        {
            job = default;
            return;
        }

        job = Jobs[index];
    }
    
    public int TotalJobCount()
    {
        return Jobs.Count;
    }

    public void ResetData()
    {
        Jobs.Clear();
        Scripts.Clear();
    }

    // -------------------------------------------------------------------------------------
    // 스크립트 형식으로 저장
    // 생성되는 파일명  : SequenceX.csv
    // 저장되는 위치    : Assets/Resources/SequenceData/현재씬명/SequenceX.csv
    // -------------------------------------------------------------------------------------
    public void SaveSequenceToScript()
    {
        /*
        if (Jobs != null)
        {
            StringBuilder sbPath = new StringBuilder();
            
            sbPath.Append(SBIO.FilePath);
            sbPath.Append(SceneManager.GetActiveScene().name);
            sbPath.Append("/");
            sbPath.Append(gameObject.name);
            sbPath.Append(".csv");
            
            using (StreamWriter outputFile = new StreamWriter(sbPath.ToString()))
            {
                outputFile.WriteLine(Scripts.Count.ToString());

                int i = 0;

                StringBuilder sbLine = new StringBuilder();

                for (i=0; i< Scripts.Count; i++)
                {
                    sbLine.Clear();
                    sbLine.Append(i);
                    sbLine.Append("|");
                    sbLine.Append(Scripts["KR"][i]);
                    sbLine.Append("|");

                    if (Scripts["EN"].Count > i)
                    {
                        sbLine.Append(Scripts["EN"][i]);
                    }
                    else
                    {
                        sbLine.Append("No Translation");
                    }
                    outputFile.WriteLine(sbLine.ToString());
                }
                
                for (i=0;i<Jobs.Count;i++)
                {
                    sbLine.Clear();

                    switch (Jobs[i].Job)
                    {
                        case SBStepJobType.End:
                            sbLine.Append("End");
                            break;

                        case SBStepJobType.WaitForClickObject:
                            sbLine.Append("WaitForClickObject");
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].ObjectName);
                            break;

                        case SBStepJobType.AnimationTrigger:
                            sbLine.Append("AnimationTrigger");
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].ObjectName);
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].MethodName);
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].UndoMethodName);
                            break;

                        case SBStepJobType.CallMethod:
                            sbLine.Append("CallMethod");
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].ObjectName);
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].MethodName);
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].UndoMethodName);
                            break;

                        case SBStepJobType.Delay:
                            sbLine.Append("Delay");
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].DelayTime);
                            break;

                        case SBStepJobType.Typing:
                            sbLine.Append("Typing");
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].ScriptIndex);
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].isWaitForEnd);
                            break;

                        case SBStepJobType.HighlightOn:
                            sbLine.Append("HighlightOn");
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].ObjectName);
                            break;

                        case SBStepJobType.HighlightOff:
                            sbLine.Append("HighlightOff");
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].ObjectName);
                            break;

                        case SBStepJobType.CameraTransform:
                            sbLine.Append("CameraTransform");
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].TargetPosition.x);
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].TargetPosition.y);
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].TargetPosition.z);

                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].TargetRotation.x);
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].TargetRotation.y);
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].TargetRotation.z);

                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].CameraMoveSpeed);
                            sbLine.Append(" ");
                            sbLine.Append(Jobs[i].CameraRotateSpeed);
                            break;

                        case SBStepJobType.BlockingNext:
                            sbLine.Append("BlockingNext");
                            break;

                        case SBStepJobType.BlockingRelease:
                            sbLine.Append("BlockingRelease");
                            break;
                    }

                    outputFile.WriteLine(sbLine.ToString());
                }
            }
        }
        */
    }
    public void LoadSequenceByScript(int index)
    {
        StringBuilder sbPath = new StringBuilder();

        // EX) Path = SequenceData/TrainingCH3/Sequence1
        sbPath.Append("SequenceData/");
        sbPath.Append(SceneManager.GetActiveScene().name);
        sbPath.Append("/Sequence");
        sbPath.Append(index);

        int i;
        TextAsset textAsset = (TextAsset)Resources.Load(sbPath.ToString());
        string[] scripts = textAsset.text.Split('\n');

        for (i=0;i <scripts.Length; i++)
        {
            scripts[i] = scripts[i].Replace("<n>", "\n");
            scripts[i] = scripts[i].Replace("\r", "");
        }

        Scripts = new Dictionary<string, List<string>>();
        SequenceName = new Dictionary<string, string>();

        Jobs = new List<SBJob>();
        
        List<string> scriptListKR = new List<string>();
        List<string> scriptListEN = new List<string>();

        // 1 : 시퀀스 제목
        string[] names = scripts[0].Split('|');
        SequenceName["KR"] = names[0];
        SequenceName["EN"] = names[1];

        // 2 : 총 스크립트 수
        int scriptCount = int.Parse(scripts[1]);
        
        for (i = 2; i <= scriptCount + 1; i++)
        {
            // [0] = index
            string[] splitScript = scripts[i].Split('|');
            scriptListKR.Add(splitScript[1]);
            scriptListEN.Add(splitScript[2]);
        }

        Scripts.Add("KR", scriptListKR);
        Scripts.Add("EN", scriptListEN);

        for (i = scriptCount + 2; i < scripts.Length; i++)
        {
            string[] splitScript = scripts[i].Split(' ');
            SBJob job = new SBJob();
            
            switch (splitScript[0])
            {
                case "End":
                    job.Job = SBStepJobType.End;
                    break;

                case "UndoPoint":
                    job.Job = SBStepJobType.UndoPoint;
                    break;

                case "UndoPointAutoNext":
                    job.Job = SBStepJobType.UndoPointAutoNext;
                    break;

                case "WaitForClickObject":
                    job.Job = SBStepJobType.WaitForClickObject;

                    job.ObjectName = splitScript[1];
                    break;

                case "CallMethod":
                    job.Job = SBStepJobType.CallMethod;

                    job.ObjectName = splitScript[1];
                    job.MethodName = splitScript[2];
                    job.UndoMethodName = splitScript[3];
                    break;

                case "AnimationTrigger":
                    // MethodName Trigger를 실행
                    job.Job = SBStepJobType.AnimationTrigger;

                    job.ObjectName = splitScript[1];
                    job.MethodName = splitScript[2];
                    break;

                case "AnimationPlay":
                    job.Job = SBStepJobType.AnimationPlay;

                    job.ObjectName = splitScript[1];
                    job.MethodName = splitScript[2];
                    break;

                case "Delay":
                    job.Job = SBStepJobType.Delay;

                    job.DelayTime = float.Parse(splitScript[1]);
                    break;

                case "Typing":
                    job.Job = SBStepJobType.Typing;

                    job.ScriptIndex = int.Parse(splitScript[1]);

                    if (splitScript.Length >= 3)
                        job.IsAutoNext = bool.Parse(splitScript[2]);
                    break;

                case "HighlightOn":
                    job.Job = SBStepJobType.HighlightOn;

                    job.ObjectName = splitScript[1];
                    break;

                case "HighlightOff":
                    job.Job = SBStepJobType.HighlightOff;

                    job.ObjectName = splitScript[1];
                    break;

                case "CameraTransform":
                    job.Job = SBStepJobType.CameraTransform;

                    job.TargetPosition = new Vector3(float.Parse(splitScript[1]), float.Parse(splitScript[2]), float.Parse(splitScript[3]));
                    job.TargetRotation = new Vector3(float.Parse(splitScript[4]), float.Parse(splitScript[5]), float.Parse(splitScript[6]));

                    job.CameraMoveSpeed = float.Parse(splitScript[7]);
                    job.CameraRotateSpeed = float.Parse(splitScript[8]);

                    if (splitScript.Length >= 10)
                        job.IsAutoNext = bool.Parse(splitScript[9]);
                    break;

                case "BlockingNext":
                    job.Job = SBStepJobType.BlockingNext;
                    break;

                case "BlockingRelease":
                    job.Job = SBStepJobType.BlockingRelease;
                    break;
            }

            Jobs.Add(job);
        }
    }
}
