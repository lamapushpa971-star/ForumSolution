using Entities;
using RepositoryContracts;
using InMemoryRepositories;

namespace ForumApp;

class Program
{
    static void Main()
    {
        IPostRepository postRepo = new InMemoryPostRepository();
        IUserRepository userRepo = new InMemoryUserRepository();
        ICommentRepository commentRepo = new InMemoryCommentRepository();

        var u = new User(    "Pushpa" ,"1234" );
        userRepo.AddAsync(u);

        var p = new Post ( "Hello",  "My first post",  u.Id );
        postRepo.AddAsync(p);

        var c = new Comment ( "Nice post!", u.Id,  p.Id );
        commentRepo.AddAsync(c);

        Console.WriteLine("== Users ==");
        foreach (var user in userRepo.GetMany())
            Console.WriteLine($"{user.Id}: {user.Username}");

        Console.WriteLine("\n== Posts ==");
        foreach (var post in postRepo.GetMany())
            Console.WriteLine($"{post.Id}: {post.Title} - {post.Body} (User {post.UserId})");

        Console.WriteLine("\n== Comments ==");
        foreach (var com in commentRepo.GetMany())
            Console.WriteLine($"{com.Id}: {com.Body} (User {com.UserId} on Post {com.PostId})");
    }
}
