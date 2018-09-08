using NUnit.Framework;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace OxyPlot.Wpf.Tests
{
    public class TestDrawOperationCache : DrawOperationCache
    {
        private Canvas canvas;
        private object[] previous;

        public TestDrawOperationCache(Canvas canvas)
        {
            this.canvas = canvas;
            this.previous = new object[0];
            Init(canvas);
        }

        public void AssertChildren(string[] expectedTexts)
        {
            var children = canvas.Children;
            Assert.AreEqual(expectedTexts.Length, children.Count);
            for (int i = 0; i < expectedTexts.Length; i++)
            {
                var tb = (TextBlock)children[i];
                Assert.AreEqual(expectedTexts[i], tb.Text);
            }
        }

        public int CountUnchanged()
        {
            int unchanged = ChildrenToArray().Intersect(this.previous).Count();
            return unchanged;
        }

        public void CreateAndAdd(string text)
        {
            var tb = base.CreateAndAdd<TextBlock>();
            tb.Text = text;
        }

        public void Draw(TestDrawOperation operation)
        {
            base.Draw(operation);
        }

        public void GetNext(string text)
        {
            var tb = base.GetNext<TextBlock>();
            Assert.AreEqual(text, tb.Text);
        }

        public void TakeSnapshot()
        {
            this.previous = ChildrenToArray();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string separator = "";
            foreach (object child in ChildrenToArray())
            {
                sb.Append(separator);
                separator = ", ";
                if (child is TextBlock)
                {
                    sb.Append(((TextBlock)child).Text);
                }
                else
                {
                    sb.Append(child);
                }
            }
            return sb.ToString();
        }

        private object[] ChildrenToArray()
        {
            var children = new object[this.canvas.Children.Count];
            for (int i = 0; i < this.canvas.Children.Count; i++)
            {
                children[i] = this.canvas.Children[i];
            }
            return children;
        }
    }
}
