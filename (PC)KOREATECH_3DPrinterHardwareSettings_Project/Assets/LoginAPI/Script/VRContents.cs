using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//임시(temporary)
public enum ContentsType
{
    NONE = 0,
    /*
	//YEAR2012,
	스크류식칠러시스템 = 168523,

	//YEAR2013,
	클린디젤엔진자동차 = 168513,
	태양광발전설치및유지관리 = 168519,
	패시브하우스건축설계 = 168509,

	//YEAR2014,
	배전접지시공 = 168499,
	유압요소설계 = 168505,
	조경기반시설관리 = 168503,
	클린룸시스템제어및유지관리 = 168495,

	//YEAR2015,
	범용밀링머신 = 168497,
	토탈스테이션과GNSS를이용한측량 = 168507,
	화력발전소시설 = 168545,
	흡수식냉동장치 = 168511,
	반도체CMOS제조공정 = 168549,

	//YEAR2016,
	머시닝센터좌표계설정 = 168599,
	지게차운전기능사실기 = 168515,
	플라스틱성형시험사출 = 168517,
	유압비례제어 = 168521,

	//YEAR2017,
	LED칩제작공정 = 168529,
	배관배선공사실무 = 168525,
	범용선반가공단순현상 = 168563,
	피복아크용접비드쌓기 = 168579,
	자동차섀시정비훈련조향장치제동장치 = 168531,
	전기유압서보제어 = 168535,
	화학물질취급실험실안전체험 = 168587,
	천장크레인안전교육 = 168583,
	흡수식냉온수기유지보수훈련 = 168591,
	화력발전보일러주요설비유지보수 = 168595,

	//YEAR2018,
	공기압제어 = 168539,
	굴삭기운전 = 168551,
	머시닝센터조작 = 168559,
	발전설비진단진동 = 168541,
	범용선반가공홈테이퍼 = 168573,
	분광분석 = 168527,
	전기자동차구동장치정비 = 168537,
	지멘스PLC제어기초 = 168543,
	피복아크용접맞대기용접 = 168555,
	항공기기체정비 = 168547,

	//YEAR2019,
	BIPV형태양광구조불시공 = 168581,
	CO2용접아래보기비드놓기 = 168553,
	머시닝센터수기프로그래밍 = 168557,
	멜섹PLC제어컨베이어시스템 = 168533,
	배전공사실습가공배전 = 168561,
	사고사례로배우는산업안전보건훈련작업자편 = 168585,
	순수공압제어 = 168571,
	엘리베이터구조와원리 = 168565,
	엘리베이터일상점검 = 168567,
	영농형태양광발전설비시공 = 168577,
	지중배전공사 = 168569,
	자동차가솔린엔진정비전자제어장치 = 168593,
	자동차가솔린엔진정비기계장치 = 168597,
	스크류칠러시스템유지관리 = 168589,
    */

    //YEAR2012,
    //스크류식칠러시스템 = 168523,

    //YEAR2013,
    클린디젤엔진자동차 = 6355,
    태양광발전설치및유지관리 = 6352,
    패시브하우스건축설계 = 6358,

    //YEAR2014,
    배전접지시공 = 104103,
    유압요소설계 = 104123,
    조경기반시설관리 = 6364,
    클린룸시스템제어및유지관리 = 6361,

    //YEAR2015,
    범용밀링머신 = 104017,
    토탈스테이션과GNSS를이용한측량 = 104031,
    화력발전소시설 = 104137,
    흡수식냉동장치 = 104141,
    반도체CMOS제조공정 = 104077,

    //YEAR2016,
    머시닝센터좌표계설정 = 104097,
    지게차운전기능사실기 = 104127,
    플라스틱성형시험사출 = 104135,
    유압비례제어 = 104121,

    //YEAR2017,
    LED칩제작공정 = 104069,
    배관배선공사실무 = 104079,
    범용선반가공단순현상 = 104105,
    피복아크용접비드쌓기 = 6415,
    자동차섀시정비훈련조향장치제동장치 = 104125,
    전기유압서보제어 = 104089,
    화학물질취급실험실안전체험 = 6427,
    천장크레인안전교육 = 6430,
    흡수식냉온수기유지보수훈련 = 104143,
    화력발전보일러주요설비유지보수 = 6424,

    //YEAR2018,
    공기압제어 = 104071,
    굴삭기운전 = 104093,
    머시닝센터조작 = 104095,
    발전설비진단진동 = 104101,
    범용선반가공홈테이퍼 = 104091,
    분광분석 = 104107,
    전기자동차구동장치정비 = 6457,
    지멘스PLC제어기초 = 104129,
    피복아크용접맞대기용접 = 6445,
    항공기기체정비 = 6460,

    //YEAR2019,
    BIPV형태양광구조불시공 = 104065,
    CO2용접아래보기비드놓기 = 104067,
    머시닝센터수기프로그래밍 = 104073,
    멜섹PLC제어컨베이어시스템 = 104099,
    배전공사실습가공배전 = 104081,
    사고사례로배우는산업안전보건훈련작업자편 = 104109,
    순수공압제어 = 104111,
    엘리베이터구조와원리 = 104115,
    엘리베이터일상점검 = 104117,
    영농형태양광발전설비시공 = 104119,
    지중배전공사 = 104131,
    자동차가솔린엔진정비전자제어장치 = 104061,
    자동차가솔린엔진정비기계장치 = 104059,
    스크류칠러시스템유지관리 = 104063,

    //YEAR2020,
    쓰리디프린터HW설정 = 108239,
    전동기기동을위한동력설비공사 = 108233,
    감전방지및설비보호를위한접지설비공사 = 108219,
    화력발전소대용량송풍기정비초급 = 108235,
    자동차냉난방장치정비 = 111287,
    가솔린자동차배출가스정비 = 108399,
    가스텅스텐아크용접맞대기용접 = 108375,
    CNC선반조작 = 108351,
    CNC선반가공프로그래밍 = 108213,
    관류보일러설비설치 = 111301,


}

public struct VRCourse
{
    public string Name { get; set; }
    public string ContentsID { get; set; }
    public string CourseID { get; set; }

    public VRCourse(string courseID, string name, string contentsID)
    {
        Name = name;
        ContentsID = contentsID;
        CourseID = courseID;
    }
}

public static class VRContents
{
    static VRContents()
    {
        // Add Contents 
        m_VRContents.Add(new VRCourse("168497", "범용밀링머신", "104017"));
        m_VRContents.Add(new VRCourse("168507", "토탈스테이션과 GNSS를 이용한 측량", "104031"));
        m_VRContents.Add(new VRCourse("168511", "흡수식 냉동장치", "104141"));
        m_VRContents.Add(new VRCourse("168515", "지게차 운전기능사 실기", "104127"));
        m_VRContents.Add(new VRCourse("168517", "플라스틱성형 시험사출", "104135"));
        m_VRContents.Add(new VRCourse("168521", "유압비례제어", "104121"));
        m_VRContents.Add(new VRCourse("168525", "배관배선공사 실무", "104079"));
        m_VRContents.Add(new VRCourse("168529", "LED칩 제작 공정", "104069"));
        m_VRContents.Add(new VRCourse("168531", "자동차 섀시정비 훈련: 조향장치, 제동장치", "104125"));
        m_VRContents.Add(new VRCourse("168535", "전기유압 서보제어", "104089"));
        m_VRContents.Add(new VRCourse("168539", "공기압제어", "104071"));
        m_VRContents.Add(new VRCourse("168543", "지멘스PLC 제어 기초", "104129"));
        m_VRContents.Add(new VRCourse("168545", "화력 발전소 시설", "104137"));
        m_VRContents.Add(new VRCourse("168549", "반도체 CMOS 제조공정", "104077"));
        m_VRContents.Add(new VRCourse("168553", "CO2용접 아래보기 비드놓기", "104067"));
        m_VRContents.Add(new VRCourse("168557", "머시닝센터 수기 프로그래밍", "104073"));
        m_VRContents.Add(new VRCourse("168561", "배전공사실습-가공배전", "104081"));
        m_VRContents.Add(new VRCourse("168565", "엘리베이터 구조와 원리", "104115"));
        m_VRContents.Add(new VRCourse("168567", "엘리베이터 일상점검", "104117"));
        m_VRContents.Add(new VRCourse("168569", "지중배전 공사", "104131"));
        m_VRContents.Add(new VRCourse("168571", "순수공압제어", "104111"));
        m_VRContents.Add(new VRCourse("168577", "영농형 태양광 발전설비 시공", "104119"));
        m_VRContents.Add(new VRCourse("168581", "BIPV형 태양광 구조불 시공", "104065"));
        m_VRContents.Add(new VRCourse("168585", "사고사례로 배우는 산업안전보건훈련-작업자편", "104109"));
        m_VRContents.Add(new VRCourse("168589", "스크류 칠러 시스템 유지관리", "104063"));
        m_VRContents.Add(new VRCourse("168593", "자동차 가솔린엔진 정비-전자제어장치", "104061"));
        m_VRContents.Add(new VRCourse("168597", "자동차 가솔린엔진 정비-기계장치", "104059"));
        m_VRContents.Add(new VRCourse("168599", "머시닝센터-좌표계 설정", "104097"));
        m_VRContents.Add(new VRCourse("168595", "화력발전 보일러 주요설비 유지보수", "6424"));
        m_VRContents.Add(new VRCourse("168591", "흡수식냉온수기 유지보수 훈련", "104143"));
        m_VRContents.Add(new VRCourse("168587", "화학물질 취급 실험실 안전체험", "6427"));
        m_VRContents.Add(new VRCourse("168583", "천장크레인 안전교육", "6430"));
        m_VRContents.Add(new VRCourse("168579", "피복아크용접-비드 쌓기", "6415"));
        m_VRContents.Add(new VRCourse("168563", "범용선반가공-단순 현상", "104105"));
        m_VRContents.Add(new VRCourse("168573", "범용선반가공-홈·테이퍼", "104091"));
        m_VRContents.Add(new VRCourse("168559", "머시닝센터 조작", "104095"));
        m_VRContents.Add(new VRCourse("168555", "피복아크용접-맞대기 용접", "6445"));
        m_VRContents.Add(new VRCourse("168551", "굴삭기 운전", "104093"));
        m_VRContents.Add(new VRCourse("168547", "항공기 기체 정비", "6460"));
        m_VRContents.Add(new VRCourse("168541", "발전설비진단-진동", "104101"));
        m_VRContents.Add(new VRCourse("168537", "전기자동차 구동장치 정비", "6457"));
        m_VRContents.Add(new VRCourse("168533", "멜섹 PLC DI/O 제어-컨베이어시스템", "104099"));
        m_VRContents.Add(new VRCourse("168527", "분광분석(UV-VIS, FT-IR, AAS)", "104107"));
        m_VRContents.Add(new VRCourse("168523", "스크류식 칠러시스템", "104113"));
        m_VRContents.Add(new VRCourse("168519", "태양광 발전 설치 및 유지관리", "6352"));
        m_VRContents.Add(new VRCourse("168513", "클린디젤 엔진 자동차", "6355"));
        m_VRContents.Add(new VRCourse("168509", "패시브 하우스 건축설계", "6358"));
        m_VRContents.Add(new VRCourse("168505", "유압요소설계", "104123"));
        m_VRContents.Add(new VRCourse("168503", "조경기반 시설관리", "6364"));
        m_VRContents.Add(new VRCourse("168499", "배전 접지 시공", "104103"));
        m_VRContents.Add(new VRCourse("168495", "클린룸 시스템 제어 및 유지관리", "104133"));
        m_VRContents.Add(new VRCourse("178763", "3D 프린터 HW 설정", "108239"));
        m_VRContents.Add(new VRCourse("178759", "전동기 기동을 위한 동력설비공사", "108233"));
        m_VRContents.Add(new VRCourse("178755", "감전 방지 및 설비 보호를 위한 접지설비공사", "108219"));
        m_VRContents.Add(new VRCourse("178761", "화력발전소 대용량 송풍기 정비-초급", "108235"));
        m_VRContents.Add(new VRCourse("185901", "자동차 냉난방장치 정비", "111287"));
        m_VRContents.Add(new VRCourse("179203", "가솔린 자동차 배출가스장치 정비", "108399"));
        m_VRContents.Add(new VRCourse("179197", "가스텅스텐아크용접 맞대기용접", "108375"));
        m_VRContents.Add(new VRCourse("178777", "CNC선반 조작", "108351"));
        m_VRContents.Add(new VRCourse("178769", "CNC선반 가공 프로그래밍", "108213"));
        m_VRContents.Add(new VRCourse("185909", "관류보일러 설비 설치", "111301"));

    }

    public static List<VRCourse> m_VRContents = new List<VRCourse>();
    public static ContentsType m_ContentsType;

}


//KOO

public struct VRCourseKoo
{
    public string ncs_code_name { get; set; }
    public string course_id { get; set; }
    public string service_title { get; set; }
    public string study_days { get; set; }
    public string cancel_days { get; set; }
    public string review_days { get; set; }
    public string course_content_id { get; set; }
    public string course_short_description { get; set; }
    public string properties { get; set; }
    public string course_image_url { get; set; }
    public string course_syllabus_url { get; set; }
    public string vt_package_file_url { get; set; }
    public string mobile_compatibility_code { get; set; }
    public string course_video { get; set; }
    public string reformat_install_file_name { get; set; }
    public string reformat_install_file_url { get; set; }


    public VRCourseKoo(string ncs, string courseID, string serviceTitle, string sdays, string cdays, string rdays, string contentsID, string csd, string pro, string ciu, string csu, string vpfu, string mcc, string cv, string rifn, string rifu)
    {
        course_id = courseID;
        service_title = serviceTitle;
        course_content_id = contentsID;
        ncs_code_name = ncs;
        study_days = sdays;
        cancel_days = cdays;
        review_days = rdays;
        course_short_description = csd;
        properties = pro;
        course_image_url = ciu;
        course_syllabus_url = csu;
        vt_package_file_url = vpfu;
        mobile_compatibility_code = mcc;
        course_video = cv;
        reformat_install_file_name = rifn;
        reformat_install_file_url = rifu;

    }
}


public static class VRContentsKoo
{
    static VRContentsKoo()
    {
        // Add Contents        
    }

    public static List<VRCourseKoo> m_VRContentsKoo = new List<VRCourseKoo>();
    public static ContentsType m_ContentsType;
}



public struct VRCourseKoo2
{

    public string course_id { get; set; }
    public string service_title { get; set; }
    public string course_content_id { get; set; }


    public VRCourseKoo2(string courseID, string serviceTitle, string contentsID)
    {
        course_id = courseID;
        service_title = serviceTitle;
        course_content_id = contentsID;


    }
}


public static class VRContentsKoo2
{
    static VRContentsKoo2()
    {
        // Add Contents        
    }

    public static List<VRCourseKoo2> m_VRContentsKoo2 = new List<VRCourseKoo2>();
    public static ContentsType m_ContentsType;
}
