using UnityEngine;

namespace SellBro.Items
{
    public class Chest : MonoBehaviour
    {
        public GameObject uI;
        public void ShowUI()
        {
            Debug.Log("UI");
            uI.SetActive(true);
        }
    }
}
