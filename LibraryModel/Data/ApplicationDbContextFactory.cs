using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using LibraryModel.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<LibraryContext>
{
    public LibraryContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LibraryNew;Trusted_Connection=True;MultipleActiveResultSets=true");

        return new LibraryContext(optionsBuilder.Options);
    }
}