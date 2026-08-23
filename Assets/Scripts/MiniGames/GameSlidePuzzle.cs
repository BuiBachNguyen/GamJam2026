using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSlidePuzzle : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RectTransform gameTransform;
    [SerializeField] private RawImage piecePrefab;

    [Header("Puzzle")]
    [SerializeField] private Texture puzzleTexture;
    [SerializeField] private int size = 4;
    [SerializeField] private float boardSize = 800f;
    [SerializeField] private float gap = 0f;

    private List<RawImage> pieces = new();

    // Index của vị trí đang trống
    private int emptyLocation;

    private bool shuffling;
    private bool solved;

    public static event Action IsSolved;


    private void Start()
    {
        //shuffling = true;
        CreateGamePieces();

        //StartCoroutine(WaitShuffle(Time.deltaTime));
    }


    private void Update()
    {
        // Chỉ check solved khi không đang shuffle
        if (!shuffling && !solved && CheckCompletion())
        {
            solved = true;

            IsSolved?.Invoke();

            //if (gameObject.activeInHierarchy)
            //    StartCoroutine(WaitShuffle(0.5f));
        }

        // Input
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(-size);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(size);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(1);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.gameObject.SetActive(false);
            SystemControl.instance.removeAction();
        }
    }


    // =========================================================
    // CREATE PUZZLE
    // =========================================================

    private void CreateGamePieces()
    {
        pieces.Clear();

        float tileSize = boardSize / size;

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                RawImage piece = Instantiate(piecePrefab, gameTransform);

                int index = row * size + col;

                piece.name = index.ToString();

                pieces.Add(piece);

                // -------------------------------------------------
                // UI SIZE
                // -------------------------------------------------

                RectTransform rect = piece.rectTransform;

                rect.sizeDelta = new Vector2(
                    tileSize - gap,
                    tileSize - gap
                );


                // -------------------------------------------------
                // UI POSITION
                // -------------------------------------------------

                rect.anchoredPosition = new Vector2(
                    -boardSize / 2f
                    + tileSize * col
                    + tileSize / 2f,

                    boardSize / 2f
                    - tileSize * row
                    - tileSize / 2f
                );


                // -------------------------------------------------
                // TEXTURE
                // -------------------------------------------------

                piece.texture = puzzleTexture;


                // -------------------------------------------------
                // UV
                // -------------------------------------------------

                float uvSize = 1f / size;

                float uvX = col * uvSize;

                // RawImage UV có gốc ở bottom-left
                float uvY = 1f - (row + 1) * uvSize;

                float uvGap = gap / boardSize / 2f;

                piece.uvRect = new Rect(
                    uvX + uvGap,
                    uvY + uvGap,
                    uvSize - uvGap * 2f,
                    uvSize - uvGap * 2f
                );


                // -------------------------------------------------
                // EMPTY TILE
                // -------------------------------------------------

                if (row == size - 1 && col == size - 1)
                {
                    emptyLocation = index;

                    piece.gameObject.SetActive(false);
                }
                else
                {
                    piece.gameObject.SetActive(true);
                }
            }
        }
    }


    // =========================================================
    // MOVE
    // =========================================================

    private void Move(int offset)
    {
        if (shuffling)
            return;

        for (int i = 0; i < pieces.Count; i++)
        {
            if (SwapIfValid(i, offset, GetColCheck(offset)))
            {
                solved = false;
                return;
            }
        }
    }


    // =========================================================
    // COLUMN CHECK
    // =========================================================

    private int GetColCheck(int offset)
    {
        if (offset == -1)
            return 0;

        if (offset == 1)
            return size - 1;

        return size;
    }


    // =========================================================
    // SWAP
    // =========================================================

    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        // Prevent horizontal wrapping
        if ((i % size) == colCheck)
            return false;


        int targetIndex = i + offset;


        // Target must be the empty slot
        if (targetIndex != emptyLocation)
            return false;


        // -------------------------------------------------
        // CURRENT PIECE
        // -------------------------------------------------

        RawImage movingPiece = pieces[i];

        // This is the invisible empty RawImage
        RawImage emptyPiece = pieces[targetIndex];


        // -------------------------------------------------
        // SWAP LIST
        // -------------------------------------------------

        (pieces[i], pieces[targetIndex]) =
            (pieces[targetIndex], pieces[i]);


        // -------------------------------------------------
        // SWAP UI POSITION
        // -------------------------------------------------

        Vector2 tempPosition =
            movingPiece.rectTransform.anchoredPosition;

        movingPiece.rectTransform.anchoredPosition =
            emptyPiece.rectTransform.anchoredPosition;

        emptyPiece.rectTransform.anchoredPosition =
            tempPosition;


        // -------------------------------------------------
        // SWAP VISIBILITY
        // -------------------------------------------------

        // movingPiece đi vào ô trống
        movingPiece.gameObject.SetActive(true);

        // emptyPiece trở thành ô trống mới
        emptyPiece.gameObject.SetActive(false);


        // -------------------------------------------------
        // UPDATE EMPTY LOCATION
        // -------------------------------------------------

        emptyLocation = i;


        return true;
    }


    // =========================================================
    // CHECK SOLVED
    // =========================================================

    private bool CheckCompletion()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != i.ToString())
                return false;
        }

        return true;
    }


    // =========================================================
    // WAIT BEFORE SHUFFLE
    // =========================================================

    private IEnumerator WaitShuffle(float duration)
    {
        shuffling = true;

        yield return new WaitForSeconds(duration);

        Shuffle();

        solved = false;
        shuffling = false;
    }


    // =========================================================
    // SHUFFLE
    // =========================================================

    private void Shuffle()
    {
        int shuffleCount = size * size * size;

        int previousEmptyLocation = -1;

        for (int count = 0; count < shuffleCount; count++)
        {
            List<int> validMoves = GetValidMoves();

            // Không chọn lại move vừa thực hiện
            if (validMoves.Count > 1)
            {
                validMoves.Remove(previousEmptyLocation);
            }

            if (validMoves.Count == 0)
                continue;


            int randomIndex =
                UnityEngine.Random.Range(0, validMoves.Count);

            int pieceIndex =
                validMoves[randomIndex];


            previousEmptyLocation = emptyLocation;


            // Tính offset từ piece -> empty
            int offset = emptyLocation - pieceIndex;


            int colCheck = GetColCheck(offset);


            SwapIfValid(
                pieceIndex,
                offset,
                colCheck
            );
        }
    }


    // =========================================================
    // GET VALID MOVES
    // =========================================================

    private List<int> GetValidMoves()
    {
        List<int> validMoves = new();


        int row = emptyLocation / size;
        int col = emptyLocation % size;


        // Piece phía trên ô trống
        if (row > 0)
        {
            validMoves.Add(emptyLocation - size);
        }


        // Piece phía dưới ô trống
        if (row < size - 1)
        {
            validMoves.Add(emptyLocation + size);
        }


        // Piece bên trái ô trống
        if (col > 0)
        {
            validMoves.Add(emptyLocation - 1);
        }


        // Piece bên phải ô trống
        if (col < size - 1)
        {
            validMoves.Add(emptyLocation + 1);
        }


        return validMoves;
    }
}