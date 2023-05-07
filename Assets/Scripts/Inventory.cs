using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum KeyType
{
    None,
    Standart,
    Red
}

public class Inventory : MonoBehaviour
{
    private HashSet<KeyType> collectedKeys;
    private Player player => GetComponent<Player>();
    public static event Action OnBatteryCollected = delegate { };
    public static event Action<Vector3> OnKeyCollected = delegate { };

    private void Start()
    {
        collectedKeys = new HashSet<KeyType>();
        collectedKeys.Add(KeyType.None);
    }

    private void OnEnable()
    {
        RegularTable.OnDropFound += CollectItem;
        Bookshelf.OnBookCollected += CollectItem;
    }

    private void OnDisable()
    {
        RegularTable.OnDropFound -= CollectItem;
        Bookshelf.OnBookCollected -= CollectItem;
    }



    private void CollectItem(Loot item)
    {
        if (item.itemName == "Battery")
        {
            OnBatteryCollected?.Invoke();
        }
        if (item.itemName == "RedKey")
        {
            OnKeyCollected?.Invoke(gameObject.transform.position);
            collectedKeys.Add(KeyType.Red);
            player.animator.Play("RedKeyCollect");
        }
        else if (item.itemName == "Key")
        {
            OnKeyCollected?.Invoke(gameObject.transform.position);
            collectedKeys.Add(KeyType.Standart);
            player.animator.Play("KeyCollect");
        }

    }


    public bool HasKey(KeyType requiredType)
    {
        if (collectedKeys.Contains(requiredType))
        {
            return true;
        }
        else return false;
    }
}
