using System.Linq;
using RepositoryContracts;
using Entities;

namespace CliApp.UI;

public class Cli
{
    private readonly IUserRepository _users;
    private readonly IPostRepository _posts;
    private readonly ICommentRepository _comments;

    public Cli(IUserRepository users, IPostRepository posts, ICommentRepository comments)
    {
        _users = users;
        _posts = posts;
        _comments = comments;
    }

    public async Task Run()
    {
        PrintWelcome();

        while (true)
        {
            Console.Write("\n> ");
            var cmd = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

            switch (cmd)
            {
                case "help": PrintHelp(); break;
                case "exit": return;

                case "user": await CreateUser(); break;
                case "post": await CreatePost(); break;
                case "comment": await AddComment(); break;

                case "posts": await ShowPostsOverview(); break;
                case "view": await ViewSinglePost(); break;

                default:
                    Console.WriteLine("Unknown command. Type 'help' to see options.");
                    break;
            }
        }
    }

    private void PrintWelcome()
    {
        Console.WriteLine("=== Forum CLI ===");
        PrintHelp();
    }

    private void PrintHelp()
    {
        Console.WriteLine(@"
Commands:
  user            -> create a new user
  post            -> create a new post (title, content, userId)
  comment         -> add a comment (postId, userId, body)
  posts           -> list all posts (id + title)
  view            -> view one post
  help            -> show this help
  exit            -> quit
");
    }

    private async Task CreateUser()
    {
        Console.Write("Username: ");
        string username = Console.ReadLine()!.Trim();
        Console.Write("Password: ");
        string password = Console.ReadLine()!.Trim();

        var user = new User(username, password);
        await _users.AddAsync(user);
        Console.WriteLine($"Created user #{user.Id} ({user.Username}).");
    }

    private async Task CreatePost()
    {
        Console.Write("Title: ");
        string title = Console.ReadLine()!.Trim();
        Console.Write("Content: ");
        string content = Console.ReadLine()!.Trim();
        Console.Write("User Id (author): ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user id.");
            return;
        }

        var author = await _users.GetByIdAsync(userId);
        if (author is null)
        {
            Console.WriteLine($"No user with id {userId}.");
            return;
        }

        var post = new Post(title, content, userId);
        await _posts.AddAsync(post);
        Console.WriteLine($"Created post #{post.Id} by {author.Username}.");
    }

    private async Task AddComment()
    {
        Console.Write("Post Id: ");
        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid post id.");
            return;
        }

        Console.Write("User Id: ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user id.");
            return;
        }

        Console.Write("Body: ");
        string body = Console.ReadLine()!.Trim();

        var post = await _posts.GetByIdAsync(postId);
        var user = await _users.GetByIdAsync(userId);
        if (post is null || user is null)
        {
            Console.WriteLine("Invalid user or post id.");
            return;
        }

        var comment = new Comment(body, userId, postId);
        await _comments.AddAsync(comment);
        Console.WriteLine($"Added comment #{comment.Id} to post #{postId} by {user.Username}.");
    }

    private async Task ShowPostsOverview()
    {
        var posts = _posts.GetMany();
        var all = await Task.Run(() => posts.OrderBy(p => p.Id).ToList());
        
        if (all.Count == 0)
        {
            Console.WriteLine("(no posts yet)");
            return;
        }

        Console.WriteLine("Posts:");
        foreach (var p in all)
            Console.WriteLine($"  {p.Id}: {p.Title}");
    }

    private async Task ViewSinglePost()
    {
        Console.Write("Post Id: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid id.");
            return;
        }

        var post = await _posts.GetByIdAsync(id);
        if (post is null)
        {
            Console.WriteLine($"No post with id {id}.");
            return;
        }

        var author = await _users.GetByIdAsync(post.UserId);
        Console.WriteLine($"\n[{post.Id}] {post.Title}");
        Console.WriteLine($"By: {(author?.Username ?? "Unknown")}");
        Console.WriteLine(post.Body);

        var comments = _comments.GetMany().Where(c => c.PostId == post.Id);
        var postComments = await Task.Run(() => comments.OrderBy(c => c.Id).ToList());
        
        if (postComments.Count == 0)
        {
            Console.WriteLine("\n(no comments)");
            return;
        }

        Console.WriteLine("\nComments:");
        foreach (var comment in postComments)
        {
            var user = await _users.GetByIdAsync(comment.UserId);
            Console.WriteLine($"  #{comment.Id} by {(user?.Username ?? "Unknown")}: {comment.Body}");
        }
    }
}