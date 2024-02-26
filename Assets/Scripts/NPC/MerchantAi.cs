using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MerchantAi : MonoBehaviour, IInteractable
{

    public UnityEvent interactShop;

    private void Start() 
    {
        interactShop.AddListener(GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<PlayerUI>().ShowShop);
    }

    public void Interact()
    {
       interactShop.Invoke();
    }


}
