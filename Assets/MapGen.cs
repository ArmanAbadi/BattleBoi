using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGen : MonoBehaviour
{
    public int ChunkSize = 100;
    public float scale = 20f;
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


    // Start is called before the first frame update
    void Start()
    {
        Tiles = new TileObject[ChunkSize*ChunkSize];
        
        GenerateTiles();
    }
    public void GenerateTiles()
    {
        tilemap.ClearAllTiles();
        for (int i = 0; i < Tiles.Length; i++)
        {
            tempX = i % ChunkSize;
            tempY = i / ChunkSize;

            if (Perlin.PerlinNoise(i % ChunkSize, i / ChunkSize , ChunkSize, scale) > 0.5f)
            {
                Tiles[i] = new TileObject(tempX, tempY, TileType.Dirt);
            }
            else
            {
                Tiles[i] = new TileObject(tempX, tempY, TileType.Grass);
            }
        }
        SetTiles();
        tilemap.RefreshAllTiles();
    }
    public void SetTiles2()
    {

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
    Dirt
}