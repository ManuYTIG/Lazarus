using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class KeyViewZone : MonoBehaviour
{
    public GameObject keyPrefab;
    public Texture2D texture;
    private Image keyImage;
    private Transform keyInstance;
    private bool playerInZone;

    void Start()
    {
        Sprite s = Sprite.Create(
        texture,
        new Rect(0, 0, texture.width, texture.height),
        new Vector2(0.5f, 0.5f)
        );

        keyInstance = Instantiate(keyPrefab).transform;
        keyInstance.SetParent(transform, worldPositionStays: true);
        keyInstance.localPosition = new Vector3(0, 0, 0);
        keyInstance.gameObject.SetActive(false);
        keyImage = keyInstance.Find("View").GetComponent<Image>();
        keyImage.sprite = s;
    }
    public void ShowKey()
    {
        keyInstance.gameObject.SetActive(true);
        playerInZone = true;
    }
    public void HideKey()
    {
        keyInstance.gameObject.SetActive(false);
        playerInZone = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ShowKey();
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HideKey();
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
