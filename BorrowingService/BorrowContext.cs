using BorrowingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BorrowingService
{
	public class BorrowContext : DbContext
	{
		public BorrowContext(DbContextOptions<BorrowContext> options) : base(options)
		{
		}
		public DbSet<Borrow> Borrows { get; set; }
	}
}
