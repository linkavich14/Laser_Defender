using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    public float damage = 10f;

    public float GetDamage() {
        return damage;
    }

    public void Hit() {
        Destroy(this.gameObject);
    }
}
