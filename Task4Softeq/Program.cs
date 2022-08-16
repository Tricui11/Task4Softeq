using System;
using System.Collections.Generic;
using System.Linq;

namespace Task4Softeq {
  class Board {
    public List<bool> LeftSide;
    public int Space { get; set; }
    public List<bool> RightSide;
    public static bool CanTransFromLeftNearest(Board Board) {
      return (Board.LeftSide.Count > 0) && Board.LeftSide[Board.LeftSide.Count - 1] && (Board.Space == Board.LeftSide.Count);
    }
    public static Board TransFormLeftNearest(Board Board) {
      Board transformed = new(Board);
      transformed.Space--;
      transformed.RightSide.Insert(0, transformed.LeftSide[transformed.LeftSide.Count - 1]);
      transformed.LeftSide.RemoveAt(transformed.LeftSide.Count - 1);

      return transformed;
    }
    public static bool CanTransFromLeftThroughOne(Board Board) {
      return (Board.LeftSide.Count > 1) && Board.LeftSide[Board.LeftSide.Count - 2] && (Board.Space == Board.LeftSide.Count);
    }
    public static Board TransFormLeftThroughOne(Board Board) {
      Board transformed = new(Board);
      transformed.Space -= 2;
      transformed.RightSide.Insert(0, transformed.LeftSide[transformed.LeftSide.Count - 2]);
      transformed.RightSide.Insert(0, transformed.LeftSide[transformed.LeftSide.Count - 1]);
      transformed.LeftSide.RemoveAt(transformed.LeftSide.Count - 1);
      transformed.LeftSide.RemoveAt(transformed.LeftSide.Count - 1);

      return transformed;
    }
    public static bool CanTransFromRightNearest(Board Board) {
      return (Board.RightSide.Count > 0) && !Board.RightSide[0] && (Board.Space == Board.LeftSide.Count);
    }
    public static Board TransFormRightNearest(Board Board) {
      Board transformed = new(Board);
      transformed.Space++;
      transformed.LeftSide.Add(transformed.RightSide[0]);
      transformed.RightSide.RemoveAt(0);

      return transformed;
    }
    public static bool CanTransFromRightThroughOne(Board Board) {
      return (Board.RightSide.Count > 1) && !Board.RightSide[1] && (Board.Space == Board.LeftSide.Count);
    }
    public static Board TransFormRightThroughOne(Board Board) {
      Board transformed = new(Board);
      transformed.Space += 2;
      transformed.LeftSide.Add(transformed.RightSide[1]);
      transformed.LeftSide.Add(transformed.RightSide[0]);
      transformed.RightSide.RemoveAt(0);
      transformed.RightSide.RemoveAt(0);

      return transformed;
    }
    public override bool Equals(object obj) {
      if (obj.GetType() != GetType()) return false;

      Board board = (Board)obj;

      if ((LeftSide.Count != board.LeftSide.Count) || (RightSide.Count != board.RightSide.Count)) {
        return false;
      }

      for (int i = 0; i < LeftSide.Count; i++) {
        if (LeftSide[i] != board.LeftSide[i]) {
          return false;
        }
      }

      for (int i = 0; i < RightSide.Count; i++) {
        if (RightSide[i] != board.RightSide[i]) {
          return false;
        }
      }

      return Space == board.Space;
    }
    public Board(int N, int M, bool isWhite) {
      Space = N;
      LeftSide = new List<bool>();
      for (int i = 0; i < N; i++) {
        LeftSide.Add(isWhite);
      }

      RightSide = new List<bool>();
      for (int i = 0; i < M; i++) {
        RightSide.Add(!isWhite);
      }
    }
    public Board(Board board) {
      Space = board.Space;
      LeftSide = new List<bool>(board.LeftSide);
      RightSide = new List<bool>(board.RightSide);
    }
    public Board() {
    }
  }
  class BoardNode {
    public Board Board { get; set; }
    public TransFormAction TransformAction { get; set; }
    public BoardNode(Board board, TransFormAction transformAction) {
      Board = board;
      TransformAction = transformAction;
    }

    public bool CanTransform() => TransformAction == TransFormAction.CanTransFromRightNearest ||
    TransformAction == TransFormAction.CanTransFromLeftNearest ||
    TransformAction == TransFormAction.CanTransFromLeftThroughOne ||
    TransformAction == TransFormAction.CanTransFromRightThroughOne;
  }
  class Program {
    static void Main(string[] args) {
      #region input and define

      ushort N = 5;
      ushort M = 6;

      Board board = new();
      Board finalBoard = new();
      List<BoardNode> road = new();
      List<List<BoardNode>> winnngRoutes = new();
      #endregion

      #region CountMinTurns function f(N, M)
      int res = 0;
      //   We can calculate res directly, but performance is poor
      res = CountMinTurns(N, M, ref board, ref finalBoard, ref road, ref winnngRoutes);

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

    private static void GetTransformsRoad(Board board, Board finalBoard, ref List<BoardNode> road) {




      foreach(var el in board.LeftSide) {
        Console.Write(el.ToString() + ' ');
      }
      Console.Write(board.Space.ToString() + ' ');
      foreach (var el in board.RightSide) {
        Console.Write(el.ToString() + ' ');
      }
      Console.WriteLine();







      if (Board.CanTransFromLeftNearest(board) && Board.TransFormLeftNearest(board).Equals(finalBoard)) {
        road.Add(new BoardNode(board, TransFormAction.CanTransFromLeftNearest));
        return;
      }
      if (Board.CanTransFromLeftThroughOne(board) && Board.TransFormLeftThroughOne(board).Equals(finalBoard)) {
        road.Add(new BoardNode(board, TransFormAction.CanTransFromLeftThroughOne));
        return;
      }
      if (Board.CanTransFromRightNearest(board) && Board.TransFormRightNearest(board).Equals(finalBoard)) {
        road.Add(new BoardNode(board, TransFormAction.CanTransFromRightNearest));
        return;
      }
      if (Board.CanTransFromRightThroughOne(board) && Board.TransFormRightThroughOne(board).Equals(finalBoard)) {
        road.Add(new BoardNode(board, TransFormAction.CanTransFromRightThroughOne));
        return;
      }

      if (Board.CanTransFromLeftNearest(board)) {
        road.Add(new BoardNode(board, TransFormAction.TransFormLeftNearest));
        GetTransformsRoad(Board.TransFormLeftNearest(board), finalBoard, ref road);
      }
      if (Board.CanTransFromLeftThroughOne(board)) {
        road.Add(new BoardNode(board, TransFormAction.TransFormLeftThroughOne));

        GetTransformsRoad(Board.TransFormLeftThroughOne(board), finalBoard, ref road);
      }
      if (Board.CanTransFromRightNearest(board)) {
        road.Add(new BoardNode(board, TransFormAction.TransFormRightNearest));
        GetTransformsRoad(Board.TransFormRightNearest(board), finalBoard, ref road);
      }
      if (Board.CanTransFromRightThroughOne(board)) {
        road.Add(new BoardNode(board, TransFormAction.TransFormRightThroughOne));
        GetTransformsRoad(Board.TransFormRightThroughOne(board), finalBoard, ref road);
      }
    }
    private static int CountMinTurns(ushort N, ushort M, ref Board board, ref Board finalBoard,
      ref List<BoardNode> road, ref List<List<BoardNode>> winnngRoutes) {
      board = new Board(N, M, true);
      finalBoard = new Board(M, N, false);
      road.Clear();
      GetTransformsRoad(board, finalBoard, ref road);
      road.Reverse();

      GetWinnngRoutes(ref road, ref winnngRoutes);
      ClearWinnngRoutes(ref road, ref winnngRoutes);

      foreach (List<BoardNode> el in winnngRoutes) {
        if (board.Equals(el.Last().Board)) {
          return el.Count;
        }
      }

      return 0;
    }
    private static void GetWinnngRoutes(ref List<BoardNode> road, ref List<List<BoardNode>> winnngRoutes) {
      winnngRoutes.Clear();

      for (int i = 0; i < road.Count; i++) {
        if (road[i].CanTransform()) {
          winnngRoutes.Add(new List<BoardNode>());
          int j = i;
          winnngRoutes.Last().Add(road[j]);
          j++;
          while ((j < road.Count) && (!road[j].CanTransform())) {
            winnngRoutes.Last().Add(road[j]);
            j++;
          }
        }
      }
    }
    private static void ClearWinnngRoutes(ref List<BoardNode> road, ref List<List<BoardNode>> winnngRoutes) {
      for (int count = 0; count < winnngRoutes.Count; count++) {
        int i = 1;
        do {
          switch (winnngRoutes[count][i].TransformAction) {
            case TransFormAction.TransFormRightThroughOne:
            case TransFormAction.CanTransFromRightThroughOne:
              if ((!Board.CanTransFromRightThroughOne(winnngRoutes[count][i].Board)) ||
                  (!Board.TransFormRightThroughOne(winnngRoutes[count][i].Board).Equals(winnngRoutes[count][i - 1].Board))) {
                winnngRoutes[count].RemoveAt(i);
              } else {
                i++;
              }
              break;
            case TransFormAction.TransFormRightNearest:
            case TransFormAction.CanTransFromRightNearest:
              if ((!Board.CanTransFromRightNearest(winnngRoutes[count][i].Board)) ||
                  (!Board.TransFormRightNearest(winnngRoutes[count][i].Board).Equals(winnngRoutes[count][i - 1].Board))) {
                winnngRoutes[count].RemoveAt(i);
              } else {
                i++;
              }
              break;
            case TransFormAction.TransFormLeftThroughOne:
            case TransFormAction.CanTransFromLeftThroughOne:
              if ((!Board.CanTransFromLeftThroughOne(winnngRoutes[count][i].Board)) ||
                  (!Board.TransFormLeftThroughOne(winnngRoutes[count][i].Board).Equals(winnngRoutes[count][i - 1].Board))) {
                winnngRoutes[count].RemoveAt(i);
              } else {
                i++;
              }
              break;
            case TransFormAction.TransFormLeftNearest:
            case TransFormAction.CanTransFromLeftNearest:
              if ((!Board.CanTransFromLeftNearest(winnngRoutes[count][i].Board)) ||
                  (!Board.TransFormLeftNearest(winnngRoutes[count][i].Board).Equals(winnngRoutes[count][i - 1].Board))) {
                winnngRoutes[count].RemoveAt(i);
              } else {
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