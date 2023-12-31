﻿using UnityEngine;
using System.Collections;

// Possible movement directions
public enum MOVEDIRECTION {UP,DOWN,LEFT,RIGHT,NONE};
// Weapons
public enum WEAPON {SWORD,BULLET,LASTWEAPON};

public class PlayerMove : MonoBehaviour 
{
	public MOVEMENTDIRECTION movementDirection {get; private set;}
	MOVEMENTDIRECTION lookingTo;
	public WEAPON weapon {get; private set;}

	public GameObject bullet,sword;

	public float speed = 0.05f;

	bool inventoryMode = false;

    //PLAYER DETECTION
    //----------
    public GameObject box;
    public GameObject carrito;

    private bool taking = false;
    //----------

	void Start () 
    {
		movementDirection = MOVEMENTDIRECTION.DOWN;
		lookingTo = MOVEMENTDIRECTION.DOWN;
	}

	void Update () 
    {
		if (RoomManager.Instance.Pause) return;

		// Inventory and map screen interaction WORK IN PROGRESS
		if (Input.GetKeyUp (KeyCode.P)) 
        {
			if (inventoryMode) {
				RoomManager.Instance.Inventory.SetActive(false);
				inventoryMode = false;
			} else {
				RoomManager.Instance.Inventory.SetActive(true);
				inventoryMode = true;
			}
		}
		if (inventoryMode) 
        {
			if (Input.GetKeyUp (KeyCode.D)) 
            {
				weapon++;
				if (weapon == WEAPON.LASTWEAPON) weapon = 0;
			} else
			if (Input.GetKeyUp (KeyCode.A)) 
            {
				weapon--;
				if (weapon < 0) weapon = WEAPON.LASTWEAPON-1;
			}
			GameObject selector = GameObject.Find ("selector");
			GameObject selectorStart = GameObject.Find ("selectorstart");
			selector.transform.position = selectorStart.transform.position + new Vector3((float)weapon,0,-4); 
		} else 
        {
			checkAction();
		}

        if (Input.GetKey(KeyCode.C))
            speed = 0.05f;
        else if (Input.GetKey(KeyCode.V))
            speed = 0.0125f;
        else
            speed = 0.025f;

        Movement();
	}

    void Movement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, speed, 0);
            movementDirection = MOVEMENTDIRECTION.UP;
        }
        else
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += new Vector3(0, -speed, 0);
                movementDirection = MOVEMENTDIRECTION.DOWN;
            }
            else
                if (Input.GetKey(KeyCode.A))
                {
                    transform.position += new Vector3(-speed, 0, 0);
                    movementDirection = MOVEMENTDIRECTION.LEFT;
                }
                else
                    if (Input.GetKey(KeyCode.D))
                    {
                        transform.position += new Vector3(speed, 0, 0);
                        movementDirection = MOVEMENTDIRECTION.RIGHT;
                    }
                    else
                        movementDirection = MOVEMENTDIRECTION.NONE;

        if (movementDirection != MOVEMENTDIRECTION.NONE) lookingTo = movementDirection;
    }

	//USE WEAPON
	void checkAction() 
    {
        //MELEE ATTACK
        //----------
		if (Input.GetKeyDown (KeyCode.I)) 
        {
			GameObject b;

			switch(weapon) 
            {
			case WEAPON.SWORD: b = Instantiate(sword,transform.position,transform.rotation) as GameObject;
				break;
			default:
				b = Instantiate(bullet,transform.position,transform.rotation) as GameObject;
                Debug.Log("Melee");
				break;
			}
			
			FixedMove fm = b.GetComponent("FixedMove") as FixedMove;
			fm.movementDirection = lookingTo;
			
			switch(lookingTo) 
            {
			case MOVEMENTDIRECTION.UP: 								
				b.transform.position+=new Vector3(0,0.32f,0);			
				break;
			case MOVEMENTDIRECTION.DOWN: 
				b.transform.position+=new Vector3(0,-0.32F,0);
				b.transform.Rotate(0,0,180);
				break;
			case MOVEMENTDIRECTION.LEFT: 
				b.transform.position+=new Vector3(-0.32f,0,0);
				b.transform.Rotate(0,0,90);
				break;
			case MOVEMENTDIRECTION.RIGHT: 
				b.transform.position+=new Vector3(0.32f,0,0);
				break;
			}
	    }
        //----------

	        //DISTANCE ATTACK
	        //----------
	        if (Input.GetKeyDown(KeyCode.O))
	        {
	            GameObject v;

	            switch (weapon)
	            {
	                case WEAPON.BULLET: v = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
	                    break;
	                default:
	                    v = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
                        Debug.Log("Distance");
	                    break;
	            }

	            FixedMove fm2 = v.GetComponent("FixedMove") as FixedMove;
	            fm2.movementDirection = lookingTo;

	            switch (lookingTo)
	            {
	                case MOVEMENTDIRECTION.UP:
	                    v.transform.position += new Vector3(0, 0.32f, 0);
	                    v.transform.Rotate(0, 0, 90);
	                    break;
	                case MOVEMENTDIRECTION.DOWN:
	                    v.transform.position += new Vector3(0, -0.32F, 0);
	                    v.transform.Rotate(0, 0, -90);
	                    break;
	                case MOVEMENTDIRECTION.LEFT:
	                    v.transform.position += new Vector3(-0.32f, 0, 0);
	                    v.transform.Rotate(0, 0, 180);
	                    break;
	                case MOVEMENTDIRECTION.RIGHT:
	                    v.transform.position += new Vector3(0.32f, 0, 0);
	                    break;
	        }
	    }
	    //----------
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Box")
        {
            Debug.Log("Touching");
            if (Input.GetKey(KeyCode.F))
            {
                Debug.Log("Taking");
                box.transform.position = transform.position;
                taking = true;
                if (taking == true)
                    box.transform.position = transform.position;
            }
            if (Input.GetKey(KeyCode.E) && taking == true)
            {
                //box.transform.position += new Vector3(0, 1.28f, 0);
                if(movementDirection == MOVEMENTDIRECTION.UP)
                {
                    box.transform.position += new Vector3(0, -1.28f, 0);
                }
                if (movementDirection == MOVEMENTDIRECTION.DOWN)
                {
                    box.transform.position += new Vector3(0, 1.28f, 0);
                }
                if (movementDirection == MOVEMENTDIRECTION.LEFT)
                {
                    box.transform.position += new Vector3(-1.28f, 0, 0);
                }
                if (movementDirection == MOVEMENTDIRECTION.RIGHT)
                {
                    box.transform.position += new Vector3(1.28f, 0, 0);
                }
            }
            else if (Input.GetKey(KeyCode.Q) && taking == true)
            {
                //box.transform.position += new Vector3(0.32f, 0, 0);
                if (movementDirection == MOVEMENTDIRECTION.UP)
                {
                    box.transform.position += new Vector3(0, -0.32f, 0);
                }
                if (movementDirection == MOVEMENTDIRECTION.DOWN)
                {
                    box.transform.position += new Vector3(0, 0.32f, 0);
                }
                if (movementDirection == MOVEMENTDIRECTION.LEFT)
                {
                    box.transform.position += new Vector3(-0.32f, 0, 0);
                }
                if (movementDirection == MOVEMENTDIRECTION.RIGHT)
                {
                    box.transform.position += new Vector3(0.32f, 0, 0);
                }
            }
        }

        if (other.tag == "Carrito")
        {
            if (Input.GetKey(KeyCode.E))
            {
                carrito.transform.position += new Vector3(2.56f, 0, 0);
            }
        }
    }
}