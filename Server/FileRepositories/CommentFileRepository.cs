using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string _filePath;

    public CommentFileRepository()
    
    {
        var dataDir = Path.Combine(AppContext.BaseDirectory, "data");
        if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);
        _filePath = Path.Combine(dataDir, "comments.json");
        if (!File.Exists(_filePath)) File.WriteAllText(_filePath, "[]");
    }

    public void Add(Comment comment)
    {
        var list = FileHelper.LoadList<Comment>(_filePath);
        comment.Id = list.Any() ? list.Max(c => c.Id) + 1 : 1;
        list.Add(comment);
        FileHelper.SaveList(_filePath, list);
    }

    public void Update(Comment comment)
    {
        var list = FileHelper.LoadList<Comment>(_filePath);
        var ex = list.FirstOrDefault(c => c.Id == comment.Id);
        if (ex != null)
        {
            list.Remove(ex);
            list.Add(comment);
            FileHelper.SaveList(_filePath, list);
        }
    }

    public void Delete(int id)
    {
        var list = FileHelper.LoadList<Comment>(_filePath);
        list.RemoveAll(c => c.Id == id);
        FileHelper.SaveList(_filePath, list);
    }

    public Comment? GetById(int id)
    {
        var list = FileHelper.LoadList<Comment>(_filePath);
        return list.FirstOrDefault(c => c.Id == id);
    }

    public List<Comment> GetAll()
    {
        return FileHelper.LoadList<Comment>(_filePath);
    }
}
