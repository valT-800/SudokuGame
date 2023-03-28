using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Numbers: MonoBehaviour
{
    private static int columns = 9;
    private static float every_square_offset = 0.0f;
    private static Vector2 start_position = new(0.0f, 0.0f);
    private static float square_scale = 1.0f;
    private static GameObject number_square;

    private static List<GameObject> numbers_squares_ = new();

    private static int selected = 0;

    // Start is called before the first frame update
    void Start()
    {
        //if (number_square.GetComponent<NumberSquare>() == null)
            //Debug.LogError("number_square object need to have NumberSquare scipt attached");
        //CreateContainer();
        //SetNumbers();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CreateContainer(GameObject numberSquare, float squareOffset, Vector2 startPosition, float squareScale)
    {
        number_square = numberSquare;
        every_square_offset = squareOffset;
        start_position = startPosition;
        square_scale = squareScale;

        SpawnSquares();
        SetSquarePosition();
        SetNumbers();
    }

    private void SpawnSquares()
    {
        for (int column = 0; column < columns; column++)
        {
            numbers_squares_.Add(Instantiate(number_square));
            numbers_squares_[numbers_squares_.Count - 1].transform.parent = transform;
            numbers_squares_[numbers_squares_.Count - 1].transform.localScale = new Vector3(square_scale, square_scale, square_scale);
        }
        

    }

    private void SetSquarePosition()
    {
        var square_rect = numbers_squares_[0].GetComponent<RectTransform>();
        Vector2 offset = new()
        {
            x = square_rect.rect.width * square_rect.transform.localScale.x + every_square_offset,
            y = square_rect.rect.height * square_rect.transform.localScale.y + every_square_offset
        };

        int column_number = 0;

        foreach (GameObject square in numbers_squares_)
        {
            var pos_x_offset = offset.x * column_number;
            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(start_position.x + pos_x_offset, start_position.y);
            column_number++;

        }
    }
    private void SetNumbers()
    {
        int i =1;
        foreach (var square in numbers_squares_)
        {
            square.GetComponent<NumberSquare>().SetNumber(i);
            i++;
        }
    }
    public int SelectNumber()
    {
        int i = 1;
        foreach (var square in numbers_squares_)
        {
            if (square.activeInHierarchy == true)
            {
                selected = i;
            }
            i++;
        }
        return selected;
    }
}
