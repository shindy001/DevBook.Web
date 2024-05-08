namespace DevBook.Web.Client.WASM.Features.Sudoku.Queries;

internal static class GetBoardData
{
	internal sealed record Query : IRequest<OneOf<int[][], DevBookError>>;

	private sealed class Handler(IDevBookWebApiGraphQLClient client) : IRequestHandler<Query, OneOf<int[][], DevBookError>>
	{
		public async Task<OneOf<int[][], DevBookError>> Handle(Query request, CancellationToken cancellationToken)
		{
			var result = await client.GetBoardData.ExecuteAsync(cancellationToken);

			return result.Unwrap(
				() => result.Data?.BoardData.GridNumbers
				   .Select(innerCollection => innerCollection.ToArray())
				   .ToArray() ?? []);
		}
	}
}
