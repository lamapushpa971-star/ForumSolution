using CliApp.UI;
using EfcRepositories;
using Microsoft.EntityFrameworkCore;

namespace CliApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var context = new ForumAppContext();
        
        // ADD THIS LINE - it will create the database and tables
        context.Database.EnsureCreated();
        
        var userRepo = new EfcUserRepository(context);
        var postRepo = new EfcPostRepository(context);
        var commentRepo = new EfcCommentRepository(context);

        var cli = new Cli(userRepo, postRepo, commentRepo);
        await cli.Run();
    }
}