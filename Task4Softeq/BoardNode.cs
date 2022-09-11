namespace Task4Softeq
{
    internal class BoardNode
    {
        public Board Board { get; set; }
        public TransFormAction TransformAction { get; set; }

        public BoardNode(Board board, TransFormAction transformAction)
        {
            Board = board;
            TransformAction = transformAction;
        }

    public bool CanTransformToFinal() => TransformAction == TransFormAction.CanTransFromRightNearest ||
        TransformAction == TransFormAction.CanTransFromLeftNearest;
    // || следуя той же логике что и в методах CanMoveRightItemsThroughLeftSpace CanMoveLeftItemsThroughRightSpace
    // можно отсечь трансформацию в финальную доску в ход через одну фигуру
    //        TransformAction == TransFormAction.CanTransFromLeftThroughOne || 
    //        TransformAction == TransFormAction.CanTransFromRightThroughOne;
  }
}
