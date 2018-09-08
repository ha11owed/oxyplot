namespace OxyPlot.Wpf.Tests
{
    using NUnit.Framework;
    using System.Windows.Controls;

    [TestFixture]
    public class DrawOperationCacheTests
    {
        public class DefaultValues
        {
            [Test]
            public void DrawOperationCache()
            {
                Canvas canvas = new Canvas();
                TestDrawOperationCache oc = new TestDrawOperationCache(canvas);

                Assert.AreEqual(0, canvas.Children.Count);

                int n = 3;
                // Draw without changes
                for (int callCount = 0; callCount < n; callCount++)
                {
                    oc.BeginDraw();

                    // Fixed 1
                    DrawFixed1(oc, callCount == 0);

                    // Fixed 2
                    DrawFixed2(oc, callCount == 0);

                    oc.EndDraw();

                    // Check the result of the Draw
                    oc.AssertChildren(new string[]
                    {
                        "fixed1.a",
                        "fixed1.b",
                        "fixed2.a",
                        "fixed2.b",
                        "fixed2.c",
                    });

                    oc.TakeSnapshot();
                }

                // Draw with a move
                for (int callCount = 0; callCount < n; callCount++)
                {
                    oc.BeginDraw();

                    // Fixed 1
                    DrawFixed1(oc, false);

                    // Moving
                    oc.Draw(new TestDrawOperation("moving 1", new OxyRect(3 + callCount, 0, 2, 2)));
                    if (callCount == 0)
                    {
                        oc.CreateAndAdd("moving1.a");
                    }
                    else
                    {
                        oc.GetNext("moving1.a");
                    }

                    // Fixed 2
                    DrawFixed2(oc, callCount == 0);

                    oc.EndDraw();

                    // Check the result of the Draw
                    oc.AssertChildren(new string[]
                    {
                        "fixed1.a",
                        "fixed1.b",
                        "moving1.a",
                        "fixed2.a",
                        "fixed2.b",
                        "fixed2.c",
                    });

                    if (callCount == 0)
                    {
                        Assert.AreEqual(2, oc.CountUnchanged());
                    }
                    else
                    {
                        Assert.AreEqual(6, oc.CountUnchanged());
                    }

                    oc.TakeSnapshot();
                }

                // Draw with a changed draw operation
                for (int callCount = 0; callCount < n; callCount++)
                {
                    oc.BeginDraw();

                    // Fixed 1
                    DrawFixed1(oc, false);

                    // Change the draw call.
                    // Also the number of elements is different on every call.
                    string changeName = "change " + callCount;
                    oc.Draw(new TestDrawOperation(changeName, new OxyRect(3 + callCount, 0, 2, 2)));
                    oc.CreateAndAdd(changeName + ".a");
                    if (callCount % 2 == 0)
                    {
                        oc.CreateAndAdd(changeName + ".b");
                    }

                    // Fixed 2
                    DrawFixed2(oc, true);

                    oc.EndDraw();

                    if (callCount % 2 == 0)
                    {
                        // Check the result of the Draw
                        oc.AssertChildren(new string[]
                        {
                            "fixed1.a",
                            "fixed1.b",
                            "change " + callCount + ".a",
                            "change " + callCount + ".b",
                            "fixed2.a",
                            "fixed2.b",
                            "fixed2.c",
                        });
                    }
                    else
                    {
                        // Check the result of the Draw
                        oc.AssertChildren(new string[]
                        {
                            "fixed1.a",
                            "fixed1.b",
                            "change " + callCount + ".a",
                            "fixed2.a",
                            "fixed2.b",
                            "fixed2.c",
                        });
                    }

                    Assert.AreEqual(2, oc.CountUnchanged());

                    oc.TakeSnapshot();
                }
            }

            /// <summary>
            /// Fixed 1
            /// </summary>
            private static void DrawFixed1(TestDrawOperationCache oc, bool create)
            {
                oc.Draw(new TestDrawOperation("fixed elements 1", new OxyRect(0, 0, 1, 1)));

                if (create)
                {
                    oc.CreateAndAdd("fixed1.a");
                    oc.CreateAndAdd("fixed1.b");
                }
            }

            /// <summary>
            /// Fixed 2
            /// </summary>
            private static void DrawFixed2(TestDrawOperationCache oc, bool create)
            {
                oc.Draw(new TestDrawOperation("fixed elements 2", new OxyRect(2, 2, 2, 2)));

                if (create)
                {
                    oc.CreateAndAdd("fixed2.a");
                    oc.CreateAndAdd("fixed2.b");
                    oc.CreateAndAdd("fixed2.c");
                }
            }
        }
    }
}
