using BuisnessLayer.DTOs;
using BuisnessLayer.Interfaces;
using BusinessLayer.DTOs;
using DataAcessLayer.Context;
using DataAcessLayer.Entities.Movies;
using DataAcessLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Services;

    public class BookmarkService : IBookmarkService
{
        private readonly ImdbContext _context;

        public BookmarkService(ImdbContext context)
        {
            _context = context;
        }

        public async Task CreateBookmarkAsync(int userId, string tconst, BookmarkDTO bookmarkDto)
    {
        // Check if the user exists
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        // Check if the title exists
        var title = await _context.TitleBasics.FindAsync(tconst);
        if (title == null)
            throw new KeyNotFoundException("Title not found.");

        // Check if the user has already bookmarked this title
        var existingBookmark = await _context.UserBookmarks
            .SingleOrDefaultAsync(r => r.UserId == userId && r.Tconst == tconst);

        if (existingBookmark != null)
        {
            // Update existing bookmark
            existingBookmark.Note = bookmarkDto.Note;
            _context.UserBookmarks.Update(existingBookmark);
        }
        else
        {
            // Add new bookmark
            var userBookmark = new UserBookmark
            {
                UserId = userId,
                Tconst = tconst,
                Note = bookmarkDto.Note,
                BookmarkDate = bookmarkDto.BookmarkDate
            };
            _context.UserBookmarks.Add(userBookmark);
        }

        // Save changes
        await _context.SaveChangesAsync();
    }


    public async Task DeleteBookmarkAsync(int userId, string tconst)
    {
        //var user = await _context.UserBookmarks.FindAsync(userId);
        var title = await _context.UserBookmarks.FindAsync(userId, tconst);
        //if (user == null)
            //throw new KeyNotFoundException("User not found.");
        if (title == null)
            throw new KeyNotFoundException("Title not found.");

        _context.UserBookmarks.Remove(title);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<BookmarkDTO>> GetUserBookmarksAsync(int userId)
    {
        var bookmarks = await _context.UserBookmarks
            .Where(r => r.UserId == userId)
            .ToListAsync();

        return bookmarks.Select(b => new BookmarkDTO
        {
            UserId = b.UserId,
            Tconst = b.Tconst,
            Note = b.Note,
            BookmarkDate = b.BookmarkDate
        });
    }
}
