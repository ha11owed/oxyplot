using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OxyPlot.Wpf
{
    public class DrawOperationCache
    {
        private readonly List<IDrawOperation> operations = new List<IDrawOperation>();
        private Canvas canvas = null;

        /// <summary>
        /// The clip rectangle.
        /// </summary>
        private Rect? clip;

        private DrawResult? currentDrawState = null;
        private int currentElementIndex = -1;
        private int currentOperationIndex = -1;

        /// <summary>
        /// The current tool tip
        /// </summary>
        private string currentToolTip;

        public void BeginDraw()
        {
            currentOperationIndex = -1;
            currentElementIndex = -1;
            currentDrawState = null;
        }

        public void EndDraw()
        {
            currentOperationIndex++;
            ClearAfterCurrent();

            int j = 0;
            foreach (IDrawOperation drawOperation in operations)
            {
                foreach (FrameworkElement fe in drawOperation.UIElements)
                {
                    if (canvas.Children.Count == j)
                    {
                        canvas.Children.Add(fe);
                    }
                    else if (fe == canvas.Children[j])
                    {
                        // Nothing to do
                    }
                    else
                    {
                        canvas.Children.RemoveAt(j);
                        canvas.Children.Insert(j, fe);
                    }
                    j++;
                }
            }

            int n = canvas.Children.Count - j;
            if (n > 0)
            {
                canvas.Children.RemoveRange(j, n);
            }
        }

        /// <summary>
        /// Resets the clip rectangle.
        /// </summary>
        public void ResetClip()
        {
            this.clip = null;
        }

        /// <summary>
        /// Sets the clipping rectangle.
        /// </summary>
        /// <param name="clippingRect">The clipping rectangle.</param>
        /// <returns><c>true</c> if the clip rectangle was set.</returns>
        public bool SetClip(OxyRect clippingRect)
        {
            this.clip = this.ToRect(clippingRect);
            return true;
        }

        /// <summary>
        /// Sets the tool tip for the following items.
        /// </summary>
        /// <param name="text">The text in the tool tip.</param>
        public void SetToolTip(string text)
        {
            this.currentToolTip = text;
        }

        /// <summary>
        /// Creates an element of the specified type and adds it to the canvas.
        /// </summary>
        /// <typeparam name="T">Type of element to create.</typeparam>
        /// <param name="clipOffsetX">The clip offset executable.</param>
        /// <param name="clipOffsetY">The clip offset asynchronous.</param>
        /// <returns>The element.</returns>
        protected T CreateAndAdd<T>(double clipOffsetX = 0, double clipOffsetY = 0) where T : FrameworkElement, new()
        {
            T element = null;

            switch (currentDrawState)
            {
                case DrawResult.Equal:
                    element = (T)TryConsume();
                    break;

                case DrawResult.Moved:
                case DrawResult.Different:
                    if (currentDrawState == DrawResult.Moved)
                    {
                        // Clear the elements in the current draw operation.
                        EndConsume();
                    }

                    // TODO: here we can reuse existing elements in the canvas.Children collection
                    element = new T();

                    if (this.clip != null)
                    {
                        element.Clip = new RectangleGeometry(
                                new Rect(
                                    this.clip.Value.X - clipOffsetX,
                                    this.clip.Value.Y - clipOffsetY,
                                    this.clip.Value.Width,
                                    this.clip.Value.Height));
                    }

                    this.operations[currentOperationIndex].Add(element);

                    this.ApplyToolTip(element);
                    break;
            }

            if (element == null)
            {
                throw new InvalidOperationException();
            }

            return element;
        }

        protected DrawResult Draw(IDrawOperation operation)
        {
            while (TryConsume() != null)
            {
                //throw new InvalidOperationException();
            }

            currentElementIndex = -1;
            currentOperationIndex++;

            DrawResult result = DrawResult.Different;

            bool add = true;
            if (currentOperationIndex >= 0 && currentOperationIndex < operations.Count)
            {
                result = operation.Compare(operations[currentOperationIndex]);

                if (result == DrawResult.Different)
                {
                    // Clear the operations after the current index
                    ClearAfterCurrent();
                }
                else
                {
                    // Copy the elements from the cached operation.
                    operation.CopyFrom(operations[currentOperationIndex]);
                    operations[currentOperationIndex] = operation;
                    add = false;
                }
            }

            if (add)
            {
                operations.Add(operation);
            }

            currentDrawState = result;
            return result;
        }

        protected T GetNext<T>() where T : FrameworkElement
        {
            FrameworkElement fe = TryConsume();
            if (fe == null) { throw new InvalidOperationException(); }
            if (!(fe is T)) { throw new InvalidOperationException(); }

            return (T)fe;
        }

        protected void Init(Canvas canvas)
        {
            BeginDraw();

            this.canvas = canvas;
            this.operations.Clear();
        }

        /// <summary>
        /// Converts an <see cref="OxyRect" /> to a <see cref="Rect" />.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        /// <returns>A <see cref="Rect" />.</returns>
        protected Rect ToRect(OxyRect r)
        {
            return new Rect(r.Left, r.Top, r.Width, r.Height);
        }

        /// <summary>
        /// Applies the current tool tip to the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        private void ApplyToolTip(FrameworkElement element)
        {
            if (!string.IsNullOrEmpty(this.currentToolTip))
            {
                element.ToolTip = this.currentToolTip;
            }
        }

        private void ClearAfterCurrent()
        {
            // Clear the remaining operations.
            int n = operations.Count - currentOperationIndex;
            if (n > 0)
            {
                operations.RemoveRange(currentOperationIndex, n);
            }
        }

        private void EndConsume()
        {
            IDrawOperation drawOperation = operations[currentOperationIndex];

            currentElementIndex++;
            for (int i = drawOperation.UIElements.Count - 1; i >= currentElementIndex; i--)
            {
                drawOperation.UIElements.RemoveAt(i);
            }

            currentElementIndex = int.MaxValue / 2;
        }

        private FrameworkElement TryConsume()
        {
            FrameworkElement result = null;

            if (currentOperationIndex >= 0 && currentOperationIndex < operations.Count)
            {
                IDrawOperation drawOperation = operations[currentOperationIndex];
                currentElementIndex++;
                if (currentElementIndex >= 0 && currentElementIndex < drawOperation.UIElements.Count)
                {
                    result = drawOperation.UIElements[currentElementIndex];
                }
            }

            return result;
        }
    }
}
