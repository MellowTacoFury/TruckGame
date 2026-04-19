using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup : MonoBehaviour
{
    //Billboard pop up
    //Change text for viewer amount
    //destory
    public TextMeshProUGUI viewerText;
    public Image image;
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
    public void DoText(int amount)
    {
        viewerText.text = $"+{amount}";
        image.enabled = true;
        image.gameObject.SetActive(true);
        Destroy(this.gameObject, 2);
    }
}
