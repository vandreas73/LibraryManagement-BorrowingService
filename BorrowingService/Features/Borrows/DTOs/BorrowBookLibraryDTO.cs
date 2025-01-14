namespace BorrowingService.Features.Borrows.DTOs
{
	public record BorrowBookLibraryDto(int Id, int BookId, string BookTitle, string BookAuthor, int UserId, int LibraryId, string LibraryName, DateOnly BorrowedAt, DateOnly DueDate, DateOnly? ReturnedAt);
}
