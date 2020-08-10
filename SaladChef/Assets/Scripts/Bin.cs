using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(GameConfig.PLAYER))
        {
            collider.gameObject.GetComponent<Player>().PutVegetablesInTrash();
        }
    }
}
