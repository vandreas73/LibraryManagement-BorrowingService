using BorrowingService.Features.Borrows.DTOs;
using BorrowingService.InventoryClient;
using MediatR;

namespace BorrowingService.Features.Borrows.Queries.GetBookFreeCount
{
	public class GetBookFreeCountHandler : IRequestHandler<GetBookFreeCountQuery, ICollection<BookCountInLibraryDTO>>
	{
		private readonly BorrowContext context;
		private readonly InventoryClient.InventoryClient inventoryClient;

		public GetBookFreeCountHandler(BorrowContext context, InventoryClient.InventoryClient inventoryClient)
		{
			this.context = context;
			this.inventoryClient = inventoryClient;
		}

		public async Task<ICollection<BookCountInLibraryDTO>> Handle(GetBookFreeCountQuery request, CancellationToken cancellationToken)
		{
			var libraries = await inventoryClient.Books2Async(request.BookId);
			//TODO: Inventory mgmt service should return count or librarybook id
			return new List<BookCountInLibraryDTO>();
		}
	}
}
