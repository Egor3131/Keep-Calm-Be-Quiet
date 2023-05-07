using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUp : MonoBehaviour
{
    private float speed = 1f;
    [SerializeField] TMP_Text itemTextField;
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text statusText;
    [SerializeField] Sprite redKeySprite;
    [SerializeField] Sprite keySprite;
    CanvasGroup canvasGroup => GetComponent<CanvasGroup>();


    private void OnEnable()
    {
        RegularTable.OnDropFound += StartItemFoundPopUp;
        Bookshelf.OnBookCollected += StartItemFoundPopUp;
        Door.OnDoorNoKey += StartKeyNeededPopUp;
    }


    private void OnDisable()
    {
        RegularTable.OnDropFound -= StartItemFoundPopUp;
        Bookshelf.OnBookCollected -= StartItemFoundPopUp;
        Door.OnDoorNoKey -= StartKeyNeededPopUp;
    }

    private void StartKeyNeededPopUp(KeyType keyType)
    {
        StartCoroutine(KeyNeeded(keyType));
    }

    private void StartItemFoundPopUp(Loot item)
    {
        StartCoroutine(ItemFound(item));
    }


    IEnumerator KeyNeeded(KeyType keyType)
    {
        statusText.text = "IS NEEDED";
        if (keyType == KeyType.Red)
        {
            string itemText = "RedKey";
            Sprite itemSprite = redKeySprite;
            itemTextField.text = itemText;
            itemImage.sprite = itemSprite;
        }
        if (keyType == KeyType.Standart)
        {
            string itemText = "Key";
            Sprite itemSprite = keySprite;
            itemTextField.text = itemText;
            itemImage.sprite = itemSprite;
        }

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * speed;
            yield return null;
        }
        yield return new WaitForSeconds(1);
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * speed;
            yield return null;
        }
    }


    IEnumerator ItemFound(Loot item)
    {
        statusText.text = "IS FOUND";
        string itemText = item.itemName;
        Sprite itemSprite = item.sprite;
        itemTextField.text = itemText;
        itemImage.sprite = itemSprite;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * speed;
            yield return null;
        }
        yield return new WaitForSeconds(1);
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * speed;
            yield return null;
        }

    }
}
