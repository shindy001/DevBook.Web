﻿namespace DevBook.Web.ApiService.Features.Sudoku;

internal sealed record GetBoardDataQuery : IQuery<BoardData>;

internal sealed class GetBoardDataQueryHandler(ISudokuService sudokuService) : IQueryHandler<GetBoardDataQuery, BoardData>
{
	public async Task<BoardData> Handle(GetBoardDataQuery request, CancellationToken cancellationToken)
	{
		return await sudokuService.GetBoardData(cancellationToken);
	}
}
