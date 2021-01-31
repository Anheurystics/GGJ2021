using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class CustomerSpawner : MonoSingleton<CustomerSpawner>
{
    public int itemsToGive = 3;
    public float probabilityFound = 0.75f;
    private bool gameIsLive = false;
    [SerializeField] private Transform customerRoot;
    [SerializeField] private Customer customerPrefab;
    [SerializeField] private int customerBufferCount = 10;
    [SerializeField] private int backlogLowerLimit = 4;
    [SerializeField] private Item[] itemPool;

    // pre-generated backlog of customers
    private List<Customer> backlogCustomers;
    // currently shown queue of customers
    private List<Customer> spawnedCustomers;
    public int queueSize {
        get { return spawnedCustomers.Count; }
    }
    public int currentlyServing {
        get { return spawnedCustomers[spawnedCustomers.Count - 1].customerId; }
    }
    [SerializeField] private AudioClip sfxLeave;
    [SerializeField] private AudioClip sfxReject;
    [SerializeField] private Bubble bubble;
    public Bubble Bubble => bubble;

    // sets
    private HashSet<string> lostSet;
    private HashSet<string> foundSet;
    public HashSet<string> heldSet;

    // for generating the backlog
    private int maxCustomersBeforeJanitor;
    private int minCustomersBeforeJanitor;
    private int customersUntilJanitor;

    void Start()
    {
        // Instantiate variables
        spawnedCustomers = new List<Customer>();
        backlogCustomers = new List<Customer>();
        lostSet = new HashSet<string>();
        foundSet = new HashSet<string>();
        heldSet = new HashSet<string>();

        // Define a max number of customers before a janitor appears
        maxCustomersBeforeJanitor = itemsToGive;
        minCustomersBeforeJanitor = 1;  // I've been getting a lot of consecutives
        customersUntilJanitor = 0;

        Debug.Log("max before j " + maxCustomersBeforeJanitor);
        GenerateBacklog();
    }

    float spawnTimer = 0;
    float spawnDuration = 0f;
    void Update()
    {
        if (gameIsLive)
        {
            spawnTimer += Time.deltaTime;
            if(spawnTimer >= spawnDuration)
            {
                spawnTimer = 0f;
                spawnDuration = Random.Range(5, 12);
                
                SpawnCustomer();
            }
        }
        // if(Input.GetKeyDown(KeyCode.S))
        // {
        //     SpawnCustomer(true);
        // }

        // if(Input.GetKeyDown(KeyCode.D))
        // {
        //     SpawnCustomer(false);
        // }

        // for debugging a rejection
        // if(Input.GetKeyDown(KeyCode.R))
        // {
        //     RejectCustomer();
        // }
    }

    public void StartSpawner()
    {
        gameIsLive = true;
    }

    private void GenerateBacklog()
    {
        for (int i = 0; i < customerBufferCount; i++)
        {
            var customer = Instantiate(customerPrefab, customerRoot);
            // don't spawn them in yet
            customer.gameObject.SetActive(false);

            // set customer needs and attributes
            if (customersUntilJanitor <= 0)
            {
                // generate a janitor
                customer.SetCustomerType(true);
                customer.SetNeededItem(null);

                // generate and give items
                var items = GenerateHeldItems();
                customer.SetHeldItems(items);
                customersUntilJanitor = (int) Random.Range(minCustomersBeforeJanitor, maxCustomersBeforeJanitor);
            }
            else
            {
                // generate a client
                customer.SetCustomerType(false);

                var _neededItem = GenerateNeededItem(customer.transform, probabilityFound);
                customer.SetNeededItem(_neededItem);
                customersUntilJanitor--;
            }

            // Add customer to backlog
            backlogCustomers.Add(customer);
        }
    }

    public Item[] GenerateHeldItems()
    {
        // A janitor should never give items that were needed before
        // i.e. generate items outside the Lost set
        var items = new Item[itemsToGive];
        for (int j = 0; j < itemsToGive; j++)
        {
            Item _item = ItemManager.Instance.GenerateRandomItem();
            while (lostSet.Contains(_item.itemSignature))
            {
                // I hate this code but the main purpose is to eventually generate
                // an item that wasn't lost / needed by a previous person
                // When half of the possible combinations have been generated and lost,
                // there's a 0.2% chance this'll loop more than 10 times
                _item = ItemManager.Instance.GenerateRandomItem();
            }
            items[j] = _item;
            foundSet.Add(_item.itemSignature);
        }
        return items;
    }

    public Item GenerateNeededItem(Transform trns, float probFound)
    {
        Item _neededItem = null;
        // A client will often need items that were found before
        if (Random.Range(0f, 1f) <= probFound)
        {
            // Generate a found item
            System.Random rander = new System.Random();
            var foundArray = foundSet.ToArray();
            var randSignature = foundArray[rander.Next(foundArray.Length)];
            Debug.Log("found: " + randSignature);
            _neededItem = ItemManager.Instance.GenerateSpecificItem(randSignature);
            // Remove it from the found set so that it doesn't get generated again
            foundSet.Remove(randSignature);
        }
        else
        {
            // Generate a lost item
            // Or a random item that mightve been found?
            _neededItem = ItemManager.Instance.GenerateRandomItem();
        }
        // Debug.Log(_neededItem);
        // Debug.Log(_neededItem.itemSignature);
        lostSet.Add(_neededItem.itemSignature);
        return _neededItem;
    }

    public void SpawnCustomer()
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
        CustomerSpawner.Instance.Bubble.HideBubble();
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
        Debug.Log("The queue: " + string.Join(",", spawnedCustomers.Select(c => c.customerId)));

        // Generate more bacclog if the queue is less than the limit
        if (backlogCustomers.Count < backlogLowerLimit)
        {
            GenerateBacklog();
            Debug.Log("Generating more customers... Right now: " + (spawnedCustomers.Count + backlogCustomers.Count));
        }
    }

    public void NextCharacteristic()
    {
        Bubble.ShowCharacteristic();
    }

    public void RejectCustomer()
    {
        // Logic for handling a rejection
        // sana ol
        if(spawnedCustomers.Count == 0)
        {
            return;
        }

        AudioManager.Instance.PlaySFX(sfxReject);
        var _customer = spawnedCustomers[spawnedCustomers.Count - 1];
        Debug.Log("sad");

        // Check if rejecting them was the right thing to do
        // :(
        var _sig = _customer.neededItemSignature;
        var _correct = true;
        if (CustomerSpawner.Instance.heldSet.Contains(_sig))
        {
            // you had it all along...
            Debug.Log("Withheld lost item...");
            _correct = false;
        }
        GameManager.Instance.AddCustomerResolved(_correct);
        DespawnCustomer();
    }
}
