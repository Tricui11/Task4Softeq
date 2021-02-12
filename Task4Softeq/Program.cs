using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Task4Softeq
{
    class Field
    {
        public bool isWhite { get; set; }
        public Field(bool _isWhite)
        {
            isWhite = _isWhite;
        }
    }

    class Board
    {
        public List<Field> LeftSide;
        public int Space { get; set; }
        public List<Field> RightSide;
        public static bool CanTransFromLeftNearest(Board Board)
        {
            return (Board.LeftSide.Count > 0) && Board.LeftSide[Board.LeftSide.Count - 1].isWhite && (Board.Space == Board.LeftSide.Count);
        }
        public static Board TransFormLeftNearest(Board Board)
        {
            Board Transformed = new Board(Board);
            Transformed.Space--;
            Transformed.RightSide.Insert(0, Transformed.LeftSide[Transformed.LeftSide.Count - 1]);
            Transformed.LeftSide.RemoveAt(Transformed.LeftSide.Count - 1);

            return Transformed;
        }
        public static bool CanTransFromLeftThroughOne(Board Board)
        {
            return (Board.LeftSide.Count > 1) && Board.LeftSide[Board.LeftSide.Count - 2].isWhite && (Board.Space == Board.LeftSide.Count);
        }
        public static Board TransFormLeftThroughOne(Board Board)
        {
            Board Transformed = new Board(Board);
            Transformed.Space -= 2;
            Transformed.RightSide.Insert(0, Transformed.LeftSide[Transformed.LeftSide.Count - 2]);
            Transformed.RightSide.Insert(0, Transformed.LeftSide[Transformed.LeftSide.Count - 1]);
            Transformed.LeftSide.RemoveAt(Transformed.LeftSide.Count - 1);
            Transformed.LeftSide.RemoveAt(Transformed.LeftSide.Count - 1);

            return Transformed;
        }
        public static bool CanTransFromRightNearest(Board Board)
        {
            return (Board.RightSide.Count > 0) && !Board.RightSide[0].isWhite && (Board.Space == Board.LeftSide.Count);
        }
        public static Board TransFormRightNearest(Board Board)
        {
            Board Transformed = new Board(Board);
            Transformed.Space++;
            Transformed.LeftSide.Add(Transformed.RightSide[0]);
            Transformed.RightSide.RemoveAt(0);

            return Transformed;
        }
        public static bool CanTransFromRightThroughOne(Board Board)
        {
            return (Board.RightSide.Count > 1) && !Board.RightSide[1].isWhite && (Board.Space == Board.LeftSide.Count);
        }
        public static Board TransFormRightThroughOne(Board Board)
        {
            Board Transformed = new Board(Board);
            Transformed.Space += 2;
            Transformed.LeftSide.Add(Transformed.RightSide[1]);
            Transformed.LeftSide.Add(Transformed.RightSide[0]);
            Transformed.RightSide.RemoveAt(0);
            Transformed.RightSide.RemoveAt(0);

            return Transformed;
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType()) return false;

            Board board = (Board)obj;

            if ((LeftSide.Count != board.LeftSide.Count) || (RightSide.Count != board.RightSide.Count))
            {
                return false;
            }

            for (int i = 0; i < LeftSide.Count; i++)
            {
                if (LeftSide[i].isWhite != board.LeftSide[i].isWhite)
                {
                    return false;
                }
            }

            for (int i = 0; i < RightSide.Count; i++)
            {
                if (RightSide[i].isWhite != board.RightSide[i].isWhite)
                {
                    return false;
                }
            }

            return Space == board.Space;
        }
        public Board(int N, int M, bool isWhite)
        {
            Space = N;
            LeftSide = new List<Field>();
            for (int i = 0; i < N; i++)
            {
                LeftSide.Add(new Field(isWhite));
            }

            RightSide = new List<Field>();
            for (int i = 0; i < M; i++)
            {
                RightSide.Add(new Field(!isWhite));
            }
        }
        public Board(Board board)
        {
            Space = board.Space;
            LeftSide = new List<Field>(board.LeftSide);
            RightSide = new List<Field>(board.RightSide);
        }
    }

    class BoardNode
    {
        public Board board { get; set; }
        public string TransformAction { get; set; }
        public BoardNode(Board _board, string _TransformAction)
        {
            board = _board;
            TransformAction = _TransformAction;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            #region input and define
            Console.Write("N = ");
            int N = int.Parse(Console.ReadLine());
            Console.Write("M = ");
            int M = int.Parse(Console.ReadLine());
            Board Board = new Board(N, M, true);
            Board FinalBoard = new Board(M, N, false);
            List<BoardNode> Road = new List<BoardNode>();
            #endregion

            #region execute
            GetTransformsRoad(Board, FinalBoard, ref Road);
            Road.Reverse();

            List<List<BoardNode>> WinnngRoutes = new List<List<BoardNode>>();

            for (int i = 0; i < Road.Count; i++)
            {
                if (Road[i].TransformAction.Contains("CanTransFrom"))
                {
                    WinnngRoutes.Add(new List<BoardNode>());
                    int j = i;
                    WinnngRoutes.Last().Add(Road[j]);
                    j++;
                    while ((j < Road.Count) && (!Road[j].TransformAction.Contains("CanTransFrom")))
                    {
                        WinnngRoutes.Last().Add(Road[j]);
                        j++;
                    }
                }
            }


            for (int count = 0; count < WinnngRoutes.Count; count++)
            {
                int i = 1;
                do
                {
                    switch (WinnngRoutes[count][i].TransformAction)
                    {
                        case "TransFormRightThroughOne":
                        case "CanTransFromRightThroughOne":
                            if ((!Board.CanTransFromRightThroughOne(WinnngRoutes[count][i].board)) ||
                                (!Board.TransFormRightThroughOne(WinnngRoutes[count][i].board).Equals(WinnngRoutes[count][i - 1].board)))
                            {
                                WinnngRoutes[count].RemoveAt(i);
                            }
                            else
                            {
                                i++;
                            }
                            break;
                        case "TransFormRightNearest":
                        case "CanTransFromRightNearest":
                            if ((!Board.CanTransFromRightNearest(WinnngRoutes[count][i].board)) ||
                                (!Board.TransFormRightNearest(WinnngRoutes[count][i].board).Equals(WinnngRoutes[count][i - 1].board)))
                            {
                                WinnngRoutes[count].RemoveAt(i);
                            }
                            else
                            {
                                i++;
                            }
                            break;
                        case "TransFormLeftThroughOne":
                        case "CanTransFromLeftThroughOne":
                            if ((!Board.CanTransFromLeftThroughOne(WinnngRoutes[count][i].board)) ||
                                (!Board.TransFormLeftThroughOne(WinnngRoutes[count][i].board).Equals(WinnngRoutes[count][i - 1].board)))
                            {
                                WinnngRoutes[count].RemoveAt(i);
                            }
                            else
                            {
                                i++;
                            }
                            break;
                        case "TransFormLeftNearest":
                        case "CanTransFromLeftNearest":
                            if ((!Board.CanTransFromLeftNearest(WinnngRoutes[count][i].board)) ||
                                (!Board.TransFormLeftNearest(WinnngRoutes[count][i].board).Equals(WinnngRoutes[count][i-1].board)))
                            {
                                WinnngRoutes[count].RemoveAt(i);
                            }
                            else
                            {
                                i++;
                            }
                            break;
                    }
                }
                while (i < WinnngRoutes[count].Count);
            }


            // сохранение первичных настроек в файл "StartupSettings.json"
            using (StreamWriter file = File.CreateText("WinnngRoutes.json"))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(file, WinnngRoutes);
            }
            #endregion

            #region output
            foreach (List<BoardNode> el in WinnngRoutes)
            {
                if (Board.Equals(el.Last().board))
                {
                    Console.WriteLine("Answer = " + el.Count);
                    // get the current process
                    Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                    Console.WriteLine("Memory used in MB = " + currentProcess.WorkingSet64 / 1024 / 1024);
                    Console.WriteLine("Time used in milliseconds = " + (int)currentProcess.TotalProcessorTime.TotalMilliseconds);
                    Console.ReadKey();
                    return;
                }
            }
            #endregion
        }

        public static void GetTransformsRoad(Board board, Board FinalBoard, ref List<BoardNode> Road)
        {
            if (Board.CanTransFromLeftNearest(board) && Board.TransFormLeftNearest(board).Equals(FinalBoard))
            {
                Road.Add(new BoardNode(board, "CanTransFromLeftNearest"));
                return;
            }
            if (Board.CanTransFromLeftThroughOne(board) && Board.TransFormLeftThroughOne(board).Equals(FinalBoard))
            {
                Road.Add(new BoardNode(board, "CanTransFromLeftThroughOne"));
                return;
            }
            if (Board.CanTransFromRightNearest(board) && Board.TransFormRightNearest(board).Equals(FinalBoard))
            {
                Road.Add(new BoardNode(board, "CanTransFromRightNearest"));
                return;
            }
            if (Board.CanTransFromRightThroughOne(board) && Board.TransFormRightThroughOne(board).Equals(FinalBoard))
            {
                Road.Add(new BoardNode(board, "CanTransFromRightThroughOne"));
                return;
            }

            if (Board.CanTransFromLeftNearest(board))
            {
                Road.Add(new BoardNode(board, "TransFormLeftNearest"));
                GetTransformsRoad(Board.TransFormLeftNearest(board), FinalBoard, ref Road);
            }
            if (Board.CanTransFromLeftThroughOne(board))
            {
                Road.Add(new BoardNode(board, "TransFormLeftThroughOne"));

                GetTransformsRoad(Board.TransFormLeftThroughOne(board), FinalBoard, ref Road);
            }
            if (Board.CanTransFromRightNearest(board))
            {
                Road.Add(new BoardNode(board, "TransFormRightNearest"));
                GetTransformsRoad(Board.TransFormRightNearest(board), FinalBoard, ref Road);
            }
            if (Board.CanTransFromRightThroughOne(board))
            {
                Road.Add(new BoardNode(board, "TransFormRightThroughOne"));
                GetTransformsRoad(Board.TransFormRightThroughOne(board), FinalBoard, ref Road);
            }
        }
    }
}