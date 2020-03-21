using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punto : MonoBehaviour
{
    //Establece el triger para cuando el jugador toque un Punto
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.addPunto();
            Destroy(gameObject);
            
        }
    }
}
