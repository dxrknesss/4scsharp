namespace practice2;

class Task4
{
  public static int[,] CreateSquareMatrix(int size)
  {
    int[,] matrix = new int[size, size];
    for (int i = 0; i < matrix.GetLength(0); i++)
    {
      for (int j = 0; j < matrix.GetLength(1); j++)
      {
        matrix[i, j] = i + j;
      }
    }
    return matrix;
  }

  public static void ReadOddRows(int[,] matrix)
  {
    for (int i = 0; i < matrix.GetLength(1); i++)
    {
      for (int j = 0; j < matrix.GetLength(0); j += 2)
      {
        Console.Write($"{matrix[i, j]} ");
      }
      Console.WriteLine();
    }
  }
}
