using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvaluationCore
{
    [Serializable]
    public class EvaluationSequenceItem
    {
        public int Index;
        public int ParentIndex;

        public string Name; // 세부 항목명
        public string Tip; // 팁
        public int Score; // 배점
        public bool IsFinished; // 완료 여부
        public bool IsNecessary; // 핵심 작업?
        public Transform MovePosition;

        public EvaluationSequenceItem()
        {
            Index = -1;
            ParentIndex = -1;

            Name = ""; // 세부 항목명
            Tip = "";
            Score = 0; // 배점
            IsFinished = false; // 완료 여부
            IsNecessary = false;
        }
    }

    [Serializable]
    public class EvaluationContainer
    {
        public Dictionary<int, EvaluationSequence> _sequenceList = null;

        public void MakeIndex(List<EvaluationSequence> list)
        {
            _sequenceList = new Dictionary<int, EvaluationSequence>();

            for (int i = 0; i < list.Count; i++)
                _sequenceList.Add(list[i].Index, list[i]);
        }

        public EvaluationSequence GetSequenceItem(int index)
        {
            if (_sequenceList == null)
                return null;

            if (_sequenceList.ContainsKey(index))
                return _sequenceList[index];

            return null;
        }

        public int TotalSequenceCount()
        {
            if (_sequenceList == null)
                return 0;

            return _sequenceList.Count;
        }

        public bool IsAllComplete()
        {
            foreach (KeyValuePair<int, EvaluationSequence> item in _sequenceList)
            {
                if (!item.Value.IsComplete)
                    return false;
            }

            return true;
        }
    }

    [Serializable]
    public class EvaluationSequence
    {
        // member
        private int _currentStep = 0; // 진행 단계

        public int Index;
        public string Name; // 시퀀스명

        public int[] postProcessSequences; // 선행작업(들)
        public List<EvaluationSequenceItem> scoreItems = null;
        public bool IsComplete = false;
        public bool IsSkip = false;

        public EvaluationSequence(int index, string name, int[] postProcesses = null)
        {
            Index = index;
            Name = name;
            postProcessSequences = postProcesses;

            scoreItems = new List<EvaluationSequenceItem>();
        }

        ~EvaluationSequence()
        {
            postProcessSequences = null;

            scoreItems.Clear();
            scoreItems = null;
        }

        public void AddStep(EvaluationSequenceItem step)
        {
            scoreItems.Add(step);
        }
        public int TotalSteps()
        {
            return (scoreItems == null) ? 0 : scoreItems.Count;
        }
        public int CutrrentStep()
        {
            return _currentStep;
        }
        public bool StepComplete(int step)
        {
            if (step >= scoreItems.Count)
                return false;

            if (!scoreItems[step].IsFinished)
            {
                scoreItems[step].IsFinished = true;

                if (scoreItems[step].IsNecessary) // 현재 시퀀스 클리어 필수 조건이면 클리어
                    IsComplete = true;

                return true;
            }

            return false;
        }
        public bool IsAllComplete()
        {
            if (scoreItems == null)
                return true;

            foreach (EvaluationSequenceItem item in scoreItems)
            {
                if (!item.IsFinished)
                    return false;
            }

            return true;
        }

        public int TotalScores()
        {
            if (scoreItems == null)
                return 0;

            int sum = 0;

            foreach (EvaluationSequenceItem item in scoreItems)
            {
                sum += item.Score;
            }

            return sum;
        }
        public int TotalGainScores()
        {
            if (scoreItems == null)
                return 0;

            int sum = 0;

            foreach (EvaluationSequenceItem item in scoreItems)
            {
                if (item.IsFinished)
                    sum += item.Score;
            }

            return sum;
        }

        public void Initialize()
        {
            foreach (EvaluationSequenceItem item in scoreItems)
            {
                item.IsFinished = false;
            }
        }
    }
}