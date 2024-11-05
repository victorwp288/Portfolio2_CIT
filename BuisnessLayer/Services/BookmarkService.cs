using BuisnessLayer.DTOs;
using BuisnessLayer.Interfaces;
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

        public async Task CreateBookmarkAsync(BookmarkDTO bookmarkDto)
    {
        // Check if the user exists
        var user = await _context.Users.FindAsync(bookmarkDto.UserId);
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        // Check if the title exists
        var title = await _context.TitleBasics.FindAsync(bookmarkDto.TConst);
        if (title == null)
            throw new KeyNotFoundException("Title not found.");

        // Check if the user has already bookmarked this title
        var existingBookmark = await _context.UserBookmarks
            .SingleOrDefaultAsync(r => r.UserId == bookmarkDto.UserId && r.Tconst == bookmarkDto.TConst);

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
                UserId = bookmarkDto.UserId,
                Tconst = bookmarkDto.TConst,
                Note = bookmarkDto.Note,
                BookmarkDate = bookmarkDto.BookmarkDate
            };
            _context.UserBookmarks.Add(userBookmark);
        }

        // Save changes
        await _context.SaveChangesAsync();

        // Update the TitleRating
        //await UpdateBookmarkAsync(bookmarkDto.TConst);
    }

    /*private async Task UpdateBookmarkAsync(string tconst)
    {
        // Calculate the new average rating and number of votes
        var bookmark = await _context.UserBookmarks
            .Where(r => r.Tconst == tconst)
            .ToListAsync();

        var averageRating = ratings.Average(r => r.Rating);
        var numVotes = ratings.Count;

        // Check if a TitleRating exists
        var titleRating = await _context.TitleRatings.FindAsync(tconst);

        if (titleRating != null)
        {
            // Update existing TitleRating
            titleRating.AverageRating = (decimal)averageRating;
            titleRating.NumVotes = numVotes;
            _context.TitleRatings.Update(titleRating);
        }
        else
        {
            // Create new TitleRating
            titleRating = new TitleRating
            {
                Tconst = tconst,
                AverageRating = (decimal)averageRating,
                NumVotes = numVotes
            };
            _context.TitleRatings.Add(titleRating);
        }

        // Save changes
        await _context.SaveChangesAsync();
    }*/

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
}
