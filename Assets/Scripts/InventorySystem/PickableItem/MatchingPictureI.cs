using Unity.VisualScripting;
using UnityEngine;

public class MatchingPicture : MonoBehaviour
{
    public bool isMatched = false;
    public int idToCheck = 0;
    public GameObject fullPicture;
    public void CheckPicture()
    {
        if (InventorySystem.instance == null) return;
        if (InventorySystem.instance.getInventory(idToCheck) == null) return;
        if (InventorySystem.instance.getInventory(idToCheck).amount <= 0) return;

        if (isMatched) return;
        GameObject go = Instantiate(fullPicture);
    }    
}
