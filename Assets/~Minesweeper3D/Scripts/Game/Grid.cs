using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minesweeper3D
{
    public class Grid : MonoBehaviour
    {
        public GameObject tilePrefab;
        public int width = 10, height = 10, depth = 10;
        public float spacing = 0.155f;

        private Tile[,,] tiles;

        Tile SpawnTile(Vector3 pos)
        {
            GameObject clone = Instantiate(tilePrefab);
            clone.transform.position = pos;
            Tile currentTile = clone.GetComponent<Tile>();
            return currentTile;
        }

        void Start()
        {
            GenerateTiles();
        }

        void GenerateTiles()
        {
            tiles = new Tile[width, height, depth];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        Vector3 halfSize = new Vector3(width * 0.5f, height * 0.5f, depth * 0.5f);
                        Vector3 pos = new Vector3(x - halfSize.x, y - halfSize.y, z - halfSize.z);
                        Vector3 offset = new Vector3(.5f, .5f, 0.5f);
                        pos += offset;
                        pos *= spacing;
                        Tile tile = SpawnTile(pos);
                        tile.transform.SetParent(transform);
                        tile.x = x;
                        tile.y = y;
                        tiles[x, y, z] = tile;
                    }
                }
            }
        }

        public int GetAdjacentMineCount(Tile tile)
        {
            int count = 0;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        int desiredX = tile.x + x;
                        int desiredY = tile.y + y;
                        int desiredZ = tile.z + z;
                        if (desiredX < 0 || desiredX >= width ||
                            desiredY < 0 || desiredY >= height ||
                            desiredZ < 0 || desiredZ >= depth)
                        {
                            continue;
                        }
                        Tile currentTile = tiles[desiredX, desiredY, desiredZ];
                        if (currentTile.isMine)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        void SelectATile()
        {
            

        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectATile();
            }
        }

        void FFuncover(int x, int y, int z, bool[,,] visited)
        {
            if (x >= 0 && y >= 0 && z >= 0 &&
                x < width && y < height && z < depth)
            {
                if (visited[x, y, z])
                    return;
                Tile tile = tiles[x, y, z];
                int adjacentMines = GetAdjacentMineCount(tile);
                tile.Reveal(adjacentMines);

                if (adjacentMines == 0)
                {
                    visited[x, y, z] = true;
                    FFuncover(x - 1, y, z, visited);
                    FFuncover(x + 1, y, z, visited);
                    FFuncover(x, y - 1, z, visited);
                    FFuncover(x, y + 1, z, visited);
                    FFuncover(x, y, z - 1, visited);
                    FFuncover(x, y, z + 1, visited);

                    FFuncover(x + 1, y + 1, z + 1, visited);
                    FFuncover(x - 1, y - 1, z - 1, visited);
                }
            }
        }

        void UncoverMines(int mineState = 0)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        Tile tile = tiles[x, y, z];
                        if (tile.isMine)
                        {
                            int adjacentMines = GetAdjacentMineCount(tile);
                            tile.Reveal(adjacentMines);
                        }
                    }

                }
            }

        }
        bool NoMoreEmptyTiles()
        {
            int emptyTileCount = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        Tile tile = tiles[x, y, z];
                        if (!tile.isRevealed &&
                           !tile.isMine)
                        {
                            emptyTileCount += 1;
                        }
                    }

                }
            }
            return emptyTileCount == 0;
        }
        void SelectTile(Tile selected)
        {
            int adjacentMines = GetAdjacentMineCount(selected);
            selected.Reveal(adjacentMines);

            if (selected.isMine)
            {
                UncoverMines();
                print("YOU LOSE");
                // Lose
            }
            else if (adjacentMines == 0)
            {
                int x = selected.x;
                int y = selected.y;
                int z = selected.z;
                FFuncover(x, y, z, new bool[width, height, depth]);
            }
            if (NoMoreEmptyTiles())
            {
                UncoverMines(1);
                print("YOUR WINNER");
                // Win
            }
        }

    }
}