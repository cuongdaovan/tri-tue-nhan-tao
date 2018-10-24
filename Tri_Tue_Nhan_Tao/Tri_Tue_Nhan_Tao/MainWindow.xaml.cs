using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tri_Tue_Nhan_Tao
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private int red; //Số lượng quân còn lại của Đỏ
		private int black; //Số lượng quân còn lại của Đen
		private Move currentMove; // Move hiện tại
		private String turn; // Lượt đi quân
		public MainWindow()
		{
			InitializeComponent();
			NewGame();
		}
		// Tạo game mới
		private void NewGame()
		{
			red = 12;
			black = 12;
			this.Title = "Bên Đen đi";
			currentMove = null;
			turn = "Black";
			ClearBoard();
			MakeBoard();
			MakeButtons();
		}
		// Xóa bàn cờ
		private void ClearBoard()
		{
			for (int r = 0; r < 8; r++)
			{
				for (int c = 0; c < 8; c++)
				{
					StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, r, c);
					CheckersGrid.Children.Remove(stackPanel);
				}
			}
		}
		// Tạo bàn cờ
		private void MakeBoard()
		{
			for (int row = 0; row < 8; row++)
			{
				for (int col = 0; col < 8; col++)
				{
					StackPanel stackPanel = new StackPanel();
					if ((row + col) % 2 == 1)
						stackPanel.Background = Brushes.Black;
					else
						stackPanel.Background = Brushes.White;

					Grid.SetRow(stackPanel, row);
					Grid.SetColumn(stackPanel, col);
					CheckersGrid.Children.Add(stackPanel);
				}
			}
			MakeButtons();
		}
		// Tạo quân cờ
		private void MakeButtons()
		{
			for (int row = 0; row < 8; row++)
			{
				for (int col = 0; col < 8; col++)
				{
					StackPanel stackpanel = (StackPanel)GetGridElement(CheckersGrid, row, col);
					Button button = new Button();
					button.Height = 60;
					button.Width = 60;
					button.HorizontalAlignment = HorizontalAlignment.Center;
					button.VerticalAlignment = VerticalAlignment.Center;
					var redBrush = new ImageBrush();
					redBrush.ImageSource = new BitmapImage(new Uri("Resource/Red.png", UriKind.Relative));
					var blackBrush = new ImageBrush();
					blackBrush.ImageSource = new BitmapImage(new Uri("Resource/Black.png", UriKind.Relative));
					switch (row)
					{
						case 0:
							if (col % 2 == 0)
							{
								button.Background = redBrush;
								button.Name = "buttonRed" + row + col;
								stackpanel.Children.Add(button);
							}
							break;
						case 1:
							if (col % 2 == 1)
							{
								button.Background = redBrush;
								button.Name = "buttonRed" + row + col;
								stackpanel.Children.Add(button);
							}
							break;
						case 2:
							if (col % 2 == 0)
							{
								button.Background = redBrush;
								button.Name = "buttonRed" + row + col;
								stackpanel.Children.Add(button);
							}
							break;
						case 3:
							if (col % 2 == 1)
							{
								button.Background = Brushes.White;
								button.Name = "button" + row + col;
								stackpanel.Children.Add(button);
							}
							break;
						case 4:
							if (col % 2 == 0)
							{
								button.Background = Brushes.White;
								button.Name = "button" + row + col;
								stackpanel.Children.Add(button);
							}
							break;
						case 5:
							if (col % 2 == 1)
							{
								button.Background = blackBrush;
								button.Name = "buttonBlack" + row + col;
								stackpanel.Children.Add(button);
							}
							break;
						case 6:
							if (col % 2 == 0)
							{
								button.Background = blackBrush;
								button.Name = "buttonBlack" + row + col;
								stackpanel.Children.Add(button);
							}
							break;
						case 7:
							if (col % 2 == 1)
							{
								button.Background = blackBrush;
								button.Name = "buttonBlack" + row + col;
								stackpanel.Children.Add(button);
							}
							break;
						default:
							break;
					}
					button.Click += new RoutedEventHandler(button_CLick);
				}
			}
		}
		// Sự kiện click button
		public void button_CLick(Object sender, RoutedEventArgs args)
		{
			Button button = (Button)sender;
			StackPanel stackPanel = (StackPanel)button.Parent;
			int row = Grid.GetRow(stackPanel);
			int col = Grid.GetColumn(stackPanel);
			var colorBrush = new ImageBrush();
			// Nếu là chọn nút lần 1
			if (currentMove == null)
				currentMove = new Move();
			if (currentMove.piece1 == null)
			{
				currentMove.piece1 = new Piece(row, col);
                if (button.Name.Contains("KingBlack"))
				{
					colorBrush.ImageSource = new BitmapImage(new Uri("Resource/KingBClick.png", UriKind.Relative));
					button.Background = colorBrush;
				}
				else
				if (button.Name.Contains("Black"))
				{
					colorBrush.ImageSource = new BitmapImage(new Uri("Resource/BlackClick.png", UriKind.Relative));
					button.Background = colorBrush;
				}
			}
			else
			if (currentMove.piece2 == null)
			{
				currentMove.piece2 = new Piece(row, col);
				stackPanel.Background = Brushes.Green;
			}

			if ((currentMove.piece1 != null) && (currentMove.piece2 != null))
			{
				MakeMove(CheckMove());
				checkWin();
				MakeAI();
				checkWin();
			}
		}
		// Tạo nước đi cho AI
		private void MakeAI()
		{
			if (turn == "Red")
			{
				Move move = new Move();
				CheckerBoard checkerBoard = new CheckerBoard(CheckersGrid, currentMove, turn);
				CheckerAI checkerAI = new CheckerAI(checkerBoard);
				move = checkerAI.findbestMove();
				if (move != null)
				{
					currentMove = move;
					MakeMove(CheckMove());
				}
			}
		}
		// Kiểm tra điều kiện Win
		private void checkWin()
		{
			CheckerBoard checkerBoard = new CheckerBoard(CheckersGrid, null, turn);
			if (checkerBoard.getListMoves().Count == 0)
				if (turn == "Red")
				{
					if (MessageBox.Show("Bên đen thắng, Chơi lại ?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
						NewGame();
					else
						this.Close();
				}
				else
				{
					if (MessageBox.Show("Bên đỏ thắng, Chơi lại ?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
						NewGame();
					else
						this.Close();
				}


		}
		// Kiểm tra khả năng đi quân
		private int CheckMove()
		{
			if (currentMove.piece1 == null || currentMove.piece2 == null)
				return 0;
			StackPanel stackPanelOld = (StackPanel)GetGridElement(CheckersGrid, currentMove.piece1.Row, currentMove.piece1.Column);
			StackPanel stackPanelNew = (StackPanel)GetGridElement(CheckersGrid, currentMove.piece2.Row, currentMove.piece2.Column);
			Button buttonOld = (Button)stackPanelOld.Children[0];
			Button buttonNew = (Button)stackPanelNew.Children[0];
			stackPanelOld.Background = Brushes.White;
			stackPanelNew.Background = Brushes.White;
			var colorBrush = new ImageBrush();
			// Đổi lại màu background
			if (buttonOld.Name.Contains("KingBlack"))
			{
				colorBrush.ImageSource = new BitmapImage(new Uri("Resource/KingB.png", UriKind.Relative));
				buttonOld.Background = colorBrush;
			}
			else
            if (buttonOld.Name.Contains("Black"))
			{		
				colorBrush.ImageSource = new BitmapImage(new Uri("Resource/Black.png", UriKind.Relative));
				buttonOld.Background = colorBrush;
			}
			
			// Xử lí trường hợp chọn quân k đúng lượt
			if ((turn == "Black") && (buttonOld.Name.Contains("Red")))
			{
				currentMove.piece1 = null;
				currentMove.piece2 = null;
				MessageBox.Show("Lượt bên Đen đi", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
				return 0;
			}
			if ((turn == "Red") && (buttonOld.Name.Contains("Black")))
			{
				currentMove.piece1 = null;
				currentMove.piece2 = null;
				MessageBox.Show("Lượt bên Đỏ đi", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
				return 0;
			}
			// Xử lí trường hợp click cùng 1 quân
			if (buttonOld.Equals(buttonNew))
			{
				currentMove.piece1 = null;
				currentMove.piece2 = null;
				System.Console.WriteLine("Click cung quan");
				return 0;

			}
			// Xét khả năng ăn quân của từng màu quân
			if (buttonOld.Name.Contains("Black"))
			{
				return CheckMoveBlack(currentMove,buttonOld,buttonNew);
			}
			else if (buttonOld.Name.Contains("Red"))
			{
				return CheckMoveRed(currentMove, buttonOld, buttonNew);
			}
			else
			{
				currentMove.piece1 = null;
				currentMove.piece2 = null;
				Console.WriteLine("False");
				return 0;
			}
			//return true;

		}
		// Kiểm tra khả năng đi của Đỏ
		private int CheckMoveRed(Move move, Button buttonOld, Button buttonNew)
		{
			if (buttonOld.Name.Contains("KingRed"))
			{
				Piece piece = move.checkJump("King");
				if (buttonNew.Name.Contains("Red") || buttonNew.Name.Contains("Black"))
				{
					System.Console.WriteLine("KingRed Click o co quan");
					return 0;
				}

				if (move.isAdjacent("King"))
				{
					return 1;
				}
				if (piece != null)
				{
					StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, piece.Row, piece.Column);
					Button button = (Button)stackPanel.Children[0];
					if (button.Name.Contains("Black"))
						return 3;
					else
					{
						System.Console.WriteLine("KingRed an quan do");
						return 0;
					}
						
				}
			}
			if (buttonOld.Name.Contains("Red"))
			{
				Piece piece = move.checkJump("Red");
				if (buttonNew.Name.Contains("Black") || buttonNew.Name.Contains("Red"))
				{
					System.Console.WriteLine("Quan thuong click o co quan");
					return 0;
				}
				if (move.isAdjacent("Red"))
				{
					return 1;
				}
				if (piece != null)
				{
					StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, piece.Row, piece.Column);
					Button button = (Button)stackPanel.Children[0];
					if (button.Name.Contains("Black"))
						return 2;
					else
					{
						System.Console.WriteLine("Quan thuong an quan do");
						return 0;
					}
					
				}
			}

			return 0;
		}
		// Kiểm tra khả năng đi của Đen
		private int CheckMoveBlack(Move move, Button buttonOld, Button buttonNew)
		{
			if (buttonNew.Name.Contains("Red") || buttonNew.Name.Contains("Black"))
				return 0;
			if (buttonOld.Name.Contains("KingBlack"))
			{
				Piece piece = move.checkJump("King");
				if (buttonNew.Name.Contains("Red") || buttonNew.Name.Contains("Black"))
					return 0;
				if (move.isAdjacent("King"))
				{
					return 1;
				}
				if (piece != null)
				{
					StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, piece.Row, piece.Column);
					Button button = (Button)stackPanel.Children[0];
					if (button.Name.Contains("Red"))
						return 3;
					else
						return 0;
				}
			}
			if (buttonOld.Name.Contains("Black"))
			{
				Piece piece = move.checkJump("Black");
				if (buttonNew.Name.Contains("Red") || buttonNew.Name.Contains("Black"))
					return 0;
				if (move.isAdjacent("Black"))
				{
					return 1;
				}
				if (piece != null)
				{
					StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, piece.Row, piece.Column);
					Button button = (Button)stackPanel.Children[0];
					if (button.Name.Contains("Red"))
						return 2;
					else
						return 0;
				}
			}
			return 0;
		}
		// Xử lí đi quân
		// 0 là bỏ
		// 1 là đi quân 
		// 2 là quân thường ăn
		// 3 là King ăn
		private void MakeMove(int status)
		{
			// Không đi
			if (status == 0)
			{
				currentMove = null;
				return;
			}

			StackPanel stackPanelOld = (StackPanel)GetGridElement(CheckersGrid, currentMove.piece1.Row, currentMove.piece1.Column);
			StackPanel stackPanelNew = (StackPanel)GetGridElement(CheckersGrid, currentMove.piece2.Row, currentMove.piece2.Column);
			CheckersGrid.Children.Remove(stackPanelOld);
			CheckersGrid.Children.Remove(stackPanelNew);
			Button buttonNew = (Button)stackPanelOld.Children[0];
			if (buttonNew.Name.Contains("Red") && currentMove.piece2.Row == 7)
			{
				var redBrush = new ImageBrush();
				redBrush.ImageSource = new BitmapImage(new Uri("Resource/KingR.png", UriKind.Relative));
				buttonNew.Name = "buttonKingRed" + currentMove.piece2.Row + currentMove.piece2.Column;
				buttonNew.Background = redBrush;
			}
			if (buttonNew.Name.Contains("Black") && currentMove.piece2.Row == 0)
			{
				var redBrush = new ImageBrush();
				redBrush.ImageSource = new BitmapImage(new Uri("Resource/KingB.png", UriKind.Relative));
				buttonNew.Name = "buttonKingBlack" + currentMove.piece2.Row + currentMove.piece2.Column;
				buttonNew.Background = redBrush;
			}
			// Đi quân
			if (status == 1)
			{
				// Đảo trạng thái 2 ô
				Grid.SetRow(stackPanelOld, currentMove.piece2.Row);
				Grid.SetColumn(stackPanelOld, currentMove.piece2.Column);
				CheckersGrid.Children.Add(stackPanelOld);
				Grid.SetRow(stackPanelNew, currentMove.piece1.Row);
				Grid.SetColumn(stackPanelNew, currentMove.piece1.Column);
				CheckersGrid.Children.Add(stackPanelNew);
				// Đặt lại lượt đi
				currentMove = null;
				if (turn == "Black")
				{
					turn = "Red";
					this.Title = "Bên Đỏ đi";
				}
				else if (turn == "Red")
				{
					turn = "Black";
					this.Title = "Bên Đen đi";
				}
			}
			// Ăn quân
			if (status > 1)
			{
				String king = turn;
				if (status == 3)
					king = "King";
				Grid.SetRow(stackPanelOld, currentMove.piece2.Row);
				Grid.SetColumn(stackPanelOld, currentMove.piece2.Column);
				CheckersGrid.Children.Add(stackPanelOld);
				Grid.SetRow(stackPanelNew, currentMove.piece1.Row);
				Grid.SetColumn(stackPanelNew, currentMove.piece1.Column);
				CheckersGrid.Children.Add(stackPanelNew);
				Piece piece = currentMove.checkJump(king);
				StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, piece.Row, piece.Column);
				Button button = (Button)stackPanel.Children[0];
				button.Background = Brushes.White;
				button.Name = "button" + piece.Row + piece.Column;
				CheckersGrid.Children.Remove(stackPanel);
				Grid.SetRow(stackPanel, piece.Row);
				Grid.SetColumn(stackPanel, piece.Column);
				CheckersGrid.Children.Add(stackPanel);
				//stackPanel.Children.Add(button);
				// Đặt lại lượt đi
				currentMove = null;
				if (turn == "Black")
				{
					red--;
					turn = "Red";
					this.Title = "Bên Đỏ đi";
				}
				else if (turn == "Red")
				{
					black--;
					turn = "Black";
					this.Title = "Bên Đen đi";
				}
			}	
		}

		UIElement GetGridElement(Grid g, int r, int c)
		{
			for (int i = 0; i < g.Children.Count; i++)
			{
				UIElement e = g.Children[i];
				if (Grid.GetRow(e) == r && Grid.GetColumn(e) == c)
					return e;
			}
			return null;
		}

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.ClearBoard();
            this.MakeBoard();
        }
    }
}
