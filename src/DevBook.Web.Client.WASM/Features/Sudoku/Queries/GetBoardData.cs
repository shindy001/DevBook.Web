namespace DevBook.Web.Client.WASM.Features.Sudoku.Queries;

internal static class GetBoardData
{
	internal sealed record Query : IRequest<OneOf<int[][], DevBookError>>;

	private sealed class Handler(IDevBookWebApiGraphQLClient client) : IRequestHandler<Query, OneOf<int[][], DevBookError>>
	{
		public async Task<OneOf<int[][], DevBookError>> Handle(Query request, CancellationToken cancellationToken)
		{
			var result = await client.GetBoardData.ExecuteAsync(cancellationToken);
			var boardData = result.Data?.BoardData;

			if (result.IsErrorResult() || boardData is null)
			{
				return result.CreateError();
			}

			var gridNumbersArray = boardData.GridNumbers
			   .Select(innerCollection => innerCollection.ToArray())
			   .ToArray();

			return result.Unwrap(() => gridNumbersArray);
		}
	}
}
