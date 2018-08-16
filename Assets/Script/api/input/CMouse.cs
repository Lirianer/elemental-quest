﻿using UnityEngine;
using System.Collections;

public class CMouse
{
	static private bool mInitialized = false;

    public enum BUTTONS : int {
		LEFT = 0,
		RIGHT = 1,
		MIDDLE = 2,
		ANY = 3
	}

	public CMouse()
	{
		throw new UnityException ("Error in CMouse(). You're not supposed to instantiate this class.");
	}

	public static void init()
	{
		if (mInitialized) 
		{
			return;
		}
		mInitialized = true;
	}

	public static void update()
	{
	}

	public static void destroy()
	{
		if (mInitialized) 
		{
			mInitialized = false;
		}
	}
    

    public static bool firstPress(BUTTONS value = BUTTONS.ANY)
    {
        if ((value != BUTTONS.ANY && Input.GetMouseButtonDown((int)value)) || ( value == BUTTONS.ANY && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool pressed(BUTTONS value = BUTTONS.ANY)
	{
		if ((value != BUTTONS.ANY && Input.GetMouseButtonDown((int)value)) || ( value == BUTTONS.ANY && (Input.GetMouseButton (0) || Input.GetMouseButton (1) || Input.GetMouseButton (2)))) 
		{
			return true;
		}
		else 
		{
			return false;
		}
	}

	public static bool release(BUTTONS value = BUTTONS.ANY)
	{
		if ((value != BUTTONS.ANY && Input.GetMouseButtonDown((int)value)) || ( value == BUTTONS.ANY && (Input.GetMouseButtonUp (0) || Input.GetMouseButtonUp (1) || Input.GetMouseButtonUp (2))))
		{
			return true;
		}
		else 
		{
			return false;
		}
	}

	public static Vector3 getPos()
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		pos.y *= - 1;
		pos.z = 0;
		return pos;
	}

	public static float getX()
	{
		return getPos ().x;
	}

	public static float getY()
	{
		return getPos ().y;
	}
}
