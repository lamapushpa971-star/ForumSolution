using FileRepositories;
using RepositoryContracts;
using Server.CliApp.UI;

namespace Server.CliApp;

class Program
{
    static void Main()
    {
        IUserRepository userRepo = new UserFileRepository();
        IPostRepository postRepo = new PostFileRepository();
        ICommentRepository commentRepo = new CommentFileRepository();

        var app = new Cli(userRepo, postRepo, commentRepo);
        app.Run();
    }
}
