using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using StoryMaker;

public class xAPIEventListener : MonoBehaviour
{
    private bool _motor_sended = false;
    private bool _reducer_sended = false;
    private bool _battery_sended = false;

    private void Awake()
    {
        EventManager.StartListening("Start", SendStatementByEvent);
        EventManager.StartListening("InitBoolean", InitBoolean);
        //SStoryManager.G.OnStoryOutlineChanged.AddListener(SendStatementByEvent);
    }

    public void InitBoolean()
    {
        _motor_sended = false;
        _reducer_sended = false;
        _battery_sended = false;
    }
    public void SendStatementByEvent()
    {
        XAPIApplication.current.Init();
        //for(int i=0; i<100; i++)
            //Debug.Log(SStoryManager.G.ActiveStoryline.Name);
        //if (SStoryManager.G.ActiveStoryline.Name == "MotorPractice")
        //{
        //    if (!_motor_sended)
        //    {
        //        _motor_sended = true;
        //        XAPIApplication.current.SendMotorStatement("Init");
        //    }
        //}
        //else if (SStoryManager.G.ActiveStoryline.Name == "ReducerPractice")
        //{
        //    if (!_reducer_sended)
        //    {
        //        _reducer_sended = true;
        //        XAPIApplication.current.SendReducerStatement("Init");
        //    }
        //}
        //else if (SStoryManager.G.ActiveStoryline.Name == "BatteryPractice")
        //{
        //    if (!_battery_sended)
        //    {
        //        _battery_sended = true;
        //        XAPIApplication.current.SendBatteryStatement("Init");
        //    }
        //}
    }
}