using System;
using System.Collections.Generic;
using System.Linq;

namespace Task4Softeq
{
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

            Board board = new(N, M, true);
            Board finalBoard = new(M, N, false);
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

            bool CanTransFromLeftNearest = board.CanTransFromLeftNearest();
            if (CanTransFromLeftNearest)
            {
                TransFormedFromLeftNearest = board.TransFormLeftNearest();
            }
            if (CanTransFromLeftNearest && TransFormedFromLeftNearest.Equals(finalBoard))
            {
                road.Add(new BoardNode(board, TransFormAction.CanTransFromLeftNearest));
                return;
            }
            bool CanTransFromLeftThroughOne = board.CanTransFromLeftThroughOne();
            if (CanTransFromLeftThroughOne)
            {
                TransFormedFromLeftThroughOne = board.TransFormLeftThroughOne();
            }
            if (CanTransFromLeftThroughOne && TransFormedFromLeftThroughOne.Equals(finalBoard))
            {
                road.Add(new BoardNode(board, TransFormAction.CanTransFromLeftThroughOne));
                return;
            }
            bool CanTransFromRightNearest = board.CanTransFromRightNearest();
            if (CanTransFromRightNearest)
            {
                TransFormedFromRightNearest = board.TransFormRightNearest();
            }
            if (CanTransFromRightNearest && TransFormedFromRightNearest.Equals(finalBoard))
            {
                road.Add(new BoardNode(board, TransFormAction.CanTransFromRightNearest));
                return;
            }
            bool CanTransFromRightThroughOne = board.CanTransFromRightThroughOne();
            if (CanTransFromRightThroughOne)
            {
                TransFormedFromRightThroughOne = board.TransFormRightThroughOne();
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
                            if ((!winnngRoutes[count][i].Board.CanTransFromRightThroughOne()) ||
                                (!winnngRoutes[count][i].Board.TransFormRightThroughOne().Equals(winnngRoutes[count][i - 1].Board)))
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
                            if ((!winnngRoutes[count][i].Board.CanTransFromRightNearest()) ||
                                (!winnngRoutes[count][i].Board.TransFormRightNearest().Equals(winnngRoutes[count][i - 1].Board)))
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
                            if ((!winnngRoutes[count][i].Board.CanTransFromLeftThroughOne()) ||
                                (!winnngRoutes[count][i].Board.TransFormLeftThroughOne().Equals(winnngRoutes[count][i - 1].Board)))
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
                            if ((!winnngRoutes[count][i].Board.CanTransFromLeftNearest()) ||
                                (!winnngRoutes[count][i].Board.TransFormLeftNearest().Equals(winnngRoutes[count][i - 1].Board)))
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