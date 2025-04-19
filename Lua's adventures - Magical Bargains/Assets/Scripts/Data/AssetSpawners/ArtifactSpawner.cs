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

    public GameObject SpawnObject(string artifactSpritePath) {

        Vector3 position = new Vector3(spawnPointX, spawnPointY, 0);
        Sprite sprite = Resources.Load<Sprite>(artifactSpritePath);

        var obj = Instantiate(objectPrefab, position, Quaternion.identity);
        var sr = obj.GetComponent<SpriteRenderer>();
        sr.sprite = sprite;

        // adapt bounding box
        Vector2 S = sr.sprite.bounds.size;
        obj.GetComponent<BoxCollider2D>().size = S;
        //obj.GetComponent<BoxCollider2D>().offset = new Vector2((S.x / 2), 0);

        obj.tag = "currentArtifact";
        obj.transform.SetParent(spawnContainer.transform);

        obj.SetActive(true);

        return obj;
    }



}
