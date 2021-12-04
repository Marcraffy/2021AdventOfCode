namespace Data
{
    public class Board
    {
        private bool[,] isMarked { get; set; }
        private int[,] data { get; set; }
        private int winningNumber { get; set; }
        public bool HasWon { get; set; }

        public bool Bingo => ((isMarked[0, 0] && isMarked[0, 1] && isMarked[0, 2] && isMarked[0, 3] && isMarked[0, 4])
                            || (isMarked[1, 0] && isMarked[1, 1] && isMarked[1, 2] && isMarked[1, 3] && isMarked[1, 4])
                            || (isMarked[2, 0] && isMarked[2, 1] && isMarked[2, 2] && isMarked[2, 3] && isMarked[2, 4])
                            || (isMarked[3, 0] && isMarked[3, 1] && isMarked[3, 2] && isMarked[3, 3] && isMarked[3, 4])
                            || (isMarked[4, 0] && isMarked[4, 1] && isMarked[4, 2] && isMarked[4, 3] && isMarked[4, 4]))
                            ||
                              ((isMarked[0, 0] && isMarked[1, 0] && isMarked[2, 0] && isMarked[3, 0] && isMarked[4, 0])
                            || (isMarked[0, 1] && isMarked[1, 1] && isMarked[2, 1] && isMarked[3, 1] && isMarked[4, 1])
                            || (isMarked[0, 2] && isMarked[1, 2] && isMarked[2, 2] && isMarked[3, 2] && isMarked[4, 2])
                            || (isMarked[0, 3] && isMarked[1, 3] && isMarked[2, 3] && isMarked[3, 3] && isMarked[4, 3])
                            || (isMarked[0, 4] && isMarked[1, 4] && isMarked[2, 4] && isMarked[3, 4] && isMarked[4, 4]));

        public int Score
        {
            get
            {
                var score = 0;
                for (int row = 0; row < 5; row++)
                {
                    for (int column = 0; column < 5; column++)
                    {
                        if (!isMarked[row, column])
                        {
                            score += data[row, column];
                        }
                    }
                }
                return score * winningNumber;
            }
        }

        public Board(BingoBoard board)
        {
            data = board.Board;
            isMarked = new bool[5, 5];
        }

        public bool CheckNumber(int number)
        {
            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    if (number == data[row, column] && !isMarked[row, column])
                    {
                        isMarked[row, column] = true;
                        if (Bingo)
                        {
                            winningNumber = number;
                            HasWon = true;
                            return true;
                        }
                        return false;
                    }
                }
            }

            return false;
        }
    }
}