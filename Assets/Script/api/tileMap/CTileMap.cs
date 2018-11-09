using TiledSharp;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using TiledSharp;


public class CTileMap  
{
	// Number of columns.
	private int mMapWidth = 8;
	// Number of rows.
	private int mMapHeight = 8;

	// Size in pixels of the tile.
	private int mTileWidth = 64;
	private int mTileHeight = 64;

	public static CTileMap Instance {get;  private set;}

	// Size of the world in pixels. Calculated automatically when changing map or tile size.
	private int mWorldWidth = 32 * 8;
	private int mWorldHeight = 32 * 8;

	// Matrix of the map. Each element is a CTile.
	private List<List<CTile>> mMap;

	// Number of different tiles.
	private int mNumTiles = 2;

	// Array with the tile sprites (images).
	private Sprite[] mTiles;
     
    // La pantalla tiene 17 columnas x 13 filas de tiles.
    // Mapa con el indice de cada tile.
    public static int[] LEVEL_ANDY_001 = {
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
       1, 1, 1, 1, 1, 1, 1, 1,1,1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
	};

	public static int[] LEVEL_ANDY_002 = 
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
	// Empty tile. Used to be returned when accessing out of the array.
	private CTile mEmptyTile;

	// Info con los items.
	private CItemInfo mItemInfo;

	private const int AIR_INDEX = 71;
	private const int HORIZONTAL_TILE_PRELOAD = 20;
	private const int VERTICAL_TILE_PRELOAD  = 14;

	private int leftLimit;
	private int rightLimit;
	private int topLimit;
	private int bottomLimit;
	
	public CTileMap(string aFileName)
	{
		loadLevelTMX (aFileName);

		// Create the empty tile. Used to be returned when accessing out of the array.
		mEmptyTile = new CTile (0, 0, 0, mTiles [AIR_INDEX], 4);
		mEmptyTile.setVisible (false);
		mEmptyTile.setWalkable (true);

		registerSingleton ();
	}

    private void registerSingleton()
	{
		if (Instance == null) 
		{
			Instance = this;
		}
		else 
		{
			throw new UnityException( "ERROR: Cannot create another instance of singleton class CEnemyManager.");
		}
	}

	private void loadLevelTMX(string aFileName)
	{
		TmxMap tmxMap = new TmxMap(aFileName);

		int mapWidth = tmxMap.Width;
		int mapHeight = tmxMap.Height;
		int tileWidth = tmxMap.TileWidth;
		int tileHeight = tmxMap.TileHeight;
		int scale = 2;

		setMapWidth(tmxMap.Width);
		setMapHeight(tmxMap.Height);
		setTileWidth(tileWidth * scale);
		setTileHeight(tileHeight * scale);

		mMap = new List<List<CTile>>();
		string tileSetPath = tmxMap.Tilesets[0].Image.Source.Replace("Assets/Resources/", "");
		tileSetPath = tileSetPath.Replace(".png", "");
		mTiles = Resources.LoadAll<Sprite> (tileSetPath);
        for (int y = 0; y < mapHeight; y++)
        {
			List<CTile> row = new List<CTile>();

            for (int x = 0; x < mapWidth; x++)
            {
				CTile tile = new CTile((x * mTileWidth), (y * mTileHeight), 0, mTiles[AIR_INDEX], scale);
				tile.setVisible(false);
				row.Add(tile);
            }

			mMap.Add(row);
        }

        for (int i = 0; i < tmxMap.Layers[0].Tiles.Count; i++)
        {
			int y = i / mapWidth;
			int x = i % mapWidth;
            // TODO: PONER UNA SOLA LINEA
            int index = tmxMap.Layers[0].Tiles[i].Gid;         // 0 a 21
            getTile(x, y).setTileIndex(index);

            //getTile (x, y).setWalkable (mWalkable [index]); NO APLICA.
            getTile(x, y).setImage(mTiles[index - 1]);           // 0 a 21
        }
        //Cargo a lo mobs en la pantalla

        CGolemTierra g = new CGolemTierra(CGolemTierra.TYPE_DONT_FALL);
        g.setXY(600, 300);
        CEnemyManager.inst().add(g);

        CSalamandra sal = new CSalamandra(CSalamandra.TYPE_DONT_FALL);
        sal.setXY(500, 300);
        CEnemyManager.inst().add(sal);



    }

    // Construye el mapa. Crear el array y carga el mapa aLevel.
    public void buildLevel(int aLevel)
	{ 
		mCurrentLevel = aLevel;

		int[] m;
		if (aLevel == 1)
			m = LEVEL_ANDY_001;
		else
			m = LEVEL_ANDY_002;

		mMap = new List<List<CTile>> ();

		// Para cada fila..
		for (int y = 0; y < mMapHeight; y++) 
		{
			// Crea un array para la fila vacio.
			mMap.Add (new List<CTile> ());			

			// Llenar la fila.
			for (int x = 0; x < mMapWidth; x++) 
			{
				// Obtener que indice de tile es: 0, 1, ....
				int index = m[y * mMapWidth + x]; 
				// Crear el tile.
				CTile tile = new CTile(x * mTileWidth, y * mTileHeight, index, mTiles[index]);
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
			m = LEVEL_ANDY_001;
		else
			m = LEVEL_ANDY_002;

		// Para cada fila..
		for (int y = 0; y < mMapHeight; y++) 
		{
			// Llenar la fila.
			for (int x = 0; x < mMapWidth; x++) 
			{
				// Obtener que indice de tile es. 0, 1, ....
				int index = m [y * mMapWidth + x];
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
		/*
        ///Cargo el Niveles 
		if (aLevel == 1) 
		{
            //Pongo a los Enemigos
			CScorpion s = new CScorpion (CScorpion.TYPE_DONT_FALL);
			s.setXY (550, 300);
			CEnemyManager.inst ().add (s);

            CGolemTierra g = new CGolemTierra(CGolemTierra.TYPE_DONT_FALL);
            g.setXY(600, 300);
            CEnemyManager.inst().add(g);

            CSalamandra sal = new CSalamandra(CSalamandra.TYPE_DONT_FALL);
            sal.setXY(500, 300);
            CEnemyManager.inst().add(sal);

            CGolemRoca gr = new CGolemRoca(CGolemTierra.TYPE_DONT_FALL);
            gr.setXY(300, 300);
            CEnemyManager.inst().add(gr);

            CBird cb = new CBird(CBird.TYPE_DONT_FALL);
            cb.setXY(450, 300);
            CEnemyManager.inst().add(cb);

            CElementalAgua el = new CElementalAgua(CElementalAgua.TYPE_DONT_FALL);
            el.setXY(150, 300);
            CEnemyManager.inst().add(el);


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
		*/
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
		CVector cameraPos = CCamera.inst() .getPos();
		leftLimit = (int)(cameraPos.x - HORIZONTAL_TILE_PRELOAD / 2 * getTileWidth()) / getTileWidth();
		rightLimit = (int)(cameraPos.x + HORIZONTAL_TILE_PRELOAD / 2 * getTileWidth()) / getTileWidth();
		topLimit = (int)(cameraPos.y - VERTICAL_TILE_PRELOAD / 2 * getTileHeight()) / getTileHeight();
		bottomLimit = (int)(cameraPos.y + VERTICAL_TILE_PRELOAD / 2 * getTileHeight()) / getTileHeight();

		leftLimit = leftLimit < 0 ? 0 : leftLimit;
		rightLimit = rightLimit > getMapWidth() ? getMapWidth() : rightLimit;
		topLimit = topLimit < 0 ? 0 : topLimit;
		bottomLimit = bottomLimit > getMapHeight() ? getMapHeight() : bottomLimit;

		for (int y = topLimit; y < bottomLimit; y++) 
		{
			for (int x = leftLimit; x < rightLimit; x++) 
			{
				if(!mMap[y][x].isVisible()) 
				{
					mMap[y][x].setVisible(true);
				}
				mMap [y] [x].update ();
			}
		}
    }

	public void render()
	{
		for (int y = topLimit; y < bottomLimit; y++) 
		{
			for (int x = leftLimit; x < rightLimit; x++) 
			{
				mMap [y] [x].render ();
			}
		}
	}

	public void destroy()
	{
		for (int y = mMapHeight - 1; y >= 0; y--) 
		{
			for (int x = mMapWidth - 1; x >= 0; x--) 
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
		if (aX < 0 || aX >= mMapWidth || aY < 0 || aY >= mMapHeight) 
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
		if (aX < 0 || aX >= mMapWidth || aY < 0 || aY >= mMapHeight) 
		{
			// Si accedo fuera del mapa retorna el empty tile que es caminable.
			return mEmptyTile;
		} 
		else 
		{
			return mMap [aY] [aX];
		}
	}

	public CItemInfo getItemInfo()
	{
		return mItemInfo;
	}

	public int getMapWidth()
	{
		return mMapWidth;
	}

	// Return the map height in tiles (number of rows of the map).
	public int getMapHeight()
	{
		return mMapHeight;
	}

	public int getTileWidth()
	{
		return mTileWidth;
	}

	public int getTileHeight()
	{
		return mTileHeight;
	}

	public int getWorldWidth()
	{
		return mWorldWidth;
	}

	public int getWorldHeight()
	{
		return mWorldHeight;
	}

	// Calculate the world size. Called each time the size of the map or the tile changes.
	// There is no setters for mWorldWidth and mWorldHeight. There are calculated automatically.
	private void calculateWorldSize()
	{
		mWorldWidth = mMapWidth * mTileWidth;
		mWorldHeight = mMapHeight * mTileHeight;
	}

	// Set the map width in tiles (number of columns of the map).
	public void setMapWidth(int aMapWidth)
	{
		mMapWidth = aMapWidth;
		calculateWorldSize();
	}

	// Set the map height in tiles (number of rows of the map).
	public void setMapHeight(int aMapHeight)
	{
		mMapHeight = aMapHeight;
		calculateWorldSize();
	}

	// Set the tile width in pixels.
	public void setTileWidth(int aTileWidth)
	{
		mTileWidth = aTileWidth;
		calculateWorldSize();
	}

	// Set the tile height in pixels.
	public void setTileHeight(int aTileHeight)
	{
		mTileHeight = aTileHeight;
		calculateWorldSize();
	}

	public void changeTile(CTile tile, CTile.Type type, int imageIndex = -1)
	{
		tile.setTileType(type);

		if(imageIndex < 0)
		{
			imageIndex = tile.getTileIndex() - 1;
		}

		tile.setImage(mTiles[imageIndex]);
	}
}
