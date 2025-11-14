using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string _filePath;

    public PostFileRepository()
    {
        var dataDir = Path.Combine(AppContext.BaseDirectory, "data");
        if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);
        _filePath = Path.Combine(dataDir, "posts.json");
        if (!File.Exists(_filePath)) File.WriteAllText(_filePath, "[]");
    }

    public void Add(Post post)
    {
        var list = FileHelper.LoadList<Post>(_filePath);
        post.Id = list.Any() ? list.Max(p => p.Id) + 1 : 1;
        list.Add(post);
        FileHelper.SaveList(_filePath, list);
    }

    public void Update(Post post)
    {
        var list = FileHelper.LoadList<Post>(_filePath);
        var ex = list.FirstOrDefault(p => p.Id == post.Id);
        if (ex != null)
        {
            list.Remove(ex);
            list.Add(post);
            FileHelper.SaveList(_filePath, list);
        }
    }

    public void Delete(int id)
    {
        var list = FileHelper.LoadList<Post>(_filePath);
        list.RemoveAll(p => p.Id == id);
        FileHelper.SaveList(_filePath, list);
    }

    public Post? GetById(int id)
    {
        var list = FileHelper.LoadList<Post>(_filePath);
        return list.FirstOrDefault(p => p.Id == id);
    }

    public List<Post> GetAll()
    {
        return FileHelper.LoadList<Post>(_filePath);
    }
}
