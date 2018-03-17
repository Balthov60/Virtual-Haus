using System;
using UnityEngine;

public class ValidationPopUp : DefaultPopUp {

    private RayCast rayCast;
    private InputManager inputManager;

    public delegate void Callback();
    public Callback callback;

    new void Start() {
        base.Start();

        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
    }
    public void SetCallback(Callback callback)
    {
        this.callback = callback;
    }
	
	void Update() {
        if (!inputManager.UserClick()) return;

        if (rayCast.GetHit().transform.name == "YesPopUp")
        {
            inputManager.CanClick = false;

            if (callback == null)
            {
                throw new Exception("No Callback defined");
            }
            else
            {
                callback();
                Remove();
            }
        }
        else if (rayCast.GetHit().transform.name == "NoPopUp")
        {
            inputManager.CanClick = false;

            Remove();
        }
	}
}
