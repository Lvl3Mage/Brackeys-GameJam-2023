using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class ShopManager : MonoBehaviour
{
    public GameObject shopManagerPanel;
    void Start()
    {
        shopManagerPanel.SetActive(false);
    }
    
    public void OpenShop()
    {
        // On Something do open the shop
        shopManagerPanel.SetActive(true);
    }
    public void CloseShop()
    {
        // On Something do close the shop
        shopManagerPanel.SetActive(false);
    }
}
