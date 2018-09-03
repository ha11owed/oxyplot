using System.Collections.Generic;
using System.Windows;

namespace OxyPlot.Wpf
{
    public interface IDrawOperation
    {
        IList<FrameworkElement> UIElements { get; }

        void Add(FrameworkElement frameworkElement);

        void Clear();

        DrawResult Compare(IDrawOperation obj);

        void CopyFrom(IDrawOperation other);
    }
}
