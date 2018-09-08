namespace OxyPlot.Wpf.Tests
{
    public class TestDrawOperation : ADrawOperation<TestDrawOperation>
    {
        private readonly string name;
        private readonly OxyRect rect;

        public TestDrawOperation(string name, OxyRect rect)
        {
            this.name = name;
            this.rect = rect;
        }

        public override bool Equals(TestDrawOperation other)
        {
            return this.name.Equals(other.name)
                && this.rect.Equals(other.rect);
        }

        public override string ToString()
        {
            return $"{this.name}: {this.rect}";
        }

        public override bool Transposed(TestDrawOperation other)
        {
            return this.name.Equals(other.name)
                && Transposed(this.rect, other.rect);
        }
    }
}
