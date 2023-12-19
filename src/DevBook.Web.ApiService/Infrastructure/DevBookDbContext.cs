using Microsoft.EntityFrameworkCore;

namespace DevBook.Web.ApiService.Infrastructure;

public sealed class DevBookDbContext(DbContextOptions<DevBookDbContext> _options) : DbContext(_options)
{

}
