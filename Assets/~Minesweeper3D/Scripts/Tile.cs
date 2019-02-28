using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace Minesweeper3D
{
    public class Tile : MonoBehaviour
    {
        public int x, y, z;
        public bool isMine = false;
        public bool isRevealed = false;
        public GameObject minePrefab;
        public GameObject textPrefab;
        [Range(0, 1)]
        public float mineChance = 0.15f;

        private Animator anim;
        private GameObject mine;
        private GameObject text;
        private Collider col;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            col = GetComponent<Collider>();
        }


        void Start()
        {
            isMine = Random.value < mineChance;

            if (isMine)
            {
                mine = Instantiate(minePrefab, transform);
                mine.SetActive(false);
            }
            else
            {
                text = Instantiate(textPrefab, transform);
                text.SetActive(false);
            }
        }

        private void OnMouseDown()
        {
            Reveal(10);
        }

        public void Reveal(int adjacentMines, int mineState = 0)
        {
            isRevealed = true;
            anim.SetTrigger("Reveal");
            col.enabled = false;
            if (isMine)
            {
                mine.SetActive(true);
            }
            else
            {
                text.SetActive(true);
                text.GetComponent<TextMeshPro>().text = adjacentMines.ToString();
            }
        }
    }

}