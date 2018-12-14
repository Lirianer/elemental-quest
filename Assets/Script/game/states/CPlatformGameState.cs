using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;


public class CPlatformGameState : CGameState
{
	private CTileMap mMap;

	private CEnemyManager mEnemyManager;
	private CItemManager mItemManager;
	private CBulletManager mBulletManager;
	private CParticleManager mParticleManger;

	private CSprite mBackground;

    private CAndy mAndy;

	public CPlatformGameState()
	{
	}

	override public void init()
	{
		base.init ();

		//CGame.inst ().setPlayer (mPlayer);

		mEnemyManager = new CEnemyManager ();
		mItemManager = new CItemManager ();
		mBulletManager = new CBulletManager ();
		mParticleManger = new CParticleManager ();


		// El mapa crea enemigos. Crear los managers antes que el mapa.
		mMap = new CTileMap ("Assets/Resources/Map/Map.tmx");
		CGame.inst ().setMap (mMap);

		mAndy = new CAndy ();
        mAndy.muestro = mMap;
		mAndy.setXY (400, 400);
		mAndy.setCheckpoint(400, 400);
		CGame.inst().setPlayer(mAndy);

		mBackground = new CSprite();
		mBackground.setImage(Resources.Load<Sprite>("Sprites/background/background"));
		mBackground.setName("Background");
		mBackground.setRegistration(CSprite.REG_CENTER);
		mBackground.setParent(CCamera.inst().getTransform());

		CCamera.inst().setSize(mMap.getTileHeight() * 8);
		CCamera.inst().setBounds(0, 0, mMap.getMapWidth() * mMap.getTileWidth(), mMap.getMapHeight() * mMap.getTileHeight());
    }

    override public void update()
	{
		base.update (); 

		mMap.update ();

		mAndy.update ();

		CCamera.inst().lookAt(mAndy);

        mEnemyManager.update ();
		mItemManager.update ();
		mBulletManager.update ();
		mParticleManger.update ();
	}

	override public void render()
	{
		base.render ();
		
		mMap.render ();

		mAndy.render ();

		mEnemyManager.render ();
		mItemManager.render ();
		mBulletManager.render ();
		mParticleManger.render ();
    }

	override public void destroy()
	{
		base.destroy ();

		mMap.destroy ();
		mMap = null;

		mAndy.destroy ();
		mAndy = null;

		mEnemyManager.destroy ();
		mEnemyManager = null;

		mItemManager.destroy ();
		mItemManager = null;

		mBulletManager.destroy ();
		mBulletManager = null;

		mParticleManger.destroy ();
		mParticleManger = null;

		mBackground.destroy();
		mBackground = null;
    }
}
