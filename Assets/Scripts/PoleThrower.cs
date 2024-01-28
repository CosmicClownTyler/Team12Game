using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleThrower : MonoBehaviour
{
    [Header("Throwing Force")]
    private float throwForce;
    public float throwForceMinLimit;
    public float throwForceMaxLimit;

    public GameObject polePrefab;
    public GameObject gameLogic;
    public Transform orientation;

    private bool throwForceDirectionUp = true;
    private bool throwableAgain = true;

    // Update is called once per frame
    void Update()
    {
        /*if (!throwableAgain)
            return;*/

        if (Input.GetMouseButton(0))
        {
            ThrowForceSlider();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ThrowPole();

            // Reset back
            gameLogic.GetComponent<GameLogic>().increaseThrows();
            throwForce = throwForceMinLimit;
            throwForceDirectionUp = true;
        }
    }

    void ThrowPole()
    {
        GameObject pole = Instantiate(polePrefab, transform.position, transform.rotation);
        pole.transform.position += new Vector3(1.0f, 0.0f, 1.0f);
        pole.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
        Rigidbody rb = pole.GetComponent<Rigidbody>();

        rb.angularVelocity = Vector3.down * 10.0f;
        rb.AddForce(orientation.transform.forward * throwForce  , ForceMode.VelocityChange);
        rb.AddTorque(Vector3.down * 10000.0f);

        // Wait until everything stops in CollisionProcessor.cs
        throwableAgain = false;
    }

    void ThrowForceSlider()
    {
        if (throwForceDirectionUp == true)
        {
            throwForce += (25 * Time.deltaTime);
        }
        else
        {
            throwForce -= (25 * Time.deltaTime);
        }

        if (throwForceDirectionUp == true && throwForce > throwForceMaxLimit)
        {
            throwForceDirectionUp = false;
        }
        if (throwForceDirectionUp == false && throwForce < throwForceMinLimit)
        {
            throwForceDirectionUp = true;
        }

    }

    public float getForceValue()
    {
        return throwForce;
    }
    public void resetThrowableTrigger()
    {
        throwableAgain = true;
    }

    public bool isThrowable()
    {
        return throwableAgain;
    }
}
