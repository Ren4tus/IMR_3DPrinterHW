using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}


[Serializable]
public class CouresStatus
{
    public string is_review;
    public string is_enroll;
}

[Serializable]
public class ResContentTrainingInfo
{
    public string current_date;
    public string code;
    public CouresStatus body;
    public string message;
}


//KOO
[Serializable]
public class VTCourseStatus
{
    public string ncs_code_name;
    public string course_id;
    public string service_title;
    public string study_days;
    public string cancel_days;
    public string review_days;
    public string course_content_id;
    public string course_short_description;
    public string properties;
    public string course_image_url;
    public string course_syllabus_url;
    public string vt_package_file_url;
    public string mobile_compatibility_code;
    public string course_video;
    public string reformat_install_file_name;
    public string reformat_install_file_url;

}

[Serializable]
public class ResVTCourseInfo
{
    public int total_count;
    public List<VTCourseStatus> list;
}

public class ResVTCourseInfon
{
    public string current_date;
    public string code;
    public ResVTCourseInfo body;
}

