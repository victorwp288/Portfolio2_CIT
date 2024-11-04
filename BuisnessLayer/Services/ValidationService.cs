using System;

public static class ValidationService
{
    public static void ValidatePageParameters(int page, int pageSize)
    {
        if (page < 0) throw new ArgumentException("Page number cannot be negative");
        if (pageSize <= 0) throw new ArgumentException("Page size must be positive");
        if (pageSize > 100) throw new ArgumentException("Page size cannot exceed 100");
    }

    public static void ValidateSearchQuery(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("Search query cannot be empty");
        if (query.Length < 2)
            throw new ArgumentException("Search query must be at least 2 characters long");
    }
} 