﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using BuisnessLayer.DTOs;


namespace BuisnessLayer.Interfaces;

public interface IBookmarkService
{
    Task CreateBookmarkAsync(int userId, string tconst, BookmarkDTO bookmarkDto);

    Task<IEnumerable<BookmarkDTO>> GetUserBookmarksAsync(int userId);

    Task DeleteBookmarkAsync(int userId, string tconst);

    Task DeleteAllBookmarksForUserAsync(int userId);
}
