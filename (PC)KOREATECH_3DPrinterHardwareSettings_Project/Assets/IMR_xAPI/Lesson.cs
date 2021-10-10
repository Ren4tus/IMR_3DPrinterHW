using System.Collections;
using System.Collections.Generic;
using IMR;


public abstract class Lesson
{
    public Dictionary<string, IMRStatement> statement_dictionary;
    public abstract void Init();
    public abstract void ChangeNewStatement(string name);
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