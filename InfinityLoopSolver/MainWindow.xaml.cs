using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using InfinityLoopSolver.GameItems;

namespace InfinityLoopSolver
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<IGameItem> GameItems = new List<IGameItem>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MoveUpButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GameItems.Any(x => x.Position.Y == 13))
            {
                SystemSounds.Exclamation.Play();
                return;
            }

            foreach (var item in GameItems)
            {
                item.Position += new Vector2(0, 1);
            }
            UpdateButtonsImages();
        }

        private void MoveDownButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GameItems.Any(x => x.Position.Y == 1))
            {
                SystemSounds.Exclamation.Play();
                return;
            }

            foreach (var item in GameItems)
            {
                item.Position -= new Vector2(0, 1);
            }
            UpdateButtonsImages();
        }

        private void MoveRightButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GameItems.Any(x => x.Position.X == 8))
            {
                SystemSounds.Exclamation.Play();
                return;
            }

            foreach (var item in GameItems)
            {
                item.Position += new Vector2(1, 0);
            }
            UpdateButtonsImages();
        }

        private void MoveLeftButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GameItems.Any(x => x.Position.X == 1))
            {
                SystemSounds.Exclamation.Play();
                return;
            }

            foreach (var item in GameItems)
            {
                item.Position -= new Vector2(1, 0);
            }
            UpdateButtonsImages();
        }

        private Button GetButtonFromPosition(Vector2 position)
        {
            if (position.X < 1 || position.X > 8 || position.Y < 1 || position.Y > 13)
            {
                return null;
            }

            return GameBoard.FindName($"GameItemButton_{position.X}_{position.Y}") as Button;
        }

        private Vector2 GetPositionFromButton(Button button)
        {
            if (button == null || !button.Name.StartsWith("GameItemButton_"))
            {
                return default(Vector2);
            }

            var split = button.Name.Split('_');
            return new Vector2(int.Parse(split[1]), int.Parse(split[2]));
        }

        private void UpdateButtonsImages()
        {
            for (var x = 1; x <= 8; x++)
            {
                for (var y = 1; y <= 13; y++)
                {
                    GetButtonFromPosition(new Vector2(x, y)).Background = new ImageBrush();
                }
            }
            foreach (var item in GameItems)
            {
                var button = GetButtonFromPosition(item.Position);
                var resourceUri = new Uri($"Resources/{item.DirectionFlags}.png", UriKind.Relative);
                var stream = Application.GetResourceStream(resourceUri);
                var temp = BitmapFrame.Create(stream.Stream);
                var brush = new ImageBrush {ImageSource = temp};
                button.Background = brush;
            }
        }

        // SPAGHETTI ALERT

        private void AnyGameItemButtonClicked(object sender, RoutedEventArgs e)
        {
            var position = GetPositionFromButton(sender as Button);

            IGameItem item;
            IGameItem item2;

            if (SelectOneDir.IsChecked.Value)
            {
                item = new OneDirectionalThing(position);
            }
            else if (SelectTwoDir.IsChecked.Value)
            {
                item = new TwoDirectionalThing(position);
            }
            else if (SelectStraight.IsChecked.Value)
            {
                item = new StraightThing(position);
            }
            else if (SelectThreeDir.IsChecked.Value)
            {
                item = new ThreeDirectionalThing(position);
            }
            else if (SelectFourDir.IsChecked.Value)
            {
                item = new FourDirectionalThing(position);
            }
            else
            {
                GameItems.RemoveAll(x => x.Position == position);
                UpdateButtonsImages();
                return;
            }

            if ((item2 = GameItems.FirstOrDefault(x => x.Position == position)) != null)
            {
                if (item2.GetType() == item.GetType())
                {
                    if (item2.PossibleDirections.Length == 1)
                    {
                        return;
                    }

                    var selectThis = false;
                    foreach (var pos in item2.PossibleDirections)
                    {
                        if (selectThis)
                        {
                            item2.DirectionFlags = pos;
                            UpdateButtonsImages();
                            return;
                        }

                        if (pos == item2.DirectionFlags)
                        {
                            selectThis = true;
                        }
                    }
                    item2.DirectionFlags = item2.PossibleDirections[0];
                    UpdateButtonsImages();
                    return;
                }

                GameItems.RemoveAll(x => x.Position == position);
            }

            GameItems.Add(item);
            UpdateButtonsImages();
        }

        private void SolveButton_OnClick(object sender, RoutedEventArgs e)
        {
            BigInteger possibilities = 1;
            foreach (var item in GameItems)
            {
                possibilities *= item.PossibleDirections.Length;
            }

            while (Solver.RemoveImpossibleConditions(GameItems)) ;
            BigInteger possibleCount = 1;
            foreach (var item in GameItems)
            {
                possibleCount *= item.PossibleDirections.Length;
            }

            var result = MessageBox.Show($"There {(possibilities == 1 ? "is" : "are")} {possibilities} total possibilities on this, but our ultra smart system removed impossible ones and there {(possibleCount == 1 ? "is" : "are")} only {possibleCount} left. Calculation will start as soon as you press OK.", "Information", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel)
            {
                goto reset;
            }

            GameItems = Solver.GetPassingCondition(GameItems);
            if (!Solver.GetIfComplete(GameItems))
            {
                MessageBox.Show("This is some impossible bullshit.");
            }
            UpdateButtonsImages();

            reset:
            // Reset possible directions
            foreach (var item in GameItems)
            {
                if (item is TwoDirectionalThing)
                {
                    item.PossibleDirections = new byte[] { 9, 5, 6, 10 };
                }
                else if (item is StraightThing)
                {
                    item.PossibleDirections = new byte[] { 12, 3 };
                }
                else if (item is FourDirectionalThing)
                {
                    item.PossibleDirections = new byte[] { 15 };
                }
                else if (item is OneDirectionalThing)
                {
                    item.PossibleDirections = new byte[] { 8, 1, 4, 2 };
                }
                else if (item is ThreeDirectionalThing)
                {
                    item.PossibleDirections = new byte[] { 11, 13, 7, 14 };
                }
            }
        }

        private void ClearAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            GameItems.Clear();
            UpdateButtonsImages();
        }
    }
}
