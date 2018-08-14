﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CTileMap  
{
	// Cantidad de columnas.
	public const int MAP_WIDTH = 17;
	// Cantidad de filas.
	public const int MAP_HEIGHT = 13;

    public CAndy mAndy;
    

    // La imagen es de 48x48 pixeles mide cada tile.
    public const int TILE_WIDTH = 64*2;
	public const int TILE_HEIGHT = 64*2;

	// Ancho y alto del nivel en pixeles.
	public const int WORLD_WIDTH = MAP_WIDTH * TILE_WIDTH;
	public const int WORLD_HEIGHT = MAP_HEIGHT * TILE_HEIGHT;

	private List<List<CTile>> mMap;

	// Cantidad de tiles que hay.
	private const int NUM_TILES = 6;

	// Array con los sprites de los tiles.
	private Sprite[] mTiles;

    //Aca tengo para controlar  si  tengo el poder tierra
    //public bool estatierra = false;

    //Contador de Tierra para que no sea mas de 2 
    private int contadorTierra;





    // La pantalla tiene 17 columnas x 13 filas de tiles.
    // Mapa con el indice de cada tile.
    public static int[] LEVEL_001 = {
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 ,
		1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1	,
		1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1		,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1		,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1		,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1		,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0		,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0		,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
	};

	public static int[] LEVEL_002 = 
	{
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
	};


	private int mCurrentLevel;

	// Tile auxiliar, caminable, que se retorna cuando accedemos afuera del mapa.
	private CTile mEmptyTile;


	public CTileMap()
	{
		mTiles = new Sprite [NUM_TILES];
		mTiles [0] = Resources.Load<Sprite> ("Sprites/tiles/tile000");
		mTiles [1] = Resources.Load<Sprite> ("Sprites/tiles/Piedra");
		mTiles [2] = Resources.Load<Sprite> ("Sprites/tiles/tile002");
		mTiles [3] = Resources.Load<Sprite> ("Sprites/tiles/Tierra");
		mTiles [4] = Resources.Load<Sprite> ("Sprites/tiles/tile004");
		mTiles [5] = Resources.Load<Sprite> ("Sprites/tiles/tile005");

		// TODO: CARGAR TODO JUNTO CON LOADALL.

		buildLevel (1);

		mEmptyTile = new CTile (0, 0, 0, mTiles [0]);
		mEmptyTile.setVisible (false);
		mEmptyTile.setWalkable (true);
	}

	// Construye el mapa. Crear el array y carga el mapa aLevel.
	public void buildLevel(int aLevel)
	{ 
		mCurrentLevel = aLevel;

		int[] m;
		if (aLevel == 1)
			m = LEVEL_001;
		else
			m = LEVEL_002;

		mMap = new List<List<CTile>> ();

		// Para cada fila..
		for (int y = 0; y < MAP_HEIGHT; y++) 
		{
			// Crea un array para la fila vacio.
			mMap.Add (new List<CTile> ());			

			// Llenar la fila.
			for (int x = 0; x < MAP_WIDTH; x++) 
			{
				// Obtener que indice de tile es: 0, 1, ....
				int index = m[y * MAP_WIDTH + x]; 
				// Crear el tile.
				CTile tile = new CTile(x * TILE_WIDTH, y * TILE_HEIGHT, index, mTiles[index]);
				// Agregar el tile a la fila.
				mMap [y].Add (tile);
			}
		}

		loadEnemies (aLevel);
	}

	// Carga el mapa en el array ya creado. No crea el array.
	public void loadLevel(int aLevel)
	{ 
		mCurrentLevel = aLevel;

		int[] m;
		if (aLevel == 1)
			m = LEVEL_001;
		else
			m = LEVEL_002;

		// Para cada fila..
		for (int y = 0; y < MAP_HEIGHT; y++) 
		{
			// Llenar la fila.
			for (int x = 0; x < MAP_WIDTH; x++) 
			{
				// Obtener que indice de tile es. 0, 1, ....
				int index = m [y * MAP_WIDTH + x];
				// Cambiar el tile.
				CTile tile = getTile(x, y);
				tile.setTileIndex (index);
				tile.setImage (mTiles[index]);
			}
		}

		loadEnemies (aLevel);
	}

	private void loadEnemies(int aLevel)
	{

        ///Cargo el Niveles 
		if (aLevel == 1) 
		{
			CScorpion s = new CScorpion (CScorpion.TYPE_DONT_FALL);
			s.setXY (550, 300);
			CEnemyManager.inst ().add (s);

			CItem item = new CItem (CItem.TYPE_DONT_FALL);
			item.setXY (700, 300);
			CItemManager.inst ().add (item);

			CCoin coin = new CCoin (CCoin.TYPE_FALL);
			coin.setXY (800, 200);
			CItemManager.inst ().add (coin);
		} 
		else if (aLevel == 2) 
		{
			CScorpion s = new CScorpion (CScorpion.TYPE_FALL);
			s.setXY (700, 300);
			CEnemyManager.inst ().add (s);

			CItem item = new CItem (CItem.TYPE_DONT_FALL);
			item.setXY (400, 300);
			CItemManager.inst ().add (item);

			CCoin coin = new CCoin (CCoin.TYPE_FALL);
			coin.setXY (550, 200);
			CItemManager.inst ().add (coin);
		}
	}


	// aDIr es por donde nos fuimos: CGameConstants.UP,...
	public void changeRoom(int aDir)
	{
		CEnemyManager.inst ().clean ();

		if (mCurrentLevel == 1) 
		{
			if (aDir == CGameConstants.D) 
			{
				loadLevel (2);
			}
		} 
		else if (mCurrentLevel == 2) 
		{
			if (aDir == CGameConstants.A) 
			{
				loadLevel (1);
			}
		}
	}

	public void update()
	{

        

		for (int y = 0; y < MAP_HEIGHT; y++) 
		{
			for (int x = 0; x < MAP_WIDTH; x++) 
			{
				mMap [y] [x].update ();
			}
		}
        
             

        

    }



   public void MuestroTierra()
    {
         

          
        int col = (int)(CMouse.getX() / TILE_WIDTH);
        int row = (int)(CMouse.getY() / TILE_HEIGHT);
        CTile tile = getTile(col, row);

        //Aca TENGO LO DEL EL CLICK PARA LOS PODERES

        if (tile != null)
        {
            if (CMouse.firstPress())
            {
                int index = tile.getTileIndex();
                if (index == 0)
                {
                    if (contadorTierra < 4)
                    {
                        contadorTierra = contadorTierra + 1;
                        tile.setTileIndex(3);
                        tile.setImage(mTiles[3]);
                    }
                    else
                    {
                        Debug.Log("YA HAY 4 TIERRA");
                    }
                }
                else if (index == 3)
                {
                    tile.setTileIndex(0);
                    tile.setImage(mTiles[0]);
                    contadorTierra = contadorTierra - 1;                }
            }

          





        }

    }


	public void render()
	{
		for (int y = 0; y < MAP_HEIGHT; y++) 
		{
			for (int x = 0; x < MAP_WIDTH; x++) 
			{
				mMap [y] [x].render ();
			}
		}
	}

	public void destroy()
	{
		for (int y = MAP_HEIGHT - 1; y >= 0; y--) 
		{
			for (int x = MAP_WIDTH - 1; x >= 0; x--) 
			{
				mMap [y] [x].destroy ();
				mMap [y] [x] = null;
			}
			mMap.RemoveAt (y);
		}

		mMap = null;
	}

	// Parametros: aX es la columna. aY es la fila.
	public int getTileIndex(int aX, int aY)
	{
		if (aX < 0 || aX >= MAP_WIDTH || aY < 0 || aY >= MAP_HEIGHT) 
		{
			return 0;
		} 
		else 
		{
			return mMap [aY] [aX].getTileIndex ();
		}
	}

	public CTile getTile(int aX, int aY)
	{
		if (aX < 0 || aX >= MAP_WIDTH || aY < 0 || aY >= MAP_HEIGHT) 
		{
			// Si accedo fuera del mapa retorna el empty tile que es caminable.
			return mEmptyTile;
		} 
		else 
		{
			return mMap [aY] [aX];
		}
	}
}
