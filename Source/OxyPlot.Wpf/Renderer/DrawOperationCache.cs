using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OxyPlot.Wpf
{
    public class DrawOperationCache
    {
        /// <summary>
        /// A collection of the operations that have been executed during a draw call.
        /// On future draw calls the list of operations is checked for modifications.
        /// </summary>
        private readonly List<IDrawOperation> operations = new List<IDrawOperation>();

        /// <summary>
        /// The canvas containing the currently elements of the Plot.
        /// </summary>
        private Canvas canvas = null;

        /// <summary>
        /// The clip rectangle.
        /// </summary>
        private Rect? clip;

        /// <summary>
        /// Contains the result of the last <see cref="Draw(IDrawOperation)"/> operation.
        /// </summary>
        private DrawResult? currentDrawState = null;

        /// <summary>
        /// The index if the current element of the current draw operation.
        /// </summary>
        private int currentElementIndex = -1;

        /// <summary>
        /// The index of the current draw operation.
        /// </summary>
        private int currentOperationIndex = -1;

        /// <summary>
        /// The current tool tip
        /// </summary>
        private string currentToolTip;

        private int statsDifferentCount = 0;
        private int statsEqualCount = 0;
        private int statsMovedCount = 0;

        /// <summary>
        /// Signals the begining of a the drawing of the plot.
        /// </summary>
        public void BeginDraw()
        {
            currentOperationIndex = -1;
            currentElementIndex = -1;
            currentDrawState = null;

            statsDifferentCount = 0;
            statsEqualCount = 0;
            statsMovedCount = 0;
        }

        /// <summary>
        /// Signals that the plot has finished drawing.
        /// </summary>
        public void EndDraw()
        {
            currentOperationIndex++;
            ClearAfterCurrent();

            int statInsertCount = 0;
            int statRemoveCount = 0;

            List<FrameworkElement> newChildren = new List<FrameworkElement>();

            foreach (IDrawOperation drawOperation in operations)
            {
                newChildren.AddRange(drawOperation.UIElements);
            }

            // Remove all the elements that are not part of the canvas.
            for (int i = 0; i < canvas.Children.Count; i++)
            {
                FrameworkElement frameworkElement = (FrameworkElement)canvas.Children[i];
                int index = newChildren.IndexOf(frameworkElement);
                if (index < 0)
                {
                    canvas.Children.RemoveAt(i);
                    i--;
                    statRemoveCount++;
                }
            }

            // Add any new elements
            for (int i = 0; i < newChildren.Count; i++)
            {
                FrameworkElement frameworkElement = newChildren[i];

                if (canvas.Children.Count > i && canvas.Children[i] == frameworkElement)
                {
                    continue;
                }

                int index = canvas.Children.IndexOf(frameworkElement);
                if (index >= 0)
                {
                    continue;
                }

                canvas.Children.Insert(i, frameworkElement);
                statInsertCount++;
            }

            //Console.WriteLine("DrawOperationCache {0} different, {1} moved, {2} equal. Insert|Removes: {3}|{4}", statsDifferentCount, statsMovedCount, statsEqualCount, statInsertCount, statRemoveCount);
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
                    throw new InvalidOperationException("Cannot create and add if the operations are equal");
                //element = (T)TryConsume();
                //break;

                case DrawResult.Moved:
                case DrawResult.Different:
                    // Clear the elements in the current draw operation.
                    EndConsume();

                    element = new T();
                    break;
            }

            if (this.clip != null)
            {
                element.Clip = new RectangleGeometry(
                        new Rect(
                            this.clip.Value.X - clipOffsetX,
                            this.clip.Value.Y - clipOffsetY,
                            this.clip.Value.Width,
                            this.clip.Value.Height));
            }

            this.ApplyToolTip(element);

            this.operations[currentOperationIndex].Add(element);

            return element;
        }

        /// <summary>
        /// Draw an element and return the comparison with the previous draw sequence.
        /// </summary>
        /// <param name="operation">The draw operation.</param>
        /// <returns>A status code that compares this call with the one from the previous sequence.</returns>
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

            switch (result)
            {
                case DrawResult.Equal:
                    statsEqualCount++;
                    break;

                case DrawResult.Moved:
                    statsMovedCount++;
                    break;

                case DrawResult.Different:
                    statsDifferentCount++;
                    break;
            }
            currentDrawState = result;
            return result;
        }

        /// <summary>
        /// Get the
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clipOffsetX"></param>
        /// <param name="clipOffsetY"></param>
        /// <returns></returns>
        protected T GetNext<T>(double clipOffsetX = 0, double clipOffsetY = 0) where T : FrameworkElement
        {
            if (currentDrawState == DrawResult.Equal)
            {
                throw new InvalidOperationException("");
            }

            FrameworkElement element = TryConsume();
            if (element == null || !(element is T))
            {
                throw new InvalidOperationException();
            }

            if (this.clip != null)
            {
                element.Clip = new RectangleGeometry(
                                new Rect(
                                    this.clip.Value.X - clipOffsetX,
                                    this.clip.Value.Y - clipOffsetY,
                                    this.clip.Value.Width,
                                    this.clip.Value.Height));
            }

            return (T)element;
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

        /// <summary>
        /// Clear the remaining operations.
        /// </summary>
        private void ClearAfterCurrent()
        {
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
