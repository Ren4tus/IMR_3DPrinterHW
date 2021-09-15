using UnityEngine;
using UnityEngine.UI;

public class StructureSubTitleControll : MonoBehaviour
{
    public GameObject[] Objs;
    public string[] subTitle;

    private Text SubTitle;

    private void Start()
    {
        SubTitle = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<Objs.Length; i++)
        {
            if(Objs[i].activeSelf)
            {
                SubTitle.text = subTitle[i];
            }
        }
    }
}
