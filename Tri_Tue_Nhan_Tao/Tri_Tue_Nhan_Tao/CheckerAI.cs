using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Tri_Tue_Nhan_Tao
{
	class CheckerAI
	{
		public CheckerBoard baseCheckerBoard;
		public String team;


		public CheckerAI()
		{
			team = null;
			baseCheckerBoard = null;
		}
		public CheckerAI(CheckerBoard checkerBoard)
		{
			this.baseCheckerBoard = checkerBoard;
			this.baseCheckerBoard.setTeam(checkerBoard.getTeam());
			this.team = checkerBoard.getTeam();
		}
		// Đi quân
		public void MakeMove(Move move, CheckerBoard checkerBoard) // Move, Trạng thái bàn cờ 
		{
			String color = checkerBoard.getTeam();
			int k;
			k = checkerBoard.GetState(move.piece1.Row, move.piece1.Column);
			if (k == 3 || k == 4)
				color = "King";
			checkerBoard.SetState(move.piece1.Row, move.piece1.Column, checkerBoard.GetState(move.piece2.Row, move.piece2.Column));
			checkerBoard.SetState(move.piece2.Row, move.piece2.Column, k);
			Piece piece = move.checkJump(color);
			if (piece != null)
			{
				checkerBoard.SetState(piece.Row, piece.Column, 0);
			}
			if (checkerBoard.getTeam() == "Red")
				checkerBoard.setTeam("Black");
			else
				checkerBoard.setTeam("Red");
		}
		// Tìm nước đi tốt nhất
		public Move findbestMove()
		{
			Move best = new Move();
			int max = -10000;
			foreach (Move move in baseCheckerBoard.getListMoves())
			{
				CheckerBoard checkerBoard1 = new CheckerBoard(baseCheckerBoard);
				MakeMove(move, checkerBoard1);
				if (max <= MiniMax(checkerBoard1, 1))
				{
					max = MiniMax(checkerBoard1, 1);
					best = move;
				}
			}
			return best;
		}
		// Lấy điểm cho mỗi nước đi bằng giải thuật MiniMax
		public int MiniMax(CheckerBoard checkerBoard, int depth)
		{
			int value = 0, best;
			if (depth >= 3|| checkerBoard.getListMoves() == null)
				return checkerBoard.getPoint();
			else
			{
				best = 0;
				if (checkerBoard.getTeam() == "Red")
					best = -10000;
				else
					best = 10000;
				foreach (Move move in checkerBoard.getListMoves())
				{
					CheckerBoard checkerBoard1 = new CheckerBoard(checkerBoard);
					MakeMove(move, checkerBoard1);
					int depth2 = depth + 1;
					value = MiniMax(checkerBoard1, depth2);
					if (checkerBoard.getTeam() == "Red")
						if (value >= best)
							best = value;
					if (checkerBoard.getTeam() == "Black")
						if (value <= best)
							best = value;
				}
				return best;
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
	}
}
