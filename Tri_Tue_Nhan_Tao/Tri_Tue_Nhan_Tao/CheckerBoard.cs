using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Tri_Tue_Nhan_Tao
{
	class CheckerBoard
	{
		public Grid checkerGrid; // trạng thái bàn cờ
		public Move moveCheckerBoard; // Move tạo ra nó
		public String team; // Lượt đi tiếp theo thuộc về quân nào

		public int[,] checkerBoard = new int[,] {
			{-1,-1,-1,-1,-1,-1,-1,-1},
			{-1,-1,-1,-1,-1,-1,-1,-1},
			{-1,-1,-1,-1,-1,-1,-1,-1},
			{-1,-1,-1,-1,-1,-1,-1,-1},
			{-1,-1,-1,-1,-1,-1,-1,-1},
			{-1,-1,-1,-1,-1,-1,-1,-1},
			{-1,-1,-1,-1,-1,-1,-1,-1},
			{-1,-1,-1,-1,-1,-1,-1,-1}
		};
		public int[,] listPointRed = new int[,] {
            {1,0,1,0,1,0,1,0},
            {0,2,0,2,0,2,0,4},
            {7,0,5,0,5,0,5,0},
            {0,6,0,6,0,6,0,8},
            {13,0,11,0,11,0,11,0},
            {0,16,0,16,0,16,0,18},
            {24,0,22,0,22,0,22,0},
            {0,29,0,29,0,29,0,40}
        };
		public CheckerBoard()
		{
			checkerGrid = null;
			moveCheckerBoard = null;
		}
		public CheckerBoard(Grid grid1, Move move1, String team1)
		{
			this.team = team1;
			this.checkerGrid = grid1;
			this.moveCheckerBoard = move1;
			MakeCheckerBoard();
		}
		public CheckerBoard(CheckerBoard checkerBoard1)
		{
			this.team = checkerBoard1.getTeam();
			this.checkerGrid = checkerBoard1.getGrid();
			this.moveCheckerBoard = checkerBoard1.getMoveCheckerBoard();
			MakeCheckerBoard();
		}
		// -1 ô không thể đi
		// 0 ô trống
		// 1 quân đỏ
		// 2 quân đen
		// 3 vua đỏ
		// 4 vua đen
		public void MakeCheckerBoard()
		{
			for (int row = 0; row < 8; row++)
				for (int col = 0; col < 8; col++)
				{
					if ((row + col) % 2 == 0)
					{
						int value = 0;
						StackPanel stackPanel = (StackPanel)GetGridElement(checkerGrid, row, col);
						Button button = (Button)stackPanel.Children[0];
						if (button.Name.Contains("KingRed"))
							value = 3;
						else
						if (button.Name.Contains("KingBlack"))
							value = 4;
						else
						if (button.Name.Contains("Red"))
							value = 1;
						else
						if (button.Name.Contains("Black"))
							value = 2;
						else
							value = 0;
						checkerBoard[row, col] = value;
					}
				}
		}
		public void SetState(int r, int c, int value)
		{
			if ((value > 4) || (value < -1))
				return;
			checkerBoard[r, c] = value;
			return;
		}
		public int GetState(int r, int c)
		{
			if ((r > 7) || (r < 0) || (c > 7) || (c < 0))
				return -1;
			return checkerBoard[r, c];
		}
		// Tạo danh sách nước có thể đi
		public List<Move> getListMoves()
		{
			List<Move> list = new List<Move>();
			if (getTeam() == "Red")
			{
				for (int r = 0; r < 8; r++)
					for (int c = 0; c < 8; c++)
					{
						if (GetState(r, c) == 3)
						{
							if ((GetState(r - 2, c - 2) == 0) && ((GetState(r - 1, c - 1) == 2) || (GetState(r - 1, c - 1) == 4)))
							{
								list.Add(new Move(new Piece(r, c), new Piece(r - 2, c - 2)));
							}
							if ((GetState(r - 2, c + 2) == 0) && ((GetState(r - 1, c + 1) == 2) || (GetState(r - 1, c + 1) == 4)))
							{
								list.Add(new Move(new Piece(r, c), new Piece(r - 2, c + 2)));
							}
							if ((GetState(r + 2, c - 2) == 0) && ((GetState(r + 1, c - 1) == 2) || (GetState(r + 1, c - 1) == 4)))
							{
								list.Add(new Move(new Piece(r, c), new Piece(r + 2, c - 2)));
							}
							if ((GetState(r + 2, c + 2) == 0) && ((GetState(r + 1, c + 1) == 2) || (GetState(r + 1, c + 1) == 4)))
							{
								list.Add(new Move(new Piece(r , c), new Piece(r + 2, c + 2)));
							}
							if (GetState(r - 1, c + 1) == 0)
								list.Add(new Move(new Piece(r, c), new Piece(r - 1, c + 1)));
							if (GetState(r - 1, c - 1) == 0)
								list.Add(new Move(new Piece(r, c), new Piece(r - 1, c - 1)));
							if (GetState(r + 1, c + 1) == 0)
								list.Add(new Move(new Piece(r, c), new Piece(r + 1, c + 1)));
							if (GetState(r + 1, c - 1) == 0)
								list.Add(new Move(new Piece(r, c), new Piece(r + 1, c - 1)));
						}
						if (GetState(r, c) == 1)
						{
							if ((GetState(r + 2, c - 2) == 0) && ((GetState(r + 1, c - 1) == 2) || (GetState(r + 1, c - 1) == 4)))
							{
								list.Add(new Move(new Piece(r , c), new Piece(r + 2, c - 2)));
							}
							if ((GetState(r + 2, c + 2) == 0) && ((GetState(r + 1, c + 1) == 2) || (GetState(r + 1, c + 1) == 4)))
							{
								list.Add(new Move(new Piece(r , c), new Piece(r + 2, c + 2)));
							}
							if (GetState(r + 1, c + 1) == 0)
								list.Add(new Move(new Piece(r, c), new Piece(r + 1, c + 1)));
							if (GetState(r + 1, c - 1) == 0)
								list.Add(new Move(new Piece(r, c), new Piece(r + 1, c - 1)));
						}
					}
			}
			if (getTeam() == "Black")
			{
				for (int r = 0; r < 8; r++)
					for (int c = 0; c < 8; c++)
					{
						if (GetState(r, c) == 4)
						{
							if ((GetState(r - 2, c - 2) == 0) && ((GetState(r - 1, c - 1) == 1) || (GetState(r - 1, c - 1) == 3)))
							{
								list.Add(new Move(new Piece(r , c), new Piece(r - 2, c - 2)));
							}
							if ((GetState(r - 2, c + 2) == 0) && ((GetState(r - 1, c + 1) == 1) || (GetState(r - 1, c + 1) == 3)))
							{
								list.Add(new Move(new Piece(r , c), new Piece(r - 2, c + 2)));
							}
							if ((GetState(r + 2, c - 2) == 0) && ((GetState(r + 1, c - 1) == 1) || (GetState(r + 1, c - 1) == 3)))
							{
								list.Add(new Move(new Piece(r , c), new Piece(r + 2, c - 2)));
							}
							if ((GetState(r + 2, c + 2) == 0) && ((GetState(r + 1, c + 1) == 1) || (GetState(r + 1, c + 1) == 3)))
							{
								list.Add(new Move(new Piece(r , c), new Piece(r + 2, c + 2)));
							}
							if (GetState(r - 1, c + 1) == 0)
								list.Add(new Move(new Piece(r, c), new Piece(r - 1, c + 1)));
							if (GetState(r - 1, c - 1) == 0)
								list.Add(new Move(new Piece(r, c), new Piece(r - 1, c - 1)));
							if (GetState(r + 1, c + 1) == 0)
								list.Add(new Move(new Piece(r, c), new Piece(r + 1, c + 1)));
							if (GetState(r + 1, c - 1) == 0)
								list.Add(new Move(new Piece(r, c), new Piece(r + 1, c - 1)));
						}
						if (GetState(r, c) == 2)
						{
							if ((GetState(r - 2, c - 2) == 0) && ((GetState(r - 1, c - 1) == 1) || (GetState(r - 1, c - 1) == 3)))
							{
								list.Add(new Move(new Piece(r , c), new Piece(r - 2, c - 2)));
							}
							if ((GetState(r - 2, c + 2) == 0) && ((GetState(r - 1, c + 1) == 1) || (GetState(r - 1, c + 1) == 3)))
							{
								list.Add(new Move(new Piece(r , c), new Piece(r - 2, c + 2)));
							}
							if (GetState(r - 1, c + 1) == 0)
								list.Add(new Move(new Piece(r, c), new Piece(r - 1, c + 1)));
							if (GetState(r - 1, c - 1) == 0)
								list.Add(new Move(new Piece(r, c), new Piece(r - 1, c - 1)));
						}
					}
			}
			return list;
		}
		// Lấy điểm trạng thái bàn cờ
		public int getPoint()
		{
			int sumPoint = 0;
			int black = 0;
			for (int row = 0; row < 8; row++)
				for (int col = 0; col < 8; col++)
					if ((row + col) % 2 == 0)
					{
						if (checkerBoard[row, col] == 3)
							sumPoint += 30-listPointRed[row,col];
						else
						if (checkerBoard[row, col] == 1)
							sumPoint += listPointRed[row, col];
						else
						if (checkerBoard[row, col] == 2 || checkerBoard[row, col] == 4)
							black++;
					}
			return sumPoint + 10*(12-black);
		}
		public int getPointBlack()
		{
			int sumPoint = 0;
			int red = 0;
			for (int row = 0; row < 8; row++)
				for (int col = 0; col < 8; col++)
					if ((row + col) % 2 == 0)
					{
						if (checkerBoard[row, col] == 4)
							sumPoint += 80 - listPointRed[row, col];
						else
						if (checkerBoard[row, col] == 2)
							sumPoint += listPointRed[row, col];
						else
						if (checkerBoard[row, col] == 2 || checkerBoard[row, col] == 4)
							red++;
					}
			return -(sumPoint + 10 * (12 - red));
		}
		// Lấy, Đặt Move tạo ra bàn cờ
		public Move getMoveCheckerBoard()
		{
			return this.moveCheckerBoard;
		}
		public void setMoveCheckerBoard(Move move)
		{
			this.moveCheckerBoard = move;
		}
		// Lấy, Đặt lại lượt đi quân
		public String getTeam()
		{
			return this.team;
		}
		public void setTeam(String team)
		{
			this.team = team;
		}
		public int[,] getCheckerBoard()
		{
			return this.checkerBoard;
		}
		public Grid getGrid()
		{
			return this.checkerGrid;
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
	}
}
