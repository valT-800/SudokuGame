using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SudokuContainer : MonoBehaviour
{
    public int size = 0;
    public int level = 0;
    private int empty = 0;

    public float every_square_offset = 0.0f;
    public float every_block_offset = 0.0f;
    public float y_offset = 0.0f;
    private Vector2 grid_start_position = new(0.0f, 0.0f);
    private Vector2 numbers_start_position = new(0.0f, 0.0f);
    public float square_scale = 1.0f;
    //private float height = 0.0f;
    //private float width = 0.0f;
    private float grid_square_size = 0.0f;
    private float number_square_size = 0.0f;
    private float grid_size = 0.0f;
    private Vector3 grid_image_size = new(0.0f, 0.0f);

    private Vector2 streakes_counter_position = new();
    private Vector2 timer_position = new();
    private Vector2 eraser_position = new();

    public GameObject grid_square;
    public GameObject number_square;
    public GameObject dialog;
    public GameObject info_block;
    public GameObject grid_image;
    public GameObject eraser;
    public GameObject menu;

    private GameObject dialog_;
    private GameObject streakes_counter_;
    private GameObject timer_;
    private GameObject eraser_;
    private GameObject grid_image_;
    private GameObject menu_;

    private List<GameObject> grid_squares_ = new();
    private static List<GameObject> numbers_squares_ = new();

    private ColorBlock default_colors = new();
    private Color streak = new(180, 0, 0);

    private string selected = "";
    private int streakes = 0;
    private TimeSpan time;
    private DateTime start_time;
    private int empty_;

    private Sudoku sudoku;

    void Start()
    {

        if (grid_square.GetComponent<GridSquare>() == null || number_square.GetComponent<NumberSquare>() == null
            || dialog.GetComponent<Dialog>() == null || info_block.GetComponent<InfoBlock>() == null || grid_image.GetComponent<Grid>() == null)
            Debug.LogError("some of objects don't have scipt attached");


        StartGame();



    }

    // Update is called once per frame
    void Update()
    {
        DisplayTime();
        //Works when mouse was pressed
        if (Input.GetMouseButtonDown(0))
        {
            //Works when was pressed gameobject
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                if (EventSystem.current.currentSelectedGameObject.GetComponent<Eraser>() != null)
                {
                    selected = "0";
                }
                //Works when was pressed numbersquare object
                else if (EventSystem.current.currentSelectedGameObject.GetComponent<NumberSquare>() != null)
                {
                    //Searchs what numbers_squares_ component was pressed by its name.
                    //When the match is found makes last selected number that components text value
                    foreach (var number in numbers_squares_)
                    {
                        if (number.name == EventSystem.current.currentSelectedGameObject.name)
                        {
                            selected = number.GetComponent<NumberSquare>().number_text.GetComponent<Text>().text;

                        }
                    }

                }
                //Works when was pressed gridsquare object
                else if (EventSystem.current.currentSelectedGameObject.GetComponent<GridSquare>() != null)
                {
                    //Searchs what grid_squares_ component was pressed by its name.
                    //When the match is found checks for streakes and changes that components text to the number that was selected last
                    foreach (var square in grid_squares_)
                    {
                        if (square.name == EventSystem.current.currentSelectedGameObject.name && square.GetComponent<GridSquare>().enabled == true)
                        {
                            square.GetComponent<GridSquare>().ChangeNumber(int.Parse(selected));
                            if (selected == "0" )
                            {
                                empty_++;
                                square.GetComponent<GridSquare>().colors = default_colors;
                            }
                            else
                            {
                              
                                if (!CheckForStreak(square))
                                {
                                    ColorBlock colors = default_colors;
                                    colors.normalColor = streak;
                                    colors.selectedColor = streak;
                                    colors.pressedColor = streak;
                                    square.GetComponent<GridSquare>().colors = colors;
                                    streakes++;
                                    CheckForGameOver();
                                }
                                else
                                {
                                    square.GetComponent<GridSquare>().colors = default_colors;
                                }
                                empty_--;
                                CheckForWin();

                            }
                            


                        }
                        
                        
                    }
                }


            }
        }
    }
    private void SelectLevel()
    {
        var menu_ = Instantiate(menu);
        menu_.transform.SetParent(transform);
        
    }
    private void StartGame()
    {
        SetParametres();
        ResetParametres();
        SetGridImage();
        SetTimer();
        SetStreaksContainer();
        SetDialog();
        SetEraser();
        CreateGrid();
        CreateNumbersContainer();

    }
    private void RestartGame()
    {
        
        
        ResetParametres();
        Activate();
        SetGridNumbers();
    }

    private void SetParametres()
    {
        if (level == 1) empty = 30;
        else if (level == 2) empty = 40;
        else if (level == 3) empty = 50;
        else if (level == 4) empty = 60;

        default_colors = grid_square.GetComponent<GridSquare>().colors;

        Canvas canvas = FindObjectOfType<Canvas>();

        //height = canvas.GetComponent<RectTransform>().rect.height;
        //width = canvas.GetComponent<RectTransform>().rect.width;   

        var grid_square_rect = grid_square.GetComponent<RectTransform>();
        grid_square_size = grid_square_rect.rect.width;
        grid_size = grid_square_size * size + every_square_offset * (size - 1) + every_block_offset * 2;
        grid_start_position.x = -(grid_size / 2 - grid_square_size / 2);
        grid_start_position.y = grid_size / 2 - grid_square_size / 2;

        grid_image_size = new Vector3((grid_size + 20.0f) / grid_size * size, (grid_size + 20.0f) / grid_size * size);

        var info_block_rect = info_block.GetComponent<RectTransform>();
        streakes_counter_position = new Vector2(grid_size / 2 - info_block_rect.rect.width / 2, info_block_rect.position.y);
        timer_position = new Vector2(-(grid_size / 2 - info_block_rect.rect.width / 2), info_block_rect.position.y);

        var number_square_rect = number_square.GetComponent<RectTransform>();
        number_square_size = number_square_rect.rect.width;
        var numbers_size = number_square_size * size + every_square_offset * (size - 1);
        numbers_start_position.x = -(numbers_size / 2 - number_square_size / 2);
        numbers_start_position.y = grid_start_position.y - grid_size - y_offset - number_square_size / 2;

        eraser_position = new Vector2(numbers_size / 2 + y_offset, numbers_start_position.y);

        

    }
    private void ResetParametres()
    {
        empty_ = empty;
        start_time = DateTime.Now;
        streakes = 0;
        selected = "";

        sudoku = new(size, empty);
        sudoku.fillValues();

        foreach (var square in grid_squares_)
        {
            square.GetComponent<GridSquare>().colors = default_colors;
        }

    }
    private void SetEraser()
    {
        eraser_ = Instantiate(eraser);
        eraser_.transform.SetParent(transform);
        eraser_.transform.localScale = new Vector3(square_scale, square_scale, square_scale);
        eraser_.GetComponent<RectTransform>().anchoredPosition = eraser_position;
    }
    private void SetGridImage()
    {

        grid_image_ = Instantiate(grid_image);
        grid_image_.transform.SetParent(transform);
        grid_image_.transform.localScale = grid_image_size;

    }
    public void CreateGrid()
    {

        SpawnGridSquares();
        SetSquarePosition();
        SetGridNumbers();
    }
    private void SpawnGridSquares()
    {
        for (int row = 0; row < size; row++)
        {
            for (int column = 0; column < size; column++)
            {
                grid_squares_.Add(Instantiate(grid_square));
                grid_squares_[grid_squares_.Count - 1].transform.SetParent(transform);
                grid_squares_[grid_squares_.Count - 1].transform.localScale = new Vector3(square_scale, square_scale, square_scale);
            }
        }

    }
    private void SetSquarePosition()
    {
        var square_rect = grid_squares_[0].GetComponent<RectTransform>();
        Vector2 square_offset = new()
        {
            x = grid_square_size * square_rect.transform.localScale.x + every_square_offset,
            y = grid_square_size * square_rect.transform.localScale.y + every_square_offset
        };
        int column_number = 0;
        int row_number = 0;

        foreach (GameObject square in grid_squares_)
        {
            if (column_number + 1 > size)
            {
                row_number++;
                column_number = 0;
            }
            var pos_x_offset = square_offset.x * column_number;
            var pos_y_offset = square_offset.y * row_number;
            if (column_number >= 3)
                pos_x_offset += every_block_offset;
            if (column_number >= 6)
                pos_x_offset += every_block_offset;
            if (row_number >= 3)
                pos_y_offset += every_block_offset;
            if (row_number >= 6)
                pos_y_offset += every_block_offset;

            square.GetComponent<RectTransform>().anchoredPosition = new Vector3(grid_start_position.x + pos_x_offset, grid_start_position.y - pos_y_offset);
            column_number++;

        }
    }

    private void SetGridNumbers()
    {
        int column_number = 0;
        int row_number = 0;
        foreach (var square in grid_squares_)
        {
            if (column_number + 1 > size)
            {
                row_number++;
                column_number = 0;
            }
            square.GetComponent<GridSquare>().SetNumber(sudoku.mat[row_number, column_number]);
            string name = string.Format("{0},{1}", row_number, column_number);
            square.name = name;
            column_number++;


        }
    }
    private void CreateNumbersContainer()
    {

        SpawnSquares();
        SetNumberSquarePosition();
        SetNumbers();
    }

    private void SpawnSquares()
    {
        for (int column = 0; column < size; column++)
        {
            numbers_squares_.Add(Instantiate(number_square));
            numbers_squares_[numbers_squares_.Count - 1].transform.SetParent(transform);
            numbers_squares_[numbers_squares_.Count - 1].transform.localScale = new Vector3(square_scale, square_scale, square_scale);
        }


    }

    private void SetNumberSquarePosition()
    {
        var square_rect = numbers_squares_[0].GetComponent<RectTransform>();
        Vector2 offset = new()
        {
            x = number_square_size * square_rect.transform.localScale.x + every_square_offset,
            y = number_square_size * square_rect.transform.localScale.y + every_square_offset
        };

        int column_number = 0;

        foreach (GameObject square in numbers_squares_)
        {
            var pos_x_offset = offset.x * column_number;
            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(numbers_start_position.x + pos_x_offset, numbers_start_position.y);
            column_number++;

        }
    }

    private void SetNumbers()
    {
        int i = 1;
        foreach (var square in numbers_squares_)
        {
            square.GetComponent<NumberSquare>().SetNumber(i);
            var name = i.ToString();
            square.name = name;
            i++;

        }
    }

    private bool CheckForStreak(GameObject square)
    {
        string[] position = square.name.Split(',');
        var row = int.Parse(position[0]);
        var col = int.Parse(position[1]);

        if (sudoku.CheckIfSafe(row, col, int.Parse(selected)) || selected =="0")
        {
            sudoku.mat[row, col] = int.Parse(selected);
            return true;        
        }
        else
        {
            return false;
        }

    }

    private void SetDialog()
    {
        dialog_ = Instantiate(dialog);
        dialog_.SetActive(false);
    }
    private void CheckForGameOver()
    {
        if (streakes >= 3)
        {
            Deactivate();
            
            
            dialog_.SetActive(true);
            dialog_.transform.SetParent(transform);
            dialog_.transform.localScale = new Vector3(square_scale, square_scale, square_scale);
            dialog_.GetComponent<Dialog>().SetDialogMessage("Game Over");
            var button = dialog_.GetComponent<Dialog>().button.GetComponent<Button>();
            button.GetComponent<Button>().onClick.AddListener(RestartGame);

        }
        DisplayStreaks();
    }

    private void SetStreaksContainer()
    {
        streakes_counter_ = Instantiate(info_block);
        streakes_counter_.transform.SetParent(transform);
        streakes_counter_.transform.localScale = new Vector3(square_scale, square_scale, square_scale);
        streakes_counter_.GetComponent<RectTransform>().anchoredPosition = streakes_counter_position;
        DisplayStreaks();
    }
    private void DisplayStreaks()
    {
        streakes_counter_.GetComponent<InfoBlock>().SetStreakes(streakes);

    }
    private void Deactivate()
    {
        dialog_.SetActive(true);
        grid_image_.SetActive(false);
        eraser_.SetActive(false);
        foreach( var square in grid_squares_)
        {
            square.SetActive(false);
        }
        foreach(var number in numbers_squares_)
        {
            number.SetActive(false);
        }
        
    }
    private void Activate()
    {
        dialog_.SetActive(false);
        grid_image_.SetActive(true);
        eraser_.SetActive(true);
        foreach (var square in grid_squares_)
        {
            square.SetActive(true);
        }
        foreach (var number in numbers_squares_)
        {
            number.SetActive(true);
        }
    }
    private void SetTimer()
    {
        timer_ = Instantiate(info_block);
        timer_.transform.SetParent(transform);
        timer_.transform.localScale = new Vector3(square_scale, square_scale, square_scale);
        timer_.GetComponent<RectTransform>().anchoredPosition = timer_position;
        DisplayTime();
    }
    private void DisplayTime()
    {
        time = DateTime.Now - start_time;
        var dt = Convert.ToDateTime(time.ToString());
        timer_.GetComponent<InfoBlock>().SetTime(dt);
    }
    private void CheckForWin()
    {
        if (empty_ == 0)
        {
            Deactivate();
            dialog_.transform.SetParent(transform);
            dialog_.transform.localScale = new Vector3(square_scale, square_scale, square_scale);
            dialog_.GetComponent<Dialog>().SetDialogMessage("You won");
            var button = dialog_.GetComponent<Dialog>().button.GetComponent<Button>();
            button.GetComponent<Button>().onClick.AddListener(RestartGame);
        }
    }
}
