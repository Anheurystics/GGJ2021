using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class CustomerSpawner : MonoSingleton<CustomerSpawner>
{
    [SerializeField] private Transform customerRoot;
    [SerializeField] private Customer customerPrefab;
    private List<Customer> spawnedCustomers;
    [SerializeField] private AudioClip sfxLeave;
    [SerializeField] private Bubble bubble;
    public Bubble Bubble => bubble;

    void Start()
    {
        spawnedCustomers = new List<Customer>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            SpawnCustomer(true);
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            SpawnCustomer(false);
        }

        // for debugging a rejection
        if(Input.GetKeyDown(KeyCode.R))
        {
            RejectCustomer();
        }
    }

    public void SpawnCustomer(bool hasItem = false)
    {
        var customer = Instantiate(customerPrefab, customerRoot);
        customer.Spawn(5, hasItem);
        customer.MoveTo(spawnedCustomers.Count);

        spawnedCustomers.Insert(0, customer);
    }

    public void DespawnCustomer()
    {
        if(spawnedCustomers.Count == 0)
        {
            return;
        }

        var sequence = DOTween.Sequence();
        var last = spawnedCustomers.Count - 1;
        spawnedCustomers[last].transform
            .DOLocalMoveX(10f, 2f)
            .SetEase(Ease.Linear);
        spawnedCustomers[last].Sprite.sortingOrder = 1000;
        spawnedCustomers.RemoveAt(last);
        AudioManager.Instance.PlaySFX(sfxLeave, 0.5f);

        if(spawnedCustomers.Count > 0)
        {
            for(int i = 0; i < spawnedCustomers.Count; i++)
            {
                spawnedCustomers[i].MoveTo(spawnedCustomers.Count - 1 - i);
            }
        }
    }

    public void RejectCustomer()
    {
        // Logic for handling a rejection
        // sana ol
        if(spawnedCustomers.Count == 0)
        {
            return;
        }

        var _customer = spawnedCustomers[spawnedCustomers.Count - 1];
        Debug.Log("sad");
        DespawnCustomer();
    }
}
