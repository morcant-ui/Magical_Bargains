using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ArtifactSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject objectPrefab;

    [Header("SpawnPoint")]
    [SerializeField] private float spawnPointX;
    [SerializeField] private float spawnPointY;

    [SerializeField] private GameObject spawnContainer;

    private string spritePathName = "Sprites";
    private int magnifierLayer = 6;
    private int cameraLayer = 7;



    private void Awake() {
        objectPrefab.SetActive(false);
    }

    public GameObject SpawnObject(string artifactSpriteName, string magnifierSpriteName, string cameraSpriteName) {

        Vector3 position = new Vector3(spawnPointX, spawnPointY, 0);
        Sprite sprite = Resources.Load<Sprite>(Path.Combine(spritePathName, artifactSpriteName));

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


        bool hasMagnifierSprite = !string.IsNullOrWhiteSpace(magnifierSpriteName);
        bool hasCameraSprite = !string.IsNullOrWhiteSpace(cameraSpriteName);

        Sprite hiddenSprite;
        int layer;

        if (hasMagnifierSprite) {
            Debug.Log("Artifact spawner: hidden sprite detected for magnifier");
            hiddenSprite = Resources.Load<Sprite>(Path.Combine(spritePathName, magnifierSpriteName));
            layer = magnifierLayer;
        } else if (hasCameraSprite) {
            Debug.Log("Artifact spawner: hidden sprite detected for camera");
            hiddenSprite = Resources.Load<Sprite>(Path.Combine(spritePathName, cameraSpriteName));
            layer = cameraLayer;
        } else {
            Debug.Log("Artifact spawner: no hidden sprite detected");
            return obj;
        }
      


        var hiddenObj = Instantiate(objectPrefab, position, Quaternion.identity);

        hiddenObj.GetComponent<SpriteRenderer>().sprite = hiddenSprite;

            
        hiddenObj.tag = "currentArtifact";
        hiddenObj.transform.SetParent(obj.transform);

        hiddenObj.layer = layer;
        hiddenObj.GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder + 1;

        hiddenObj.SetActive(true);

        return obj;
    }



}
