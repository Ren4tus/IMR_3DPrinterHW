using UnityEngine;

public class EquipDisableTraining : MonoBehaviour
{
    private void OnMouseUp()
    {
        this.gameObject.SetActive(false);
    }
}