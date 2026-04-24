using UnityEngine;
using UnityEngine.UI; // Needed for UI Image

public class Pipe : MonoBehaviour
{
    public Image buttonImage;     // Assign in Inspector
    public Sprite newSprite;      // Assign the new image in Inspector

    public void RotateObject()
    {
        transform.Rotate(0, 0, 90);
    }
    //The tagged out code below changes the sprite

    //public void ChangeButtonImage()
    //{
        //if (buttonImage != null && newSprite != null)
        //{
        //    buttonImage.sprite = newSprite;
       // }
       // else
       // {
       //     Debug.LogWarning("Button Image or Sprite not assigned!");
       // }
    //}
}