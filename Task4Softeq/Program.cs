using System;
using System.Collections.Generic;
using System.Linq;

namespace Task4Softeq
{
    class Board
    {
        public bool[] Fields;
        public int Space { get; set; }
        public static bool CanTransFromLeftNearest(Board Board) => Board.Space != 0 && Board.Fields[Board.Space - 1];
        public static Board TransFormLeftNearest(Board Board)
        {
            Board transformed = new(Board);
            transformed.Space--;

            return transformed;
        }
        public static bool CanTransFromLeftThroughOne(Board Board) => Board.Space > 1 && Board.Fields[Board.Space - 2];
        public static Board TransFormLeftThroughOne(Board Board)
        {
            Board transformed = new(Board);
            bool temp = transformed.Fields[transformed.Space - 1];
            transformed.Fields[transformed.Space - 1] = transformed.Fields[transformed.Space - 2];
            transformed.Fields[transformed.Space - 2] = temp;
            transformed.Space -= 2;

            return transformed;
        }
        public static bool CanTransFromRightNearest(Board Board) => Board.Space < Board.Fields.Length && !Board.Fields[Board.Space];
        public static Board TransFormRightNearest(Board Board)
        {
            Board transformed = new(Board);
            transformed.Space++;

            return transformed;
        }
        public static bool CanTransFromRightThroughOne(Board Board) => Board.Space < Board.Fields.Length - 1 && !Board.Fields[Board.Space + 1];
        public static Board TransFormRightThroughOne(Board Board)
        {
            Board transformed = new(Board);
            bool temp = transformed.Fields[transformed.Space];
            transformed.Fields[transformed.Space] = transformed.Fields[transformed.Space + 1];
            transformed.Fields[transformed.Space + 1] = temp;
            transformed.Space += 2;

            return transformed;
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType()) return false;

            Board board = (Board)obj;

            if (Fields.Length != board.Fields.Length)
            {
                return false;
            }

            for (int i = 0; i < Fields.Length; i++)
            {
                if (Fields[i] != board.Fields[i])
                {
                    return false;
                }
            }

            return Space == board.Space;
        }
        public Board(int N, int M, bool isWhite)
        {
            Space = N;
            Fields = new bool[N + M];
            for (int i = 0; i < N; i++)
            {
                Fields[i] = isWhite;
            }

            for (int i = N; i < M + N; i++)
            {
                Fields[i] = !isWhite;
            }
        }
        public Board(Board board)
        {
            Space = board.Space;
            Fields = new bool[board.Fields.Length];
            for (int i = 0; i < board.Fields.Length; i++)
            {
                Fields[i] = board.Fields[i];
            }
        }
    }
    class BoardNode
    {
        public Board Board { get; set; }
        public TransFormAction TransformAction { get; set; }
        public BoardNode(Board board, TransFormAction transformAction)
        {
            Board = board;
            TransformAction = transformAction;
        }
        public bool CanTransform() => TransformAction == TransFormAction.CanTransFromRightNearest ||
            TransformAction == TransFormAction.CanTransFromLeftNearest ||
            TransformAction == TransFormAction.CanTransFromLeftThroughOne ||
            TransformAction == TransFormAction.CanTransFromRightThroughOne;
    }
    class Program
    {
        static void Main(string[] args)
        {
            #region input and define

            ushort N = 8;
            ushort M = 10;

            Board board = new (N, M, true);
            Board finalBoard = new (M, N, false);
            List<BoardNode> road = new();
            List<List<BoardNode>> winnngRoutes = new();
            #endregion

            #region CountMinTurns function f(N, M)
            //   We can calculate res directly, but performance is poor
            int res = CountMinTurns(N, M, ref board, ref finalBoard, ref road, ref winnngRoutes);

            // easy to notice 
            // f(N, M) = f(M, N)                        (I)   symmetrical definition - function is even
            // f(N + 1, M) = f(N, M) + M + 1            (II)  mathematical induction

            //res = CountMinTurns(1, 1, ref board, ref finalBoard, ref road, ref winnngRoutes);

            //while (M > 1) {
            //  res += N + 1;
            //  M--;
            //}

            //while (N > 1) {
            //  res += M + 1;
            //  N--;
            //}
            #endregion

            Console.WriteLine(res);
            return;
        }

        private static void GetTransformsRoad(Board board, Board finalBoard, ref List<BoardNode> road)
        {
            Board TransFormedFromLeftNearest = null;
            Board TransFormedFromLeftThroughOne = null;
            Board TransFormedFromRightNearest = null;
            Board TransFormedFromRightThroughOne = null;

            bool CanTransFromLeftNearest = Board.CanTransFromLeftNearest(board);
            if (CanTransFromLeftNearest)
            {
                TransFormedFromLeftNearest = Board.TransFormLeftNearest(board);
            }
            if (CanTransFromLeftNearest && TransFormedFromLeftNearest.Equals(finalBoard))
            {
                road.Add(new BoardNode(board, TransFormAction.CanTransFromLeftNearest));
                return;
            }
            bool CanTransFromLeftThroughOne = Board.CanTransFromLeftThroughOne(board);
            if (CanTransFromLeftThroughOne)
            {
                TransFormedFromLeftThroughOne = Board.TransFormLeftThroughOne(board);
            }
            if (CanTransFromLeftThroughOne && TransFormedFromLeftThroughOne.Equals(finalBoard))
            {
                road.Add(new BoardNode(board, TransFormAction.CanTransFromLeftThroughOne));
                return;
            }
            bool CanTransFromRightNearest = Board.CanTransFromRightNearest(board);
            if (CanTransFromRightNearest)
            {
                TransFormedFromRightNearest = Board.TransFormRightNearest(board);
            }
            if (CanTransFromRightNearest && TransFormedFromRightNearest.Equals(finalBoard))
            {
                road.Add(new BoardNode(board, TransFormAction.CanTransFromRightNearest));
                return;
            }
            bool CanTransFromRightThroughOne = Board.CanTransFromRightThroughOne(board);
            if (CanTransFromRightThroughOne)
            {
                TransFormedFromRightThroughOne = Board.TransFormRightThroughOne(board);
            }
            if (CanTransFromRightThroughOne && TransFormedFromRightThroughOne.Equals(finalBoard))
            {
                road.Add(new BoardNode(board, TransFormAction.CanTransFromRightThroughOne));
                return;
            }

            if (CanTransFromLeftNearest)
            {
                road.Add(new BoardNode(board, TransFormAction.TransFormLeftNearest));
                GetTransformsRoad(TransFormedFromLeftNearest, finalBoard, ref road);
            }
            if (CanTransFromLeftThroughOne)
            {
                road.Add(new BoardNode(board, TransFormAction.TransFormLeftThroughOne));

                GetTransformsRoad(TransFormedFromLeftThroughOne, finalBoard, ref road);
            }
            if (CanTransFromRightNearest)
            {
                road.Add(new BoardNode(board, TransFormAction.TransFormRightNearest));
                GetTransformsRoad(TransFormedFromRightNearest, finalBoard, ref road);
            }
            if (CanTransFromRightThroughOne)
            {
                road.Add(new BoardNode(board, TransFormAction.TransFormRightThroughOne));
                GetTransformsRoad(TransFormedFromRightThroughOne, finalBoard, ref road);
            }
        }
        private static int CountMinTurns(ushort N, ushort M, ref Board board, ref Board finalBoard,
          ref List<BoardNode> road, ref List<List<BoardNode>> winnngRoutes)
        {
            road.Clear();
            GetTransformsRoad(board, finalBoard, ref road);
            road.Reverse();

            GetWinnngRoutes(ref road, ref winnngRoutes);
            ClearWinnngRoutes(ref road, ref winnngRoutes);

            foreach (List<BoardNode> el in winnngRoutes)
            {
                if (board.Equals(el.Last().Board))
                {
                    return el.Count;
                }
            }

            return 0;
        }
        private static void GetWinnngRoutes(ref List<BoardNode> road, ref List<List<BoardNode>> winnngRoutes)
        {
            winnngRoutes.Clear();

            for (int i = 0; i < road.Count; i++)
            {
                if (road[i].CanTransform())
                {
                    winnngRoutes.Add(new List<BoardNode>());
                    int j = i;
                    winnngRoutes.Last().Add(road[j]);
                    j++;
                    while ((j < road.Count) && (!road[j].CanTransform()))
                    {
                        winnngRoutes.Last().Add(road[j]);
                        j++;
                    }
                }
            }
        }
        private static void ClearWinnngRoutes(ref List<BoardNode> road, ref List<List<BoardNode>> winnngRoutes)
        {
            for (int count = 0; count < winnngRoutes.Count; count++)
            {
                int i = 1;
                do
                {
                    switch (winnngRoutes[count][i].TransformAction)
                    {
                        case TransFormAction.TransFormRightThroughOne:
                        case TransFormAction.CanTransFromRightThroughOne:
                            if ((!Board.CanTransFromRightThroughOne(winnngRoutes[count][i].Board)) ||
                                (!Board.TransFormRightThroughOne(winnngRoutes[count][i].Board).Equals(winnngRoutes[count][i - 1].Board)))
                            {
                                winnngRoutes[count].RemoveAt(i);
                            }
                            else
                            {
                                i++;
                            }
                            break;
                        case TransFormAction.TransFormRightNearest:
                        case TransFormAction.CanTransFromRightNearest:
                            if ((!Board.CanTransFromRightNearest(winnngRoutes[count][i].Board)) ||
                                (!Board.TransFormRightNearest(winnngRoutes[count][i].Board).Equals(winnngRoutes[count][i - 1].Board)))
                            {
                                winnngRoutes[count].RemoveAt(i);
                            }
                            else
                            {
                                i++;
                            }
                            break;
                        case TransFormAction.TransFormLeftThroughOne:
                        case TransFormAction.CanTransFromLeftThroughOne:
                            if ((!Board.CanTransFromLeftThroughOne(winnngRoutes[count][i].Board)) ||
                                (!Board.TransFormLeftThroughOne(winnngRoutes[count][i].Board).Equals(winnngRoutes[count][i - 1].Board)))
                            {
                                winnngRoutes[count].RemoveAt(i);
                            }
                            else
                            {
                                i++;
                            }
                            break;
                        case TransFormAction.TransFormLeftNearest:
                        case TransFormAction.CanTransFromLeftNearest:
                            if ((!Board.CanTransFromLeftNearest(winnngRoutes[count][i].Board)) ||
                                (!Board.TransFormLeftNearest(winnngRoutes[count][i].Board).Equals(winnngRoutes[count][i - 1].Board)))
                            {
                                winnngRoutes[count].RemoveAt(i);
                            }
                            else
                            {
                                i++;
                            }
                            break;
                    }
                }
                while (i < winnngRoutes[count].Count);
            }
        }
    }
}