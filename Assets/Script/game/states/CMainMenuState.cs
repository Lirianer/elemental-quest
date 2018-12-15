using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class CMainMenuState : CGameState
{
	private CSprite mBackground;

	private CButtonSprite mButtonPlay;
    private CButtonSprite exitButton;



    private CText logo;

	public CMainMenuState()
	{

	}
	
	override public void init()
	{
		base.init ();

		mBackground = new CSprite ();
		mBackground.setImage (Resources.Load<Sprite> ("Sprites/ui/menu/fondo"));
		mBackground.setXY (0, 0);
		mBackground.setSortingLayerName("Background");
		mBackground.setName ("background");



		mButtonPlay = new CButtonSprite ("Play");
        mButtonPlay.setFrames(Resources.LoadAll<Sprite>("Sprites/ui/menu/button"));
        mButtonPlay.setName("PlayImage");
        mButtonPlay.setXY (CGameConstants.SCREEN_WIDTH / 2 +700 , CGameConstants.SCREEN_HEIGHT / 2 - 150 );
        mButtonPlay.setScale(500);

        


        exitButton = new CButtonSprite("Exit");
        exitButton.setFrames(Resources.LoadAll<Sprite>("Sprites/ui/menu/button"));
        exitButton.setName("ExitImage");
        exitButton.setXY(mButtonPlay.getX(), CGameConstants.SCREEN_HEIGHT / 3 * 2 -10);

  

        logo = new CText(" ");
        logo.setXY(CGameConstants.SCREEN_WIDTH / 2, CGameConstants.SCREEN_HEIGHT / 4 * 1);
        logo.setFontSize(1000f);
        logo.setPivot(0.5f, 0.5f);
        logo.setWidth(CGameConstants.SCREEN_WIDTH);
        logo.setAlignment(TMPro.TextAlignmentOptions.Center);
         

    }
	
	override public void update()
	{
		base.update ();

		mButtonPlay.update ();

        exitButton.update();
        logo.update();

        if (mButtonPlay.isMouseOver())

        {
            
                
            

        }




        if (mButtonPlay.clicked ()) 
		{
			CGame.inst ().setState(new CPlatformGameState());


			return;
		}

        if (exitButton.clicked())
        {
            Application.Quit();
        }
	}
	
	override public void render()
	{
		base.render ();

        exitButton.render();
		mButtonPlay.render ();
        logo.render();
	}
	
	override public void destroy()
	{
		base.destroy ();
		
		mBackground.destroy ();
		mBackground = null;

		mButtonPlay.destroy ();
		mButtonPlay = null;

        exitButton.destroy();
        exitButton = null;

        logo.destroy();
        logo = null;
	}
	
}


