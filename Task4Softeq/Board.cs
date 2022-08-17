﻿using System;

namespace Task4Softeq
{
    internal class Board
    {
        private readonly bool[] fields;
        private int space;

        internal Board(int N, int M, bool isWhite)
        {
            space = N;
            fields = new bool[N + M];
            for (int i = 0; i < N; i++)
            {
                fields[i] = isWhite;
            }

            for (int i = N; i < M + N; i++)
            {
                fields[i] = !isWhite;
            }
        }
        internal Board(Board board)
        {
            space = board.space;
            fields = new bool[board.fields.Length];
            Array.Copy(board.fields, fields, board.fields.Length);
        }

        internal bool CanTransFromLeftNearest() => space != 0 && fields[space - 1];
        internal bool CanTransFromLeftThroughOne() => space > 1 && fields[space - 2];
        internal bool CanTransFromRightNearest() => space < fields.Length && !fields[space];
        internal bool CanTransFromRightThroughOne() => space < fields.Length - 1 && !fields[space + 1];

        internal Board TransFormLeftNearest()
        {
            Board transformed = new(this);
            transformed.space--;

            return transformed;
        }
        internal Board TransFormRightNearest()
        {
            Board transformed = new(this);
            transformed.space++;

            return transformed;
        }
        internal Board TransFormLeftThroughOne()
        {
            Board transformed = new(this);
            bool temp = transformed.fields[transformed.space - 1];
            transformed.fields[transformed.space - 1] = transformed.fields[transformed.space - 2];
            transformed.fields[transformed.space - 2] = temp;
            transformed.space -= 2;

            return transformed;
        }
        internal Board TransFormRightThroughOne()
        {
            Board transformed = new(this);
            bool temp = transformed.fields[transformed.space];
            transformed.fields[transformed.space] = transformed.fields[transformed.space + 1];
            transformed.fields[transformed.space + 1] = temp;
            transformed.space += 2;

            return transformed;
        }

        public override bool Equals(object obj)
        {
//            if (obj.GetType() != GetType()) return false;

            Board board = (Board)obj;

            if (fields.Length != board.fields.Length)
            {
                return false;
            }

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i] != board.fields[i])
                {
                    return false;
                }
            }

            return space == board.space;
        }
    }
}
