using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab;
    public float width = 10f;
    public float height = 5f;
    private bool movingRight = true;
    public float spawnDelay = 0.5f;
    public float speed = 5;
    private float xmax;
    private float xmin;


	// Use this for initialization
	void Start () {
        float distanceToCamera = this.gameObject.transform.position.z - Camera.main.transform.position.z;
        Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
        Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));
        xmax = rightBoundary.x;
        xmin = leftBoundary.x;
        SpawnEnemies();        
	}

    void SpawnEnemies() {
        foreach (Transform child in this.gameObject.transform) {
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
        }
    }

    void SpawnUntilFull() {
        Transform freePosition = NextFreePosition();
        if (freePosition) {
            GameObject enemy = Instantiate(enemyPrefab, freePosition.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePosition;
        }
        if (NextFreePosition()) {
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }


    public void OnDrawGizmos() {
        Gizmos.DrawWireCube(this.gameObject.transform.position, new Vector3(width, height));
    }

    // Update is called once per frame
    void Update () {
        if (movingRight) {
            this.gameObject.transform.position += Vector3.right * speed * Time.deltaTime;
        }else {
            this.gameObject.transform.position += Vector3.left * speed * Time.deltaTime;
        }

        float rightEdgeOfFormation =  this.gameObject.transform.position.x + (0.5f * width);
        float leftEdgeOfFormation = this.gameObject.transform.position.x - (0.5f * width);

        if (leftEdgeOfFormation < xmin) {
            movingRight = true;
        }else if (rightEdgeOfFormation > xmax) {
            movingRight = false;
        }

        if (AllMembersDead()) {
            SpawnEnemies();
        }		
	}

    Transform NextFreePosition() {
        foreach (Transform childPositionGameObject in this.gameObject.transform) {
            if (childPositionGameObject.childCount == 0) {
                return childPositionGameObject;
            }
        }
        return null;
    }

    bool AllMembersDead() {
        foreach(Transform childPositionGameObject in this.gameObject.transform) {
            if(childPositionGameObject.childCount > 0) {
                return false;
            }
        }
        return true;
    }
}
