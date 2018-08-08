using UnityEngine;
using System.Collections;

public class CKeyboard
{
	static private bool mInitialized = false;

    //  A 97 , D 100 S 115 W 119
	public const KeyCode KEY_D = KeyCode.D;
	public const KeyCode KEY_A = KeyCode.A;
	public const KeyCode SPACE = KeyCode.Space;
	public const KeyCode ESCAPE = KeyCode.Escape;
	public const KeyCode KEY_W = KeyCode.W;
	public const KeyCode KEY_S = KeyCode.S;

	// TODO: HACER TODO EL TECLADO.
	public const KeyCode KEY_G = KeyCode.G;
	
	public CKeyboard()
	{
		throw new UnityException ("Error in CKeyboard(). You're not supposed to instantiate this class.");
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

	public static bool pressed(KeyCode aKey)
	{
		return Input.GetKey(aKey);
	}

	public static bool firstPress(KeyCode aKey)
	{
		return Input.GetKeyDown (aKey);
	}
}