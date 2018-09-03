using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace OxyPlot.Wpf
{
    internal abstract class ADrawOperation<T> : IDrawOperation, IEquatable<T>
    {
        private readonly List<FrameworkElement> frameworkElements = new List<FrameworkElement>();

        IList<FrameworkElement> IDrawOperation.UIElements
        {
            get { return frameworkElements; }
        }

        public void Add(FrameworkElement frameworkElement)
        {
            frameworkElements.Add(frameworkElement);
        }

        public void Clear()
        {
            frameworkElements.Clear();
        }

        DrawResult IDrawOperation.Compare(IDrawOperation obj)
        {
            DrawResult result = DrawResult.Different;
            if (ReferenceEquals(obj, this))
            {
                result = DrawResult.Equal;
            }
            else if (obj is T)
            {
                T other = (T)obj;
                if (Equals(other))
                {
                    result = DrawResult.Equal;
                }
                else if (Transposed(other))
                {
                    result = DrawResult.Moved;
                }
            }
            return result;
        }

        public void CopyFrom(IDrawOperation other)
        {
            frameworkElements.Clear();
            frameworkElements.AddRange(other.UIElements);
        }

        public abstract bool Equals(T other);

        public abstract bool Transposed(T other);

        static protected bool ArrayEquals(double[] a, double[] b)
        {
            return (a == b) || Enumerable.SequenceEqual(a, b);
        }

        protected static bool ListEquals<TElem>(IList<TElem> a, IList<TElem> b) where TElem : IEquatable<TElem>
        {
            return (a == b) || Enumerable.SequenceEqual(a, b);
        }

        protected static bool ListEquals<TElem>(IList<IList<TElem>> a, IList<IList<TElem>> b) where TElem : IEquatable<TElem>
        {
            if (a == b)
                return true;

            if (a.Count != b.Count)
                return false;

            for (int i = 0; i < a.Count; i++)
            {
                if (!ListEquals(a[i], b[i]))
                    return false;
            }
            return true;
        }

        static protected bool Transposed(IList<IList<ScreenPoint>> a, IList<IList<ScreenPoint>> b)
        {
            if (a.Count != b.Count)
                return false;

            if (a.Count == 0)
                return true;

            double dxPrev = double.NaN;
            double dyPrev = double.NaN;
            double dx = double.NaN;
            double dy = double.NaN;

            for (int i = 0; i < a.Count; i++)
            {
                if (!Transposed(a[i], b[i], out dx, out dy))
                    return false;

                if (a[i].Count == 0)
                    continue;

                if (!double.IsNaN(dxPrev))
                {
                    if (dx != dxPrev || dy != dyPrev)
                        return false;
                }

                dxPrev = dx;
                dyPrev = dy;
            }
            return true;
        }

        static protected bool Transposed(ScreenPoint a, ScreenPoint b)
        {
            return true;
        }

        static protected bool Transposed(IList<ScreenPoint> a, IList<ScreenPoint> b, out double dx, out double dy)
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
                    return false;
            }
            return true;
        }

        static protected bool Transposed(IList<ScreenPoint> a, IList<ScreenPoint> b)
        {
            if (a.Count != b.Count)
                return false;

            if (a.Count == 0)
                return true;

            double dx = a[0].X - b[0].X;
            double dy = a[0].Y - b[0].Y;

            for (int i = 1; i < a.Count; i++)
            {
                if ((a[i].X - b[i].X != dx) ||
                    (a[i].Y - b[i].Y != dy))
                    return false;
            }
            return true;
        }

        static protected bool Transposed(IList<OxyRect> a, IList<OxyRect> b)
        {
            if (a.Count != b.Count)
                return false;

            if (a.Count == 0)
                return true;

            double dx = a[0].Right - b[0].Right;
            double dy = a[0].Top - b[0].Top;
            for (int i = 0; i < a.Count; i++)
            {
                if (!Transposed(a[i], b[i]))
                    return false;

                if (a[i].Right - b[i].Right != dx
                    || a[i].Top - b[i].Top != dy)
                {
                    return false;
                }
            }
            return true;
        }

        static protected bool Transposed(OxyRect a, OxyRect b)
        {
            return a.Width == b.Width
                && a.Height == b.Height;
        }

        protected bool Equals(OxyImage a, OxyImage b)
        {
            if (a == b)
                return true;
            if (a == null || b == null)
                return false;

            return a.Width == b.Width
                && a.BitsPerPixel == b.BitsPerPixel
                && a.DpiX == b.DpiX
                && a.DpiY == b.DpiY
                && Enumerable.SequenceEqual(a.GetData(), b.GetData());
        }
    }
}
