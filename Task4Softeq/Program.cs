using System;
using System.Collections.Generic;
using System.Linq;

namespace Task4Softeq
{
    class Program
    {
        static void Main(string[] args)
        {
            #region input and define

            ushort N = 10;
            ushort M = 15;

            List<BoardNode> road = new();

            Board initialBoard = new(N, M, true);
            //изначально всегда можно сделать четыре ход двумя ближайшими к пробелу или же через одну черными или белыми 
            //легко показать что первый ход через одну всегда проигрышный, так как после него можно ходить только фигурами этого же цвета,
            //а фигуры противоположенного становятся недоступны после чего пробел всегда смещается в край и мы
            //получаем доски вида WWWWBBBBBX или XWWWWWBBBBB

            //т.е. первый ход всегда будет ближайшей черной или белой
            //также задача определенна симетрична, поэтому не теряя общности будем считать что всегда первый ход делаем белыми
            //так случай когда первый ход черные переформулируетя из N, M, true в M, N, true
            road.Add(new BoardNode(initialBoard, TransFormAction.TransFormLeftNearest));
            Board initialBoardAfterFirstTurn = initialBoard.TransFormLeftNearest();

            Board finalBoard = new(M, N, false);
            List<List<BoardNode>> winnngRoutes = new();
            #endregion

            int res = CountMinTurns(N, M, ref initialBoard, ref initialBoardAfterFirstTurn, ref finalBoard, ref road, ref winnngRoutes);

            Console.WriteLine(res);
            return;
        }

        private static void GetTransformsRoad(Board board, Board finalBoard, ref List<BoardNode> road)
        {
            Board transFormedFromLeftNearest = null;
            Board transFormedFromRightNearest = null;

            #region check for return from recursion
            bool canTransFromLeftNearest = board.CanTransFromLeftNearest();
            if (canTransFromLeftNearest)
            {
                transFormedFromLeftNearest = board.TransFormLeftNearest();
            }
            if (canTransFromLeftNearest && transFormedFromLeftNearest.Equals(finalBoard))
            {
                road.Add(new BoardNode(board, TransFormAction.CanTransFromLeftNearest));
                return;
            }
            bool canTransFromRightNearest = board.CanTransFromRightNearest();
            if (canTransFromRightNearest)
            {
                transFormedFromRightNearest = board.TransFormRightNearest();
            }
            if (canTransFromRightNearest && transFormedFromRightNearest.Equals(finalBoard))
            {
                road.Add(new BoardNode(board, TransFormAction.CanTransFromRightNearest));
                return;
            }
            #endregion

            if (canTransFromLeftNearest && transFormedFromLeftNearest.CanMoveItemsThroughSpace())
            {
                road.Add(new BoardNode(board, TransFormAction.TransFormLeftNearest));
                GetTransformsRoad(transFormedFromLeftNearest, finalBoard, ref road);
            }
            if (canTransFromRightNearest && transFormedFromRightNearest.CanMoveItemsThroughSpace())
            {
                road.Add(new BoardNode(board, TransFormAction.TransFormRightNearest));
                GetTransformsRoad(transFormedFromRightNearest, finalBoard, ref road);
            }
            if (board.CanTransFromLeftThroughOne())
            {
                Board TransFormedFromLeftThroughOne = board.TransFormLeftThroughOne();
                if (TransFormedFromLeftThroughOne.CanMoveItemsThroughSpace())
                {
                    road.Add(new BoardNode(board, TransFormAction.TransFormLeftThroughOne));
                    GetTransformsRoad(TransFormedFromLeftThroughOne, finalBoard, ref road);
                }
            }
            if (board.CanTransFromRightThroughOne())
            {
                Board TransFormedFromRightThroughOne = board.TransFormRightThroughOne();
                if (TransFormedFromRightThroughOne.CanMoveItemsThroughSpace())
                {
                    road.Add(new BoardNode(board, TransFormAction.TransFormRightThroughOne));
                    GetTransformsRoad(TransFormedFromRightThroughOne, finalBoard, ref road);
                }
            }
        }

        private static int CountMinTurns(ushort N, ushort M, ref Board initialboard, ref Board initialBoardAfterFirstTurn, ref Board finalBoard,
          ref List<BoardNode> road, ref List<List<BoardNode>> winnngRoutes)
        {
            GetTransformsRoad(initialBoardAfterFirstTurn, finalBoard, ref road);
            road.Reverse();

            GetWinnngRoutes(ref road, ref winnngRoutes);
            ClearWinnngRoutes(ref winnngRoutes);
            
            foreach (List<BoardNode> el in winnngRoutes)
            {
                for (int i = 0; i < finalBoard.space; i++)
                {
                    Console.Write(finalBoard.fields[i].ToString() + " ");
                }
                Console.Write("       ");
                for (int i = finalBoard.space; i < finalBoard.fields.Length; i++)
                {
                    Console.Write(finalBoard.fields[i].ToString() + " ");
                }
                Console.WriteLine();
                foreach (var pos in el)
                {
                    for(int i = 0; i < pos.Board.space; i ++)
                    {
                        Console.Write(pos.Board.fields[i].ToString() + " ");
                    }
                    Console.Write("       ");
                    for (int i = pos.Board.space; i < pos.Board.fields.Length; i++)
                    {
                        Console.Write(pos.Board.fields[i].ToString() + " ");
                    }
                    Console.WriteLine();
                }
            }            

            foreach (List<BoardNode> el in winnngRoutes)
            {
                if (initialboard.Equals(el.Last().Board))
                {
                    return el.Count;
                }
            }

            return 0;
        }

        private static void GetWinnngRoutes(ref List<BoardNode> road, ref List<List<BoardNode>> winnngRoutes)
        {
            int i = 0;
            while (i < road.Count)
            {
                if (road[i].CanTransformToFinal())
                {
                    var newWinnngRoute = new List<BoardNode>();
                    do
                    {
                          newWinnngRoute.Add(road[i]);
                          i++;
                    }
                    while ((i < road.Count) && (!road[i].CanTransformToFinal()));
                    winnngRoutes.Add(newWinnngRoute);
                }
                i++;
            } 
        }

        private static void ClearWinnngRoutes(ref List<List<BoardNode>> winnngRoutes)
        {
            for (int count = 0; count < winnngRoutes.Count; count++)
            {
                int i = 1;
                do
                {
                    switch (winnngRoutes[count][i].TransformAction)
                    {
                        case TransFormAction.TransFormRightThroughOne:
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