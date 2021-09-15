using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    CL_CommonFunctionManager commonManager;
    Button thisButton;
    
    void Start()
    {
        commonManager = GameObject.Find("CommonLayer").GetComponent<CL_CommonFunctionManager>();
        thisButton = GetComponent<Button>();

        switch(this.gameObject.name)
        {
            case "ConfigurationBtn":
                thisButton.onClick.AddListener(commonManager.ConfigurationPanelOpen);
                break;
            case "ExitBtn":
                thisButton.onClick.AddListener(commonManager.ExitCommand);
                break;
        }
    }
}
