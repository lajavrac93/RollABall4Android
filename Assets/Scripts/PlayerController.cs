using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    TextMeshProUGUI textCronometro, textInformativeGameover;
    [SerializeField]
    GameObject textGameOver, textStart;

    private Rigidbody rb;
    private float speedTimer, sumTime, secondsTimer;
    private static bool paused = true;
    private void Start()
    {
        //crono
        {
            speedTimer = 1f;
            sumTime = 0f;
            secondsTimer = 60f;
        }
        showStarDown();

    }

    private void Update()
    {
        //Control de si el juego está pausado que no siga avanzando
        if (!paused)
        {
            //Crono
            sumTime += Time.deltaTime;
            if (secondsTimer - sumTime <= secondsTimer - speedTimer)
            {
                secondsTimer -= speedTimer;
                sumTime -= speedTimer;
                this.textCronometro.text = "Time: " + ((int)secondsTimer).ToString();
                if (secondsTimer <= 0)
                {
                    customGameOver();
                }
            }

            Vector3 movementDirection = Vector3.zero;
            movementDirection.x = Input.acceleration.x;  //Input.GetAxis("Horizontal");
            movementDirection.z = Input.acceleration.y; //Input.GetAxis("Vertical");
          
            movementDirection.Normalize();
            if (movementDirection.magnitude > 0)
            {
                rb.AddForce(movementDirection * speed, ForceMode.Force);
            }
            else
            {
                Vector3 velocity = rb.velocity;
                rb.AddForce(-velocity, ForceMode.Force);
            }
        }
    }
    //Con el juego pausado en el inicio, muestra la cuenta regresiva y después inicia el juego
    public void showStarDown()
    {
        StartCoroutine(waitSecondsStarting(3f));
        
    }
    //Metodo que maneja que ocurre al final del juego
    public void customGameOver()
    {
        textGameOver.SetActive(true);
        paused = true;
        StartCoroutine(waitToKeyPress());    
    }
    //metodo que maneja el tiempo de espera para empezar, y así prepararse.
    private IEnumerator waitSecondsStarting(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        textStart.SetActive(false);
        paused = false;
        rb = GetComponent<Rigidbody>();
    }
    //Metodo para que se espere hasta que se pulsa una tecla y tras esto iniciar de nuevo el juego
    private IEnumerator waitToKeyPress()
    {
        StartCoroutine(waitToShowInfo());
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    //Metodo para mostrar el mensaje informativo después de unos segundos
    private IEnumerator waitToShowInfo()
    {
        yield return new WaitForSecondsRealtime(2f);
        textInformativeGameover.enabled=true;
    }


}
