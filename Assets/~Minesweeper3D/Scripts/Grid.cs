using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper3D
{
    public class Grid : MonoBehaviour
    {
        public GameObject tilePrefab;
        public int width = 10, height = 10, depth = 10;
        public float spacing;

        private Tile[,,] tiles;

        // Use this for initialization
        void Start()
        {
            GenerateTiles();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void GenerateTiles()
        {
            tiles = new Tile[width, height, depth];
            Vector3 halfSize = new Vector3(width * .5f, height * .5f, depth * .5f);
            Vector3 offset = new Vector3(.5f, .5f, .5f);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        Vector3 position = new Vector3(x - halfSize.x,
                                                       y - halfSize.y,
                                                       z - halfSize.z);
                        position *= spacing;
                        Tile newTile = SpawnTile(position);
                        newTile.x = x;
                        newTile.y = y;
                        newTile.z = z;
                        tiles[x, y, z] = newTile;
                    }
                }
            }
            
        }
        Tile SpawnTile (Vector3 position)
            {
                GameObject clone = Instantiate(tilePrefab);
                clone.transform.position = position;
                return clone.GetComponent<Tile>();
            }
    }
}