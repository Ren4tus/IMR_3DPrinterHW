using UnityEngine;

public class MainTest : MonoBehaviour
{
    RectTransform transform_image;

    void Start()
    {
        transform_image = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Update_MousePosition();
    }

    private void Update_MousePosition()
    {
        Vector2 mousePos = Input.mousePosition;

        mousePos.x = (mousePos.x - (Screen.width/2))/10;
        mousePos.y = (mousePos.y - (Screen.height / 2)) / 10;

        transform_image.position = new Vector2((Screen.width/2)+mousePos.x, (Screen.height/2)+mousePos.y);
    }

    private void Update_ClickParticle()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}
