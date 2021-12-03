using System.Collections;
using System.Collections.Generic;
using IMR;


public abstract class Lesson
{
    public Dictionary<string, IMRStatement> statement_dictionary;
    protected int _hintCount = 1;
    protected string lessonName;
    protected double _score;
    protected bool _completion;
    public abstract void Init();
    public abstract void ChangeNewStatement(string name);
    public virtual void CloneResultCanvas(EvaluationCore.EvaluationContainer SequenceConatiner) { }
    public virtual void SetLimitStatementResult(bool b) { }
    public virtual void SetEvaluationItemElement(string _item, string _step, bool sucess) { }
    public virtual void SetHintStatementResultExtensions(string hintText) { }

    public void AddHintCount(int n)
    {
        _hintCount += n;
    }
    public virtual IMRStatement GetIMRStatement(string name)
    {
        if (statement_dictionary.TryGetValue(name, out IMRStatement statement))
        {
            
            return statement;
        }
        else
        {
            return new IMRStatement();
        }
    }

}