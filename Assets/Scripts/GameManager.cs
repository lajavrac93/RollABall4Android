using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Establecer donde se escriben los puntos
    [SerializeField]
    private TextMeshProUGUI textPuntos, textPuntuacionMaxima;
    //Establecer el objeto punto para poder instanciarlo facilmente
    [SerializeField]
    private GameObject puntoPrefab;

    //variable que controla la puntuación
    private int puntos, maxPuntos;

    private void Start()
    {
        //establecer que no se quede inactiva la pantalla
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        puntos = 0;
        if (PlayerPrefs.HasKey("bestScore") == true)
        {
            maxPuntos = PlayerPrefs.GetInt("bestScore");
        }
        else
        {
            maxPuntos = puntos;
        }
        textPuntuacionMaxima.text = "Best: " + maxPuntos.ToString();
        textPuntos.text = "Puntos: " + puntos.ToString();
    }
    //Metodo que suma puntos y establece el tiempo para que se instancie el siguiente
    public void addPunto()
    {
        puntos++;
        textPuntos.text = "Puntos: " + puntos.ToString();
        if (puntos > maxPuntos)
        {
            maxPuntos = puntos;
            textPuntuacionMaxima.text = "Best: " + maxPuntos.ToString();
        }
        StartCoroutine(waitSecondsPuntosRespawn(2f));
    }
    //Metodo que con un tiempo instancia después del mismo el objeto del prefab introducido, en este caso Punto
    private IEnumerator waitSecondsPuntosRespawn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Instantiate(puntoPrefab, new Vector3(Random.Range(-4.27f, 4.27f), 0.15f, Random.Range(-3.54f, 4.17f)), Quaternion.identity);
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("bestScore", maxPuntos);
        PlayerPrefs.Save();
    }
}
 

