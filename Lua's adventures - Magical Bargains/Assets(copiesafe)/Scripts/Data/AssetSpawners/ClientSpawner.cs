using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject clientPrefab;

    [Header("SpawnPoint")]
    [SerializeField] private float spawnPointX;
    [SerializeField] private float spawnPointY;

    [SerializeField] private GameObject spawnContainer;


    private void Awake()
    {
        clientPrefab.SetActive(false);
    }

    public GameObject SpawnClient(string clientSpritePath)
    {
        Vector3 position = new Vector3(spawnPointX, spawnPointY, 0);
        Sprite sprite = Resources.Load<Sprite>(clientSpritePath);

        var obj = Instantiate(clientPrefab, position, Quaternion.identity);
        obj.GetComponent<SpriteRenderer>().sprite = sprite;

        obj.tag = "currentClient";
        obj.transform.SetParent(spawnContainer.transform);

        obj.SetActive(true);
        return obj;
    }



}