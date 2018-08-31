using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace OxyPlot.Wpf
{
    internal abstract class ADrawOperation<T> : IDrawOperation
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
                if (Transposed((T)obj))
                {
                    result = DrawResult.Moved;
                }
                else
                {
                    result = DrawResult.Equal;
                }
            }
            return result;
        }

        public void CopyFrom(IDrawOperation other)
        {
            frameworkElements.Clear();
            frameworkElements.AddRange(other.UIElements);
        }

        public bool Equals(IDrawOperation other)
        {
            IDrawOperation oThis = (IDrawOperation)this;
            return oThis.Compare(other) == DrawResult.Equal;
        }

        public abstract bool Transposed(T other);

        static protected bool ArrayEquals(double[] a, double[] b)
        {
            return (a == b) || Enumerable.SequenceEqual(a, b);
        }

        static protected bool Transposed(IList<IList<ScreenPoint>> a, IList<IList<ScreenPoint>> b)
        {
            if (a.Count != b.Count)
                return false;

            for (int i = 0; i < a.Count; i++)
            {
                if (!Transposed(a[i], b[i]))
                    return false;
            }
            return true;
        }

        static protected bool Transposed(ScreenPoint a, ScreenPoint b)
        {
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

            for (int i = 0; i < a.Count; i++)
            {
                if (!Transposed(a[i], b[i]))
                    return false;
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
