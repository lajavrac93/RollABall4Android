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
    private delegate void DelegatedFunction();
    private void Start()
    {
        //crono
        {
            speedTimer = 1f;
            sumTime = 0f;
            secondsTimer = 60f;
        }
        StartCoroutine(genericWaitSeconds(3f, endTextStart));

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
    
    //Metodo que maneja que ocurre al final del juego
    public void customGameOver()
    {
        textGameOver.SetActive(true);
        paused = true;
        StartCoroutine(waitToKeyPress());    
    }
    //metodo generico para introducir una espera y una accion (función) tras la misma
    private IEnumerator genericWaitSeconds(float seconds, DelegatedFunction funtion)
    {
        yield return new WaitForSecondsRealtime(seconds);
        funtion();
    }

    //Metodo que hace desaparecer las letras inciales, y despausa el juego.
    private void endTextStart()
    {
        StartCoroutine(genericWaitSeconds(1f, disbleTextStart));
        paused = false;
        rb = GetComponent<Rigidbody>();

        void disbleTextStart()
        {
            textStart.SetActive(false);
        }
    }
    //Metodo para que se espere hasta que se pulsa una tecla y tras esto iniciar de nuevo el juego
    private IEnumerator waitToKeyPress()
    {
        StartCoroutine(genericWaitSeconds(2f,showInfo));
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    //Metodo para mostrar el mensaje informativo después de unos segundos
    private void showInfo()
    {
        textInformativeGameover.enabled=true;
    }


}
