using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGen : MonoBehaviour
{
    public int ChunkSize = 100;
    public float scale = 20f;
    public float scale2 = 5f;
    public TileObject[] Tiles;
    public Tilemap tilemap;
    int tempX;
    int tempY;

    public TileBase Grass;
    public TileBase GrassFull;
    public TileBase Dirt;
    public TileBase GrassLF;
    public TileBase GrassL;
    public TileBase GrassRF;
    public TileBase GrassR;
    public TileBase GrassTf;
    public TileBase GrassT;
    public TileBase GrassBF;
    public TileBase GrassB;
    public TileBase GrassLFTF;
    public TileBase GrassLFBF;
    public TileBase GrassRFTF;
    public TileBase GrassRFBF;
    public TileBase GrassLT;
    public TileBase GrassLB;
    public TileBase GrassRT;
    public TileBase GrassRB;
    public TileBase GrassLR;
    public TileBase GrassTB;
    public TileBase GrassTBLR;
    public TileBase GrassTopLeftToBottmRight;
    public TileBase GrassBottomLeftTopRight;
    public TileBase GrassTFB;
    public TileBase GrassBFT;
    public TileBase GrassRFL;
    public TileBase GrassLFR;

    public TileBase GrassTLT;
    public TileBase GrassTRT;
    public TileBase GrassBLB;
    public TileBase GrassBRB;
    public TileBase GrassLTL;
    public TileBase GrassLBL;
    public TileBase GrassRTR;
    public TileBase GrassRBR;

    public TileBase GrassCurveTR;
    public TileBase GrassCurveBR;
    public TileBase GrassCurveLT;
    public TileBase GrassCurveLB;

    public TileBase GrassCornerTL;
    public TileBase GrassCornerTR;
    public TileBase GrassCornerBL;
    public TileBase GrassCornerBR;

    public TileBase WaterFull;
    public TileBase WaterRF;
    public TileBase WaterTF;
    public TileBase WaterLF;
    public TileBase WaterBF;
    public TileBase WaterRFBF;
    public TileBase WaterLFBF;
    public TileBase WaterRFTF;
    public TileBase WaterLFTF;
    public TileBase WaterRT;
    public TileBase WaterRB;
    public TileBase WaterLT;
    public TileBase WaterLB;

    public static MapGen Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Tiles = new TileObject[ChunkSize*ChunkSize];
    }
    private void Update()
    {
        if((PlayerController.Instance.transform.position-tilemap.transform.parent.position).magnitude > ChunkSize / 4)
        {
            GenerateTiles((int)PlayerController.Instance.transform.position.x, (int)PlayerController.Instance.transform.position.y);
        }
    }
    public void GenerateTiles(float x, float y)
    {
        tilemap.ClearAllTiles();
        for (int i = 0; i < Tiles.Length; i++)
        {
            tempX = i % ChunkSize;
            tempY = i / ChunkSize;
            float noise = Perlin.PerlinNoise(i % ChunkSize + x - ChunkSize / 2, i / ChunkSize + y - ChunkSize / 2, ChunkSize, scale, 1233245.123f, 436872.345f);
            noise += Perlin.PerlinNoise(i % ChunkSize + x - ChunkSize / 2, i / ChunkSize + y - ChunkSize / 2, ChunkSize, scale2, 23245.63f, 536962.785f);
            noise = noise / 2f;

            noise = 1f - noise * 2f;
            noise = 1f - Mathf.Abs(noise);
            if (noise > 1f) Debug.Log("bro wat");
            if (noise > 0.95f)
            {
                Tiles[i] = new TileObject(tempX, tempY, TileType.Dirt);
            }
            else if(noise > 0.5f)
            {
                Tiles[i] = new TileObject(tempX, tempY, TileType.Grass);
            }
            else
            {
                Tiles[i] = new TileObject(tempX, tempY, TileType.Water);
            }
        }
        SetTiles2();
        tilemap.RefreshAllTiles();
        AdjustMap(x,y);
    }
    public void SetTiles()
    {
        byte TempByte = 0;
        for (int i = 0; i < Tiles.Length; i++)
        {
            tempX = i % ChunkSize;
            tempY = i / ChunkSize;
            //tilemap.SetTile(new Vector3Int(tempX, tempY, 0), tileDictionary[Tiles[i].tileType]);
            if (Tiles[i].tileType == TileType.Grass)
            {
                TempByte = 0;
                if (tempX == 0) continue;
                if (tempX == ChunkSize - 1) continue;
                if (tempY == 0) continue;
                if (tempY == ChunkSize - 1) continue;

                if (Tiles[i - 1 - ChunkSize].tileType == TileType.Grass) TempByte += 0b10000000;
                if (Tiles[i - ChunkSize].tileType == TileType.Grass) TempByte += 0b01000000;
                if (Tiles[i + 1 - ChunkSize].tileType == TileType.Grass) TempByte += 0b00100000;
                if (Tiles[i - 1].tileType == TileType.Grass) TempByte += 0b00010000;
                if (Tiles[i + 1].tileType == TileType.Grass) TempByte += 0b00001000;
                if (Tiles[i - 1 + ChunkSize].tileType == TileType.Grass) TempByte += 0b000000100;
                if (Tiles[i + ChunkSize].tileType == TileType.Grass) TempByte += 0b000000010;
                if (Tiles[i + 1 + ChunkSize].tileType == TileType.Grass) TempByte += 0b000000001;

                if (TempByte == 0b00000000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), Grass); continue; }

                if (TempByte == 0b11111111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassFull); continue; }

                if (TempByte == 0b01010101) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassTBLR); continue; }
                if (TempByte == 0b01111111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRFTF); continue; }
                if (TempByte == 0b11011111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLFTF); continue; }
                if (TempByte == 0b11111011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRFBF); continue; }
                if (TempByte == 0b11111110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLFBF); continue; }

                if ((TempByte & 0b01011111) == 0b00011111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassTf); continue; }
                if ((TempByte & 0b11111010) == 0b11111000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassBF); continue; }
                if ((TempByte & 0b11011110) == 0b11010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLF); continue; }
                if ((TempByte & 0b01111011) == 0b01101011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRF); continue; }

                if ((TempByte & 0b01011010) == 0b01000000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassB); continue; }
                if ((TempByte & 0b01011010) == 0b00010000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassL); continue; }
                if ((TempByte & 0b01011010) == 0b00001000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassR); continue; }
                if ((TempByte & 0b01011010) == 0b00000010) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassT); continue; }

                if ((TempByte & 0b01011010) == 0b01000010) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassTB); continue; }
                if ((TempByte & 0b01011010) == 0b00011000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLR); continue; }

                if ((TempByte & 0b11011010) == 0b11010000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLB); continue; }
                if ((TempByte & 0b01111010) == 0b01101000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRB); continue; }
                if ((TempByte & 0b01011110) == 0b00010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLT); continue; }
                if ((TempByte & 0b01011011) == 0b00001011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRT); continue; }

                if (TempByte == 0b11011011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassBottomLeftTopRight); continue; }
                if (TempByte == 0b01111110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassTopLeftToBottmRight); continue; }

                if (TempByte == 0b01011111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassTFB); continue; }
                if (TempByte == 0b11011110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLFR); continue; }
                if (TempByte == 0b01111011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRFL); continue; }
                if (TempByte == 0b11111010) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassBFT); continue; }

                if (TempByte == 0b11011010) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassCornerBL); continue; }
                if (TempByte == 0b01111010) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassCornerBR); continue; }
                if (TempByte == 0b01011110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassCornerTL); continue; }
                if (TempByte == 0b01011011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassCornerTL); continue; }

                if ((TempByte & 0b11011010) == 0b11011000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassBRB); continue; }
                if ((TempByte & 0b01111010) == 0b01111000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassBLB); continue; }
                if ((TempByte & 0b01011110) == 0b01010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLBL); continue; }
                if ((TempByte & 0b11011010) == 0b11010010) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLTL); continue; }
                if ((TempByte & 0b01011011) == 0b00011011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassTLT); continue; }
                if ((TempByte & 0b01011110) == 0b00011110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassTRT); continue; }
                if ((TempByte & 0b01011011) == 0b01001011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRBR); continue; }
                if ((TempByte & 0b01111010) == 0b01101010) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRTR); continue; }

                if ((TempByte & 0b11011010) == 0b01010000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassCurveLB); continue; }
                if ((TempByte & 0b01111010) == 0b01001000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassCurveBR); continue; }
                if ((TempByte & 0b01011011) == 0b00001010) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassCurveTR); continue; }
                if ((TempByte & 0b01011110) == 0b00010010) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassCurveLT); continue; }
                //if ( (TempByte & 0b10100101) == 0b10100101) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassFull); continue; }
                /*
                                if ( (TempByte & 0b11111101) == 0b00000101) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassTf); continue; }
                                if ( (TempByte & 0b10111111) == 0b10100000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassBF); continue; }
                                if ( (TempByte & 0b11101111) == 0b10000100) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLF); continue; }
                                if ( (TempByte & 0b11110111) == 0b00100001) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRF); continue; }

                                if ( TempByte == 0b01000000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassB); continue; }
                                if ( TempByte == 0b00010000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassL); continue; }
                                if ( TempByte == 0b00001000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassR); continue; }
                                if ( TempByte == 0b00000010) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassT); continue; }

                                if (TempByte == 0b01000010) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassTB); continue; }
                                if (TempByte == 0b00011000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLR); continue; }

                                if ( (TempByte & 0b01111111) == 0b01010000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLB); continue; }
                                if ((TempByte & 0b11011111) == 0b01001000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRB); continue; }
                                if ((TempByte & 0b11111110) == 0b00001001) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRT); continue; }
                                if ((TempByte & 0b11111011) == 0b00010010) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLT); continue; }*/
            }
            else
            {
                tilemap.SetTile(new Vector3Int(tempX, tempY, 0), Dirt);
            }

        }
    }
    public void SetTiles2()
    {
        byte TempByte = 0;
        for (int i = 0; i < Tiles.Length; i++)
        {
            tempX = i % ChunkSize;
            tempY = i / ChunkSize;
            //tilemap.SetTile(new Vector3Int(tempX, tempY, 0), tileDictionary[Tiles[i].tileType]);
            /*if (Tiles[i].tileType == TileType.Water)
            {

                TempByte = 0;
                if (tempX == 0) continue;
                if (tempX == ChunkSize - 1) continue;
                if (tempY == 0) continue;
                if (tempY == ChunkSize - 1) continue;

                if (Tiles[i - 1 - ChunkSize].tileType == TileType.Water) TempByte += 0b10000000;
                if (Tiles[i - ChunkSize].tileType == TileType.Water) TempByte += 0b01000000;
                if (Tiles[i + 1 - ChunkSize].tileType == TileType.Water) TempByte += 0b00100000;
                if (Tiles[i - 1].tileType == TileType.Water) TempByte += 0b00010000;
                if (Tiles[i + 1].tileType == TileType.Water) TempByte += 0b00001000;
                if (Tiles[i - 1 + ChunkSize].tileType == TileType.Water) TempByte += 0b000000100;
                if (Tiles[i + ChunkSize].tileType == TileType.Water) TempByte += 0b000000010;
                if (Tiles[i + 1 + ChunkSize].tileType == TileType.Water) TempByte += 0b000000001;

                if (TempByte == 0b11111111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterFull); continue; }

                if ((TempByte & 0b01111010) == 0b01101000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRB); continue; }
                if ((TempByte & 0b01111001) == 0b01101000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRB); continue; }
                if ((TempByte & 0b11101010) == 0b01101000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRB); continue; }

                if ((TempByte & 0b01011011) == 0b00001011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRT); continue; }
                if ((TempByte & 0b00111011) == 0b00001011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRT); continue; }
                if ((TempByte & 0b01001111) == 0b00001011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRT); continue; }

                if ((TempByte & 0b11011010) == 0b11010000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLB); continue; }
                if ((TempByte & 0b11110010) == 0b11010000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLB); continue; }
                if ((TempByte & 0b11011100) == 0b11010000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLB); continue; }

                if ((TempByte & 0b01011110) == 0b00010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLT); continue; }
                if ((TempByte & 0b10011110) == 0b00010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLT); continue; }
                if ((TempByte & 0b01010111) == 0b00010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLT); continue; }

                if ((TempByte & 0b11111101) == 0b11111000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterBF); continue; }
                if ((TempByte & 0b11110111) == 0b11010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLF); continue; }
                if ((TempByte & 0b11101111) == 0b01101011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRF); continue; }
                if ((TempByte & 0b10111111) == 0b00011111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterTF); continue; }

                if ((TempByte & 0b11111010) == 0b11111000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterBF); continue; }
                if ((TempByte & 0b11011110) == 0b11010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLF); continue; }
                if ((TempByte & 0b01111011) == 0b01101011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRF); continue; }
                if ((TempByte & 0b01011111) == 0b00011111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterTF); continue; }

                if (TempByte == 0b11011111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLFTF); continue; }
                if (TempByte == 0b01111111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRFTF); continue; }
                if (TempByte == 0b11111011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRFBF); continue; }
                if (TempByte == 0b11111110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLFBF); continue; }

                //if (TempByte == 0b11011011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassBottomLeftTopRight); continue; }
                //if (TempByte == 0b01111110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassTopLeftToBottmRight); continue; }

                tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassFull);
            }*/
            if (Tiles[i].tileType == TileType.Water)
            {
                 tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterFull);
            }
            else if (Tiles[i].tileType == TileType.Grass)
            {
                TempByte = 0;
                if (tempX == 0) continue;
                if (tempX == ChunkSize - 1) continue;
                if (tempY == 0) continue;
                if (tempY == ChunkSize - 1) continue;

                if (Tiles[i - 1 - ChunkSize].tileType == TileType.Grass) TempByte += 0b10000000;
                if (Tiles[i - ChunkSize].tileType == TileType.Grass) TempByte += 0b01000000;
                if (Tiles[i + 1 - ChunkSize].tileType == TileType.Grass) TempByte += 0b00100000;
                if (Tiles[i - 1].tileType == TileType.Grass) TempByte += 0b00010000;
                if (Tiles[i + 1].tileType == TileType.Grass) TempByte += 0b00001000;
                if (Tiles[i - 1 + ChunkSize].tileType == TileType.Grass) TempByte += 0b000000100;
                if (Tiles[i + ChunkSize].tileType == TileType.Grass) TempByte += 0b000000010;
                if (Tiles[i + 1 + ChunkSize].tileType == TileType.Grass) TempByte += 0b000000001;
                bool water = false;
                if (Tiles[i - 1 - ChunkSize].tileType == TileType.Water) water = true;
                if (Tiles[i - ChunkSize].tileType == TileType.Water) water = true;
                if (Tiles[i + 1 - ChunkSize].tileType == TileType.Water) water = true;
                if (Tiles[i - 1].tileType == TileType.Water) water = true;
                if (Tiles[i + 1].tileType == TileType.Water) water = true;
                if (Tiles[i - 1 + ChunkSize].tileType == TileType.Water) water = true;
                if (Tiles[i + ChunkSize].tileType == TileType.Water) water = true;
                if (Tiles[i + 1 + ChunkSize].tileType == TileType.Water) water = true;

                if (water)
                {
                    if (TempByte == 0b11111111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassFull); continue; }

                    if ((TempByte & 0b01111010) == 0b01101000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRB); continue; }
                    if ((TempByte & 0b01111001) == 0b01101000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRB); continue; }
                    if ((TempByte & 0b11101010) == 0b01101000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRB); continue; }

                    if ((TempByte & 0b01011011) == 0b00001011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRT); continue; }
                    if ((TempByte & 0b00111011) == 0b00001011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRT); continue; }
                    if ((TempByte & 0b01001111) == 0b00001011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRT); continue; }

                    if ((TempByte & 0b11011010) == 0b11010000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLB); continue; }
                    if ((TempByte & 0b11110010) == 0b11010000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLB); continue; }
                    if ((TempByte & 0b11011100) == 0b11010000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLB); continue; }

                    if ((TempByte & 0b01011110) == 0b00010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLT); continue; }
                    if ((TempByte & 0b10011110) == 0b00010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLT); continue; }
                    if ((TempByte & 0b01010111) == 0b00010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLT); continue; }

                    if ((TempByte & 0b11111101) == 0b11111000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterBF); continue; }
                    if ((TempByte & 0b11110111) == 0b11010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLF); continue; }
                    if ((TempByte & 0b11101111) == 0b01101011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRF); continue; }
                    if ((TempByte & 0b10111111) == 0b00011111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterTF); continue; }

                    if ((TempByte & 0b11111010) == 0b11111000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterBF); continue; }
                    if ((TempByte & 0b11011110) == 0b11010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLF); continue; }
                    if ((TempByte & 0b01111011) == 0b01101011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRF); continue; }
                    if ((TempByte & 0b01011111) == 0b00011111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterTF); continue; }

                    if (TempByte == 0b11011111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLFTF); continue; }
                    if (TempByte == 0b01111111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRFTF); continue; }
                    if (TempByte == 0b11111011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterRFBF); continue; }
                    if (TempByte == 0b11111110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterLFBF); continue; }

                    if (TempByte == 0b11011011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassBottomLeftTopRight); continue; }
                    if (TempByte == 0b01111110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassTopLeftToBottmRight); continue; }

                    tilemap.SetTile(new Vector3Int(tempX, tempY, 0), WaterFull);
                }
                else
                {
                    if (TempByte == 0b11111111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassFull); continue; }

                    if ((TempByte & 0b01111010) == 0b01101000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRB); continue; }
                    if ((TempByte & 0b01111001) == 0b01101000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRB); continue; }
                    if ((TempByte & 0b11101010) == 0b01101000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRB); continue; }

                    if ((TempByte & 0b01011011) == 0b00001011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRT); continue; }
                    if ((TempByte & 0b00111011) == 0b00001011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRT); continue; }
                    if ((TempByte & 0b01001111) == 0b00001011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRT); continue; }

                    if ((TempByte & 0b11011010) == 0b11010000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLB); continue; }
                    if ((TempByte & 0b11110010) == 0b11010000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLB); continue; }
                    if ((TempByte & 0b11011100) == 0b11010000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLB); continue; }

                    if ((TempByte & 0b01011110) == 0b00010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLT); continue; }
                    if ((TempByte & 0b10011110) == 0b00010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLT); continue; }
                    if ((TempByte & 0b01010111) == 0b00010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLT); continue; }

                    if ((TempByte & 0b11111101) == 0b11111000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassBF); continue; }
                    if ((TempByte & 0b11110111) == 0b11010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLF); continue; }
                    if ((TempByte & 0b11101111) == 0b01101011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRF); continue; }
                    if ((TempByte & 0b10111111) == 0b00011111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassTf); continue; }

                    if ((TempByte & 0b11111010) == 0b11111000) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassBF); continue; }
                    if ((TempByte & 0b11011110) == 0b11010110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLF); continue; }
                    if ((TempByte & 0b01111011) == 0b01101011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRF); continue; }
                    if ((TempByte & 0b01011111) == 0b00011111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassTf); continue; }

                    if (TempByte == 0b11011111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLFTF); continue; }
                    if (TempByte == 0b01111111) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRFTF); continue; }
                    if (TempByte == 0b11111011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassRFBF); continue; }
                    if (TempByte == 0b11111110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassLFBF); continue; }

                    if (TempByte == 0b11011011) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassBottomLeftTopRight); continue; }
                    if (TempByte == 0b01111110) { tilemap.SetTile(new Vector3Int(tempX, tempY, 0), GrassTopLeftToBottmRight); continue; }

                    tilemap.SetTile(new Vector3Int(tempX, tempY, 0), Dirt);
                }
            }
            else
            {
                tilemap.SetTile(new Vector3Int(tempX, tempY, 0), Dirt);
            }

        }
    }
    public void AdjustMap(float x, float y)
    {
        tilemap.transform.parent.position = new Vector3(x, y, 0);
    }
}
public class TileObject
{
    public int x;
    public int y;
    public TileType tileType;

    public TileObject(int _x, int _y, TileType _tileType)
    {
        x = _x;
        y = _y;
        tileType = _tileType;
    }
}
public enum TileType
{
    Grass,
    Dirt,
    Water
}