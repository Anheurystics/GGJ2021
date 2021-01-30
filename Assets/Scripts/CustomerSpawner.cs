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

    void Start()
    {
        spawnedCustomers = new List<Customer>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            SpawnCustomer();
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            DespawnCustomer();
        }
    }

    public void SpawnCustomer()
    {
        var customer = Instantiate(customerPrefab, customerRoot);
        customer.Spawn(5);
        customer.MoveTo(spawnedCustomers.Count, () => {
            ItemManager.Instance.SpawnItem();
            Invoke(nameof(DespawnCustomer), 0.5f);
        });

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

        if(spawnedCustomers.Count > 0)
        {
            for(int i = 0; i < spawnedCustomers.Count; i++)
            {
                spawnedCustomers[i].MoveTo(spawnedCustomers.Count - 1 - i);
            }
        }
    }
}
