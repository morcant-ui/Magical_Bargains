using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject objectPrefab;

    [Header("SpawnPoint")]
    [SerializeField] private float spawnPointX;
    [SerializeField] private float spawnPointY;

    [SerializeField] private GameObject spawnContainer;


    private void Awake() {
        objectPrefab.SetActive(false);
    }

    public GameObject SpawnObject(Color objectColor, bool hasDefects) {

        Vector3 position = new Vector3(spawnPointX, spawnPointY, 0);
        var obj = Instantiate(objectPrefab, position, Quaternion.identity);
        var sr = obj.GetComponent<SpriteRenderer>();
        sr.color = objectColor;
        obj.tag = "currentClient";
        obj.transform.SetParent(spawnContainer.transform);

        obj.SetActive(true);

        if (hasDefects) {
            // maybe add a defect overlay or blinking animation
            Debug.Log("Defective item spawned.");
        }

        return obj;
    }



}
