using UnityEngine;
using System.Collections;

public class CMainMenuState : CGameState
{
	private CSprite mBackground;

	private CButtonSprite mButtonPlay;

	int[] a = { 0, 1, 2, 3, 4, 5 };

	public CMainMenuState()
	{
		//Debug.Log (fact (5));
		///mostrar (a, 0);
	}

	private void swap(int[] aArray, int a, int b)
	{
		int aux;
		aux = aArray [a];
		aArray [a] = aArray[b];
		aArray [b] = aux;
	}



	private float fact(float n)
	{
		if (n == 0)
			return 1;
		else
			return n * fact (n - 1);
	}

	private void mostrar(int[] aArray, int aIndex)
	{
		if (aIndex < aArray.Length) 
		{
			Debug.Log (aArray [aIndex]);
			mostrar(aArray, aIndex+1);
		}
	}


	override public void init()
	{
		base.init ();

		mBackground = new CSprite ();
		mBackground.setImage (Resources.Load<Sprite> ("Sprites/menu/menu_background"));
		mBackground.setXY (0, 0);
		mBackground.setSortingLayerName("Background");
		mBackground.setName ("background");

		mButtonPlay = new CButtonSprite ();
		mButtonPlay.setFrames (Resources.LoadAll<Sprite> ("Sprites/ui"));
		mButtonPlay.gotoAndStop (1);
		mButtonPlay.setXY (CGameConstants.SCREEN_WIDTH / 2, CGameConstants.SCREEN_HEIGHT / 2);
		mButtonPlay.setWidth (190);
		mButtonPlay.setHeight (50);
		mButtonPlay.setSortingLayerName ("UI");
		mBackground.setName ("button");
	}
	
	override public void update()
	{
		base.update ();

		mButtonPlay.update ();

		if (mButtonPlay.clicked ()) 
		{
			CGame.inst ().setState(new CLevelState ());
			return;
		}
	}
	
	override public void render()
	{
		base.render ();

		mButtonPlay.render ();
	}
	
	override public void destroy()
	{
		base.destroy ();
		
		mBackground.destroy ();
		mBackground = null;

		mButtonPlay.destroy ();
		mButtonPlay = null;
	}
	
}


