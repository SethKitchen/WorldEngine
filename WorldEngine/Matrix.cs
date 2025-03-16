using System.Numerics;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace WorldEngine
{
    public class Matrix<T>
    {
        public int Rows { get; }
        public int Cols { get; }
        private readonly T[,] data;

        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            data = new T[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    try
                    {
                        data[i, j] = Activator.CreateInstance<T>(); // Uses reflection
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public Matrix(T[,] values)
        {
            Rows = values.GetLength(0);
            Cols = values.GetLength(1);
            data = (T[,])values.Clone();
        }

        public T this[int i, int j]
        {
            get => data[i, j];
            set => data[i, j] = value;
        }

        public static Matrix<T> operator +(Matrix<T> a, Matrix<T> b)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols)
                throw new ArgumentException("Matrix dimensions must match for addition.");

            Matrix<T> result = new(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Cols; j++)
                    result[i, j] = (a[i, j] as dynamic) + (b[i, j] as dynamic);

            return result;
        }

        public static Matrix<T> operator -(Matrix<T> a, Matrix<T> b)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols)
                throw new ArgumentException("Matrix dimensions must match for subtraction.");

            Matrix<T> matrix = new(a.Rows, a.Cols);
            Matrix<T> result = matrix;
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Cols; j++)
                    result[i, j] = (a[i, j] as dynamic) - (b[i, j] as dynamic);

            return result;
        }

        public static Matrix<T> operator *(Matrix<T> a, Matrix<T> b)
        {
            if (a.Cols != b.Rows)
                throw new ArgumentException("Matrix multiplication requires A.cols == B.rows.");

            Matrix<T> result = new(a.Rows, b.Cols);
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < b.Cols; j++)
                    for (int k = 0; k < a.Cols; k++)
                        result[i, j] += (a[i, k] as dynamic) * (b[k, j] as dynamic);

            return result;
        }

        public static Matrix<T> operator *(Matrix<T> a, double scalar)
        {
            Matrix<T> result = new(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Cols; j++)
                    result[i, j] = (a[i, j] as dynamic) * scalar;

            return result;
        }

        public Matrix<T> Transpose()
        {
            Matrix<T> result = new(Cols, Rows);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    result[j, i] = data[i, j];

            return result;
        }
    }

    public static class MatrixExtensions
    {
        public static Matrix<T> Inverse<T>(this Matrix<T> matrix) where T : INumber<T>
        {
            if (matrix.Rows != matrix.Cols)
                throw new ArgumentException("Only square matrices can be inverted.");

            int n = matrix.Rows;
            Matrix<T> augmented = new(n, 2 * n);

            // Copy the original matrix and setup the identity matrix.
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmented[i, j] = matrix[i, j];
                }
                for (int j = n; j < 2 * n; j++)
                {
                    augmented[i, j] = (i == (j - n)) ? T.One : T.Zero;
                }
            }

            // Gauss-Jordan elimination (similar to above).
            for (int i = 0; i < n; i++)
            {
                dynamic pivot = augmented[i, i];
                if (pivot == T.Zero)
                {
                    int swapRow = i + 1;
                    while (swapRow < n && augmented[swapRow, i] == T.Zero)
                    {
                        swapRow++;
                    }
                    if (swapRow == n)
                        throw new Exception("Matrix is singular and cannot be inverted.");
                    for (int j = 0; j < 2 * n; j++)
                    {
                        (augmented[swapRow, j], augmented[i, j]) = (augmented[i, j], augmented[swapRow, j]);
                    }
                    pivot = augmented[i, i];
                }
                for (int j = 0; j < 2 * n; j++)
                {
                    augmented[i, j] = (augmented[i, j] as dynamic) / pivot;
                }
                for (int k = 0; k < n; k++)
                {
                    if (k == i) continue;
                    dynamic factor = augmented[k, i];
                    for (int j = 0; j < 2 * n; j++)
                    {
                        augmented[k, j] = (augmented[k, j] as dynamic) - factor * (augmented[i, j] as dynamic);
                    }
                }
            }

            Matrix<T> inverse = new(n, n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    inverse[i, j] = augmented[i, j + n];
                }
            }
            return inverse;
        }

        public static Matrix<T> InverseExpr<T>(this Matrix<T> matrix) where T : Expr
        {
            if (matrix.Rows != matrix.Cols)
                throw new ArgumentException("Only square matrices can be inverted.");

            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Cols; j++)
                {
                    if (matrix[i,j] == null)
                    {
                        matrix[i, j] = (T)Expr.Zero;
                    }
                }
            }

            int n = matrix.Rows;
            Matrix<T> augmented = new(n, 2 * n);

            // Copy the original matrix and setup the identity matrix.
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmented[i, j] = matrix[i, j];
                }
                for (int j = n; j < 2 * n; j++)
                {
                    augmented[i, j] = (T)((i == (j - n)) ? Expr.One : Expr.Zero);
                }
            }

            // Gauss-Jordan elimination (similar to above).
            for (int i = 0; i < n; i++)
            {
                dynamic pivot = augmented[i, i];
                if (pivot == Expr.Zero)
                {
                    int swapRow = i + 1;
                    while (swapRow < n && augmented[swapRow, i] == Expr.Zero)
                    {
                        swapRow++;
                    }
                    if (swapRow == n)
                        throw new Exception("Matrix is singular and cannot be inverted.");
                    for (int j = 0; j < 2 * n; j++)
                    {
                        (augmented[swapRow, j], augmented[i, j]) = (augmented[i, j], augmented[swapRow, j]);
                    }
                    pivot = augmented[i, i];
                }
                for (int j = 0; j < 2 * n; j++)
                {
                    augmented[i, j] = (augmented[i, j] as dynamic) / pivot;
                }
                for (int k = 0; k < n; k++)
                {
                    if (k == i) continue;
                    dynamic factor = augmented[k, i];
                    for (int j = 0; j < 2 * n; j++)
                    {
                        augmented[k, j] = (augmented[k, j] as dynamic) - factor * (augmented[i, j] as dynamic);
                    }
                }
            }

            Matrix<T> inverse = new(n, n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    inverse[i, j] = augmented[i, j + n];
                }
            }
            return inverse;
        }

        // Determines the determinant using Gaussian elimination for numeric types.
        public static T Determinant<T>(this Matrix<T> matrix) where T : INumber<T>
        {
            if (matrix.Rows != matrix.Cols)
                throw new ArgumentException("Only square matrices have determinants.");

            int n = matrix.Rows;
            // Create a copy of the matrix so we don't modify the original.
            Matrix<T> mat = new(n, n);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    mat[i, j] = matrix[i, j];

            T det = T.One;
            // We'll adjust the sign if we swap rows.
            int sign = 1;

            for (int i = 0; i < n; i++)
            {
                dynamic pivot = mat[i, i];
                int pivotRow = i;
                if (pivot == T.Zero)
                {
                    bool found = false;
                    for (int k = i + 1; k < n; k++)
                    {
                        if (mat[k, i] != T.Zero)
                        {
                            pivot = mat[k, i];
                            pivotRow = k;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        return T.Zero; // The matrix is singular.

                    // Swap rows i and pivotRow.
                    for (int j = 0; j < n; j++)
                    {
                        (mat[pivotRow, j], mat[i, j]) = (mat[i, j], mat[pivotRow, j]);
                    }
                    sign = -sign;
                }
                det *= pivot;
                // Eliminate below pivot.
                for (int k = i + 1; k < n; k++)
                {
                    dynamic factor = (mat[k, i] as dynamic) / pivot;
                    for (int j = i; j < n; j++)
                    {
                        mat[k, j] = (mat[k, j] as dynamic) - factor * (mat[i, j] as dynamic);
                    }
                }
            }

            if (sign < 0)
            {
                // Multiply by negative one (T.Zero - T.One gives -1)
                det *= (T.Zero - T.One);
            }
            return det;
        }

        // Determines the determinant using Gaussian elimination for expression types.
        public static T DeterminantExpr<T>(this Matrix<T> matrix) where T : Expr
        {
            if (matrix.Rows != matrix.Cols)
                throw new ArgumentException("Only square matrices have determinants.");

            int n = matrix.Rows;
            // Create a copy of the matrix.
            Matrix<T> mat = new(n, n);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    mat[i, j] = matrix[i, j];

            T det = (T)Expr.One;
            int sign = 1;

            for (int i = 0; i < n; i++)
            {
                dynamic pivot = mat[i, i];
                int pivotRow = i;
                if (pivot == Expr.Zero)
                {
                    bool found = false;
                    for (int k = i + 1; k < n; k++)
                    {
                        if (mat[k, i] != Expr.Zero)
                        {
                            pivot = mat[k, i];
                            pivotRow = k;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        return (T)Expr.Zero;

                    for (int j = 0; j < n; j++)
                    {
                        (mat[pivotRow, j], mat[i, j]) = (mat[i, j], mat[pivotRow, j]);
                    }
                    sign = -sign;
                }
                det = (det as dynamic) * pivot;
                for (int k = i + 1; k < n; k++)
                {
                    dynamic factor = (mat[k, i] as dynamic) / pivot;
                    for (int j = i; j < n; j++)
                    {
                        mat[k, j] = (mat[k, j] as dynamic) - factor * (mat[i, j] as dynamic);
                    }
                }
            }

            if (sign < 0)
            {
                det = (det as dynamic) * ((Expr.Zero as dynamic) - Expr.One);
            }
            return det;
        }
    }
}