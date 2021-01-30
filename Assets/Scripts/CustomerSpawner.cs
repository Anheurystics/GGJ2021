using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class CustomerSpawner : MonoSingleton<CustomerSpawner>
{
    public int itemsToGive = 3;
    [SerializeField] private Transform customerRoot;
    [SerializeField] private Customer customerPrefab;
    [SerializeField] private int customerCount;

    // pre-generated backlog of customers
    private List<Customer> backlogCustomers;
    // currently shown queue of customers
    private List<Customer> spawnedCustomers;
    [SerializeField] private AudioClip sfxLeave;
    [SerializeField] private Bubble bubble;
    public Bubble Bubble => bubble;

    void Start()
    {
        // Instantiate variables
        spawnedCustomers = new List<Customer>();
        backlogCustomers = new List<Customer>();

        // Pre-generate a random list of customers
        customerCount = 10;

        // Define a max number of janitors to serve
        int maxJanitors = (customerCount / itemsToGive) + 1;
        int currJanitors = 0;

        // Define a max number of customers before a janitor appears
        int maxCustomersBeforeJanitor = itemsToGive;
        int customersUntilJanitor = 0;

        Debug.Log("max " + maxJanitors);
        Debug.Log("max before j " + maxCustomersBeforeJanitor);

        for (int i = 0; i < customerCount; i++)
        {
            var customer = Instantiate(customerPrefab, customerRoot);
            // don't spawn them in yet
            customer.gameObject.SetActive(false);

            // set customer needs and attributes
            bool hasItem = false;
            if (customersUntilJanitor <= 0 && currJanitors <= maxJanitors)
            {
                hasItem = true;
                currJanitors++;
                customersUntilJanitor = (int) Random.Range(0, maxCustomersBeforeJanitor);
            }
            customer.SetCustomerType(hasItem);

            backlogCustomers.Add(customer);
            customersUntilJanitor--;
        }
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
        if (backlogCustomers.Count <= 0)
        {
            // backlog empty
            return;
        }

        var customer = backlogCustomers[0];
        backlogCustomers.RemoveAt(0);

        // time to spawn them in
        customer.gameObject.SetActive(true);
        customer.Spawn(5);
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
