using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SBFrameworkCore
{
    // -------------------------------------------------------------------------------------
    // Core Structure & Class
    // -------------------------------------------------------------------------------------
    public static class SBIO
    {
        private static string defaultIOPath = "SequenceData/";
        public static string FilePath => defaultIOPath;
    }
    
    public enum SBStepJobType
    {
        Init,
        End,
        UndoPoint,
        UndoPointAutoNext,
        Typing,
        WaitForClickObject,
        CameraTransform,
        HighlightOn,
        HighlightOff,
        CallMethod,
        AnimationTrigger,
        AnimationPlay,
        Delay,
        BlockingNext,
        BlockingRelease
    }

    public class SBStateMachine
    {
        //public EventArgs stateArgs;
        private event EventHandler _stateChange;

        public event EventHandler StateChange
        {
            add
            {
                _stateChange += value;
            }
            remove
            {
                _stateChange -= value;
            }
        }

        public void OnStateChange()
        {
            if (_stateChange != null)
                _stateChange(this, EventArgs.Empty);
        }
    }
    
    public class SBJob
    {
        public SBStepJobType Job = SBStepJobType.Init;

        public int ScriptIndex = 0;
        public bool IsAutoNext = false;

        public string ObjectName = null;
        public string MethodName = null;
        public string UndoMethodName = null;

        // 시작 위치
        public Vector3 CameraPosition = Vector3.zero;
        public Vector3 CameraRotation = Vector3.zero;

        // 카메라 이동 위치
        public Vector3 TargetPosition = Vector3.zero;
        public Vector3 TargetRotation = Vector3.zero;
        public float CameraMoveSpeed = 1.0f;
        public float CameraRotateSpeed = 1.0f;

        public float DelayTime = 0.0f;
        
        public SBJob()
        {
            Job = SBStepJobType.End;
        }
        public SBJob(float delaytime)
        {
            Job = SBStepJobType.Delay;
        }
        public SBJob(int index, bool isAutoNext = false)
        {
            Job = SBStepJobType.Typing;
            ScriptIndex = index;
            IsAutoNext = isAutoNext;
        }
        public SBJob(Vector3 position, Vector3 rotation)
        {
            Job = SBStepJobType.CameraTransform;
            TargetPosition = position;
            TargetRotation = rotation;
        }
        public SBJob(Vector3 position, Vector3 rotation, float moveSpeed, float rotateSpeed)
        {
            Job = SBStepJobType.CameraTransform;
            TargetPosition = position;
            TargetRotation = rotation;
            CameraMoveSpeed = moveSpeed;
            CameraRotateSpeed = rotateSpeed;
        }
        public SBJob(SBStepJobType job)
        {
            Job = job;
        }
        public SBJob(SBStepJobType job, string name, string method, string undo)
        {
            Job = job;
            ObjectName = name;
            MethodName = method;
            UndoMethodName = undo;
        }
        ~SBJob()
        {
            ObjectName = null;
            MethodName = null;
            UndoMethodName = null;
        }
    }

    // -------------------------------------------------------------------------------------
    // Logging System
    // 
    // Log : 메세지와 호출한 메소드 기록
    // ExportLogs : 기록된 값을 미리 지정된 타입으로 출력하고 값 초기화
    // [출력위치]
    //   - 에디터 : Debug 콘솔
    //   - 스탠드얼론 :
    //   - 웹빌드 :
    // -------------------------------------------------------------------------------------
    public static class SBLoggingSystem
    {
        public static StringBuilder sb;

        public static void Init()
        {
            if (sb == null)
                sb = new StringBuilder();

            sb.Clear();
        }

        public static void Log(string str, [CallerMemberName] string caller = "")
        {
            if (sb == null)
                sb = new StringBuilder();

            sb.Append('[');
            sb.Append(caller);
            sb.Append(']');
            sb.Append(str);
            sb.AppendLine();
        }

        public static void ExportLogs()
        {
#if UNITY_EDITOR
            Debug.Log(sb);
            Init();
#endif
        }
    }

}