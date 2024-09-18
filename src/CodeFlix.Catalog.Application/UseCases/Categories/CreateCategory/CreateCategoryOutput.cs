﻿using CodeFlix.Catalog.Domain.Entities;

namespace CodeFlix.Catalog.Application.UseCases.Categories.CreateCategory;

public class CreateCategoryOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public CreateCategoryOutput(
        Guid id, 
        string name, 
        string? description, 
        bool isActive, 
        DateTime createdAt)
    {
        Id = id;
        Name = name;
        Description = description ?? "";
        IsActive = isActive;
        CreatedAt = createdAt;
    }

    public static CreateCategoryOutput FromCategory(Category category)
    {
        return new CreateCategoryOutput(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive,
            category.CreatedAt);
    }
}
