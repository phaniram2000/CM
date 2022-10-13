using UnityEngine;
using UnityEngine.UI;


public class TBRCanvasItemsButtonController : MonoBehaviour
{
    public void OnItemsButtonPressedChangeColor(Button button)
    {
        button.GetComponent<Image>().color = Color.green;
        
    }

    public void OnItemsButtonPressed(int id)
    {
       TBREvents.InvokeOnItemsButtonPressed(id);
    }

}
