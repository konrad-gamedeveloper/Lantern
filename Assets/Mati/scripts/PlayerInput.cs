using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// nadanie pierwszenstwa skryptowi żeby nie było laga przy naciskaniu klawiszy
[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public float horizontal;      //Float przechowujący input horyzontalny
    [HideInInspector] public bool jumpHeld;         //Bool przechowujący nacisnięcie skoku
    [HideInInspector] public bool jumpPressed;      //Bool przechowujący trzymanie skoku
    [HideInInspector] public bool crouchHeld;       //Bool przechowujący nacisnięcie kucania
    [HideInInspector] public bool crouchPressed;    //Bool przechowujący trzymanie kucania

    bool readyToClear;                              //Bool uzywany do synchronizacji inputów
    
    


    // Update is called once per frame
    void Update()
    {
        //czyszczenie istniejącuch inputów
        ClearInput();
    
        //przetwarzanie inputów
        ProcessInputs();
        
        // ustawienie horyzontalnych inputów pomiędzy -1 i 1
        horizontal = Mathf.Clamp(horizontal, -1f, 1f);
    }

    private void FixedUpdate()
    {
        readyToClear = true;
    }
    void ClearInput()
    {
        //jesi funkcja nie jest true, wyjdz
        if (!readyToClear)
            return;

        //resetujemy wszystkie inputy
        horizontal      = 0f;
        jumpPressed     = false;
        jumpHeld        = false;
        crouchPressed   = false;
        crouchHeld      = false;

        readyToClear    = false;
    }
    void ProcessInputs()
    {
        //zbieranie inputów horyzontalnych
        horizontal      += Input.GetAxis("Horizontal");

        // zbieranie inputów skoku 
        jumpPressed     = jumpPressed || Input.GetButtonDown("Jump");
        jumpHeld        = jumpHeld || Input.GetButton("Jump");

        // zbieranie inputów kucania
        crouchPressed = crouchPressed || Input.GetButtonDown("Crouch");
        crouchHeld      = crouchHeld || Input.GetButton("Crouch");
    }
    
}
