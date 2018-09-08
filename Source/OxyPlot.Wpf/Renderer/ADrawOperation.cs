namespace OxyPlot.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// The base class for all Draw operations.
    /// It stores the UI elements that are generated as a result of the draw call.
    /// </summary>
    /// <typeparam name="T">The type of the final draw call.</typeparam>
    public abstract class ADrawOperation<T> : IDrawOperation, IEquatable<T>
    {
        /// <summary>
        /// The UI elements that need to be added to the Canvas for the current draw call.
        /// </summary>
        private readonly List<FrameworkElement> frameworkElements = new List<FrameworkElement>();

        /// <summary>
        /// Gets the UI elements that need to be added to the Canvas for the current draw call.
        /// </summary>
        IList<FrameworkElement> IDrawOperation.UIElements
        {
            get { return this.frameworkElements; }
        }

        /// <summary>
        /// Add a new UI element for the current draw call.
        /// </summary>
        /// <param name="frameworkElement">element to add</param>
        public void Add(FrameworkElement frameworkElement)
        {
            this.frameworkElements.Add(frameworkElement);
        }

        /// <summary>
        /// Clear all the current UI elements
        /// </summary>
        public void Clear()
        {
            this.frameworkElements.Clear();
        }

        /// <summary>
        /// Compare two draw calls.
        /// </summary>
        /// <param name="obj">draw call to compare with</param>
        /// <returns>The compare result</returns>
        DrawResult IDrawOperation.Compare(IDrawOperation obj)
        {
            DrawResult result = DrawResult.Different;

            do
            {
                if (ReferenceEquals(obj, this))
                {
                    result = DrawResult.Equal;
                    break;
                }

                if (!(obj is T))
                {
                    break;
                }

                T other = (T)obj;
                if (this.Equals(other))
                {
                    result = DrawResult.Equal;
                    break;
                }

                if (this.Transposed(other))
                {
                    result = DrawResult.Moved;
                }
            }
            while (false);

            return result;
        }

        /// <summary>
        /// Copy the UI elements from another draw call.
        /// </summary>
        /// <param name="other">The draw call to copy from.</param>
        public void CopyFrom(IDrawOperation other)
        {
            this.frameworkElements.Clear();
            this.frameworkElements.AddRange(other.UIElements);
        }

        /// <summary>
        /// Check if two draw calls are equal.
        /// </summary>
        /// <param name="other">Object to use in the equality check.</param>
        /// <returns>True if the objects are equal.</returns>
        public abstract bool Equals(T other);

        /// <summary>
        /// Check if two draw calls are tranposed.
        /// Two draw calls are thansposed if the only difference between the two is the position where the objects are drawn.
        /// </summary>
        /// <param name="other">Object to use in the equality check.</param>
        /// <returns>True if the objects are equal.</returns>
        public abstract bool Transposed(T other);

        /// <summary>
        /// Helper to check if two arrays are equal.
        /// </summary>
        /// <param name="a">First item the the comparison.</param>
        /// <param name="b">Second item the the comparison.</param>
        /// <returns>The result of the equlity check.</returns>
        protected static bool ArrayEquals(double[] a, double[] b)
        {
            return (a == b) || Enumerable.SequenceEqual(a, b);
        }

        /// <summary>
        /// Helper to check if two lists are equal.
        /// </summary>
        /// <param name="a">First item the the comparison.</param>
        /// <param name="b">Second item the the comparison.</param>
        /// <returns>The result of the equlity check.</returns>
        protected static bool ListEquals<TElem>(IList<TElem> a, IList<TElem> b) where TElem : IEquatable<TElem>
        {
            return (a == b) || Enumerable.SequenceEqual(a, b);
        }

        /// <summary>
        /// Helper to check if two lists of lists are equal.
        /// </summary>
        /// <param name="a">First item the the comparison.</param>
        /// <param name="b">Second item the the comparison.</param>
        /// <returns>The result of the equlity check.</returns>
        protected static bool ListEquals<TElem>(IList<IList<TElem>> a, IList<IList<TElem>> b) where TElem : IEquatable<TElem>
        {
            if (a == b)
            {
                return true;
            }

            if (a.Count != b.Count)
            {
                return false;
            }

            for (int i = 0; i < a.Count; i++)
            {
                if (!ListEquals(a[i], b[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Helper to check if two lists of lists are transposed.
        /// </summary>
        /// <param name="a">First item the the comparison.</param>
        /// <param name="b">Second item the the comparison.</param>
        /// <returns>The result of the transpose check.</returns>
        protected static bool Transposed(IList<IList<ScreenPoint>> a, IList<IList<ScreenPoint>> b)
        {
            if (a.Count != b.Count)
            {
                return false;
            }

            // If the list is empty a tranposition check is not possible.
            if (a.Count == 0)
            {
                return true;
            }

            double dxPrev = double.NaN;
            double dyPrev = double.NaN;
            double dx = double.NaN;
            double dy = double.NaN;

            for (int i = 0; i < a.Count; i++)
            {
                if (!Transposed(a[i], b[i], out dx, out dy))
                {
                    return false;
                }

                // If the list is empty a tranposition check is not possible.
                if (a[i].Count == 0)
                {
                    continue;
                }

                // Check if the tranpose amount has changed.
                // If thats the case two lists are tranposed by different amounts
                // meaning that the lists of lists are not transposed.
                if (!double.IsNaN(dxPrev) && (dx != dxPrev || dy != dyPrev))
                {
                    return false;
                }

                dxPrev = dx;
                dyPrev = dy;
            }
            return true;
        }

        /// <summary>
        /// Helper to check if two points are transposed.
        /// The helper is only exists to make the code more readable,
        /// and just returns true.
        /// </summary>
        protected static bool Transposed(ScreenPoint a, ScreenPoint b)
        {
            return true;
        }

        /// <summary>
        /// Helper to check if two lists of points are transposed.
        /// The amount for the transposition is stored in the output parameters.
        /// </summary>
        protected static bool Transposed(IList<ScreenPoint> a, IList<ScreenPoint> b, out double dx, out double dy)
        {
            dx = double.NaN;
            dy = double.NaN;

            if (a.Count != b.Count)
            {
                return false;
            }

            if (a.Count == 0)
            {
                return true;
            }

            dx = a[0].X - b[0].X;
            dy = a[0].Y - b[0].Y;

            for (int i = 1; i < a.Count; i++)
            {
                if ((a[i].X - b[i].X != dx) ||
                    (a[i].Y - b[i].Y != dy))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Helper to check if two lists or points are transposed.
        /// </summary>
        /// <param name="a">First item the the comparison.</param>
        /// <param name="b">Second item the the comparison.</param>
        /// <returns>The result of the transpose check.</returns>
        protected static bool Transposed(IList<ScreenPoint> a, IList<ScreenPoint> b)
        {
            if (a.Count != b.Count)
            {
                return false;
            }

            if (a.Count == 0)
            {
                return true;
            }

            double dx = a[0].X - b[0].X;
            double dy = a[0].Y - b[0].Y;

            for (int i = 1; i < a.Count; i++)
            {
                if ((a[i].X - b[i].X != dx) ||
                    (a[i].Y - b[i].Y != dy))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Helper to check if two lists of rectangles are transposed.
        /// </summary>
        /// <param name="a">First item the the comparison.</param>
        /// <param name="b">Second item the the comparison.</param>
        /// <returns>The result of the transpose check.</returns>
        protected static bool Transposed(IList<OxyRect> a, IList<OxyRect> b)
        {
            if (a.Count != b.Count)
            {
                return false;
            }

            if (a.Count == 0)
            {
                return true;
            }

            double dx = a[0].Right - b[0].Right;
            double dy = a[0].Top - b[0].Top;
            for (int i = 0; i < a.Count; i++)
            {
                if (!Transposed(a[i], b[i]))
                {
                    return false;
                }

                if (a[i].Right - b[i].Right != dx
                    || a[i].Top - b[i].Top != dy)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Helper to check if two recttangles are transposed.
        /// </summary>
        /// <param name="a">First item the the comparison.</param>
        /// <param name="b">Second item the the comparison.</param>
        /// <returns>The result of the transpose check.</returns>
        protected static bool Transposed(OxyRect a, OxyRect b)
        {
            return a.Width == b.Width
                && a.Height == b.Height;
        }

        /// <summary>
        /// Helper to check if Two image objects are equal.
        /// TODO: maybe this should be moded in the <see cref="OxyImage"/> class.
        /// </summary>
        protected bool Equals(OxyImage a, OxyImage b)
        {
            if (a == b)
            {
                return true;
            }

            if (a == null || b == null)
            {
                return false;
            }

            return a.Width == b.Width
                && a.BitsPerPixel == b.BitsPerPixel
                && a.DpiX == b.DpiX
                && a.DpiY == b.DpiY
                && Enumerable.SequenceEqual(a.GetData(), b.GetData());
        }
    }
}
