using WorldEngine;
using MathNet.Spatial.Euclidean;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace WorldEngineTest
{
    public class MatrixTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void MultiplicationWithNumbers()
        {
            Matrix<double> A = new(2, 2);
            A[0, 0] = 0;
            A[0, 1] = 1;
            A[1, 0] = 0;
            A[1, 1] = 0;

            Matrix<double> B = new(2, 2);
            B[0, 0] = 0;
            B[0, 1] = 0;
            B[1, 0] = 0;
            B[1, 1] = 1;

            var ans = A * B;
            Assert.Multiple(() =>
            {
                Assert.That(ans[0, 0], Is.EqualTo(0));
                Assert.That(ans[0, 1], Is.EqualTo(1));
                Assert.That(ans[1, 0], Is.EqualTo(0));
                Assert.That(ans[1, 1], Is.EqualTo(0));
            });
        }

        [Test]
        public void MultiplicationWithExpr()
        {
            Matrix<Symbolic3D> A = new(2, 2);
            A[0, 0] = new Symbolic3D(Expr.Zero, Expr.Zero, Expr.Zero);
            A[0, 1] = new Symbolic3D(Expr.One, Expr.One, Expr.One);
            A[1, 0] = new Symbolic3D(Expr.Zero, Expr.Zero, Expr.Zero);
            A[1, 1] = new Symbolic3D(Expr.Zero, Expr.Zero, Expr.Zero);

            Matrix<Symbolic3D> B = new(2, 2);
            B[0, 0] = new Symbolic3D(Expr.Zero, Expr.Zero, Expr.Zero);
            B[0, 1] = new Symbolic3D(Expr.Zero, Expr.Zero, Expr.Zero);
            B[1, 0] = new Symbolic3D(Expr.Zero, Expr.Zero, Expr.Zero);
            B[1, 1] = new Symbolic3D(Expr.One, Expr.One, Expr.One);

            var ans = A * B;
            Assert.Multiple(() =>
            {
                Assert.That(ans[0, 0], Is.EqualTo(new Symbolic3D(Expr.Zero, Expr.Zero, Expr.Zero)));
                Assert.That(ans[0, 1], Is.EqualTo(new Symbolic3D(Expr.One, Expr.One, Expr.One)));
                Assert.That(ans[1, 0], Is.EqualTo(new Symbolic3D(Expr.Zero, Expr.Zero, Expr.Zero)));
                Assert.That(ans[1, 1], Is.EqualTo(new Symbolic3D(Expr.Zero, Expr.Zero, Expr.Zero)));
            });
        }

        [Test]
        public void InvertWithNumber()
        {
            Matrix<double> B = new(2, 2);
            B[0, 0] = -1;
            B[0, 1] = 3.0 / 2.0;
            B[1, 0] = 1;
            B[1, 1] = -1;

            var ans = B.Inverse();
            Assert.Multiple(() =>
            {
                Assert.That(ans[0, 0], Is.EqualTo(2));
                Assert.That(ans[0, 1], Is.EqualTo(3));
                Assert.That(ans[1, 0], Is.EqualTo(2));
                Assert.That(ans[1, 1], Is.EqualTo(2));
            });
        }

        [Test]
        public void InvertWithExpr()
        {
            Matrix<Expr> B = new(2, 2);
            B[0, 0] = Expr.Parse("-1");
            B[0, 1] = Expr.Parse("3.0 / 2.0");
            B[1, 0] = Expr.Parse("1");
            B[1, 1] = Expr.Parse("-1");

            var ans = B.InverseExpr();
            Assert.Multiple(() =>
            {
                Assert.That(ans[0, 0].ToString(), Is.EqualTo(Expr.Parse("2.0").ToString()));
                Assert.That(ans[0, 1].ToString(), Is.EqualTo(Expr.Parse("3.0").ToString()));
                Assert.That(ans[1, 0].ToString(), Is.EqualTo(Expr.Parse("2.0").ToString()));
                Assert.That(ans[1, 1].ToString(), Is.EqualTo(Expr.Parse("2.0").ToString()));
            });
        }

        [Test]
        public void DeterminantWithNumber()
        {
            Matrix<double> B = new(2, 2);
            B[0, 0] = 2;
            B[0, 1] = 3;
            B[1, 0] = 4;
            B[1, 1] = 5;

            var ans = B.Determinant();
            Assert.That(ans, Is.EqualTo(-2));
        }

        [Test]
        public void DeterminantWithExpr()
        {
            Matrix<Expr> B = new(3, 3);
            B[0, 0] = Expr.Parse("1");
            B[0, 1] = Expr.Parse("2");
            B[0, 2] = Expr.Parse("3");
            B[1, 0] = Expr.Parse("4");
            B[1, 1] = Expr.Parse("5");
            B[1, 2] = Expr.Parse("6");
            B[2, 0] = Expr.Parse("7");
            B[2, 1] = Expr.Parse("8");
            B[2, 2] = Expr.Parse("9");

            var ans = B.DeterminantExpr();
            Assert.That(ans.ToString(), Is.EqualTo(Expr.Parse("0").ToString()));
        }

        [Test]
        public void ReducedRowEchelonWithNumber()
        {
            Matrix<double> B = new(2, 3);
            B[0, 0] = 1;
            B[0, 1] = 2;
            B[0, 2] = 3;
            B[1, 0] = 2;
            B[1, 1] = 4;
            B[1, 2] = 6;

            var ans = B.ReducedRowEchelonForm();
            Assert.Multiple(() =>
            {
                Assert.That(ans.Rows, Is.EqualTo(2));
                Assert.That(ans.Cols, Is.EqualTo(3));
                Assert.That(ans[0, 0], Is.EqualTo(1));
                Assert.That(ans[0, 1], Is.EqualTo(2));
                Assert.That(ans[0, 2], Is.EqualTo(3));
                Assert.That(ans[1, 0], Is.EqualTo(0));
                Assert.That(ans[1, 1], Is.EqualTo(0));
                Assert.That(ans[1, 2], Is.EqualTo(0));
            });
        }


        [Test]
        public void KernelWithNumber()
        {
            Matrix<double> B = new(2, 3);
            B[0, 0] = 1;
            B[0, 1] = 2;
            B[0, 2] = 3;
            B[1, 0] = 2;
            B[1, 1] = 4;
            B[1, 2] = 6;

            var ans = B.Kernel();
            Assert.Multiple(() =>
            {
                Assert.That(ans, Has.Count.EqualTo(2));
                Assert.That(ans[0][0], Is.EqualTo(-2));
                Assert.That(ans[0][1], Is.EqualTo(1));
                Assert.That(ans[0][2], Is.EqualTo(0));
                Assert.That(ans[1][0], Is.EqualTo(-3));
                Assert.That(ans[1][1], Is.EqualTo(0));
                Assert.That(ans[1][2], Is.EqualTo(1));
            });
        }

        [Test]
        public void ImageWithNumber()
        {
            Matrix<double> A = new(3, 4);
            A[0, 0] = 1;
            A[0, 1] = 2;
            A[0, 2] = 3;
            A[0, 3] = 4;
            A[1, 0] = 1;
            A[1, 1] = 4;
            A[1, 2] = 0;
            A[1, 3] = 2;
            A[2, 0] = 2;
            A[2, 1] = 2;
            A[2, 2] = 9;
            A[2, 3] = 10;

            var ans = A.ImageBasisColumns();
            Assert.Multiple(() =>
            {
                Assert.That(ans, Has.Count.EqualTo(2));
                Assert.That(ans[0][0], Is.EqualTo(1));
                Assert.That(ans[0][1], Is.EqualTo(0));
                Assert.That(ans[0][2], Is.EqualTo(0));
                Assert.That(ans[1][0], Is.EqualTo(0));
                Assert.That(ans[1][1], Is.EqualTo(1));
                Assert.That(ans[1][2], Is.EqualTo(0));
            });
        }

        [Test]
        public void ReducedRowEchelonWithExpr()
        {
            Matrix<Expr> A = new(3, 4);
            A[0, 0] = Expr.Parse("1");
            A[0, 1] = Expr.Parse("1");
            A[0, 2] = Expr.Parse("0");
            A[0, 3] = Expr.Parse("1");
            A[1, 0] = Expr.Parse("-1");
            A[1, 1] = Expr.Parse("3");
            A[1, 2] = Expr.Parse("2");
            A[1, 3] = Expr.Parse("1");
            A[2, 0] = Expr.Parse("4");
            A[2, 1] = Expr.Parse("0");
            A[2, 2] = Expr.Parse("-2");
            A[2, 3] = Expr.Parse("1");

            var ans = A.ReducedRowEchelonFormExpr();
            Assert.Multiple(() =>
            {
                Assert.That(ans.Rows, Is.EqualTo(3));
                Assert.That(ans.Cols, Is.EqualTo(4));

                Assert.That(ans[0, 0].ToString(), Is.EqualTo("1"));
                Assert.That(ans[0, 1].ToString(), Is.EqualTo("0"));
                Assert.That(ans[0, 2].ToString(), Is.EqualTo("-1/2"));
                Assert.That(ans[0, 3].ToString(), Is.EqualTo("0"));

                Assert.That(ans[1, 0].ToString(), Is.EqualTo("0"));
                Assert.That(ans[1, 1].ToString(), Is.EqualTo("1"));
                Assert.That(ans[1, 2].ToString(), Is.EqualTo("1/2"));
                Assert.That(ans[1, 3].ToString(), Is.EqualTo("0"));

                Assert.That(ans[2, 0].ToString(), Is.EqualTo("0"));
                Assert.That(ans[2, 1].ToString(), Is.EqualTo("0"));
                Assert.That(ans[2, 2].ToString(), Is.EqualTo("0"));
                Assert.That(ans[2, 3].ToString(), Is.EqualTo("1"));
            });
        }

        [Test]
        public void KernelWithExpr()
        {
            Matrix<Expr> A = new(3, 4);
            A[0, 0] = Expr.Parse("1");
            A[0, 1] = Expr.Parse("1");
            A[0, 2] = Expr.Parse("0");
            A[0, 3] = Expr.Parse("1");
            A[1, 0] = Expr.Parse("-1");
            A[1, 1] = Expr.Parse("3");
            A[1, 2] = Expr.Parse("2");
            A[1, 3] = Expr.Parse("1");
            A[2, 0] = Expr.Parse("4");
            A[2, 1] = Expr.Parse("0");
            A[2, 2] = Expr.Parse("-2");
            A[2, 3] = Expr.Parse("1");

            var ans = A.KernelExpr();
            Assert.Multiple(() =>
            {
                Assert.That(ans, Has.Count.EqualTo(1));

                Assert.That(ans[0][0].ToString(), Is.EqualTo("1/2"));
                Assert.That(ans[0][1].ToString(), Is.EqualTo("-1/2"));
                Assert.That(ans[0][2].ToString(), Is.EqualTo("1"));
                Assert.That(ans[0][3].ToString(), Is.EqualTo("0"));
            });
        }

        [Test]
        public void ImageWithExpr()
        {
            Matrix<Expr> A = new(3, 4);
            A[0, 0] = 1;
            A[0, 1] = 2;
            A[0, 2] = 3;
            A[0, 3] = 4;
            A[1, 0] = 1;
            A[1, 1] = 4;
            A[1, 2] = 0;
            A[1, 3] = 2;
            A[2, 0] = 2;
            A[2, 1] = 2;
            A[2, 2] = 9;
            A[2, 3] = 10;

            var ans = A.ImageBasisColumnsExpr();
            Assert.Multiple(() =>
            {
                Assert.That(ans, Has.Count.EqualTo(2));
                Assert.That(ans[0][0].ToString(), Is.EqualTo("1"));
                Assert.That(ans[0][1].ToString(), Is.EqualTo("0"));
                Assert.That(ans[0][2].ToString(), Is.EqualTo("0"));
                Assert.That(ans[1][0].ToString(), Is.EqualTo("0"));
                Assert.That(ans[1][1].ToString(), Is.EqualTo("1"));
                Assert.That(ans[1][2].ToString(), Is.EqualTo("0"));
            });
        }
    }
}