﻿using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAcessLayer;
using DataAcessLayer.Users; // Ensure correct namespace
using DataAcessLayer.Movies; // For TitleRating
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services
{
        public class RatingService : IRatingService
        {
            private readonly ImdbContext _context;

            public RatingService(ImdbContext context)
            {
                _context = context;
            }
            public async Task<UserRatingDTO> GetUserRatingAsync(int userId, string tconst)
            {
                var rating = await _context.UserRatingReviews
                                           .SingleOrDefaultAsync(r => r.UserId == userId && r.Tconst == tconst);

                if (rating == null)
                    return null;

                var ratingDto = new UserRatingDTO
                {
                    UserId = rating.UserId,
                    TConst = rating.Tconst,
                    Rating = rating.Rating, // No need for '?? 0'
                    Review = rating.Review,
                    ReviewDate = rating.ReviewDate
                };

                return ratingDto;
            }

            public async Task SubmitUserRatingAsync(UserRatingDTO ratingDto)
            {
                // Check if the user exists
                var user = await _context.Users.FindAsync(ratingDto.UserId);
                if (user == null)
                    throw new KeyNotFoundException("User not found.");

                // Check if the title exists
                var title = await _context.TitleBasics.FindAsync(ratingDto.TConst);
                if (title == null)
                    throw new KeyNotFoundException("Title not found.");

                // Check if the user has already rated this title
                var existingRating = await _context.UserRatingReviews
                    .SingleOrDefaultAsync(r => r.UserId == ratingDto.UserId && r.Tconst == ratingDto.TConst);

                if (existingRating != null)
                {
                    // Update existing rating
                    existingRating.Rating = ratingDto.Rating;
                    existingRating.Review = ratingDto.Review;
                    _context.UserRatingReviews.Update(existingRating);
                }
                else
                {
                    // Add new rating
                    var userRating = new UserRatingReview
                    {
                        UserId = ratingDto.UserId,
                        Tconst = ratingDto.TConst,
                        Rating = ratingDto.Rating,
                        Review = ratingDto.Review
                    };
                    _context.UserRatingReviews.Add(userRating);
                }

                // Save changes
                await _context.SaveChangesAsync();

                // Update the TitleRating
                await UpdateTitleRatingAsync(ratingDto.TConst);
            }

            private async Task UpdateTitleRatingAsync(string tconst)
            {
                // Calculate the new average rating and number of votes
                var ratings = await _context.UserRatingReviews
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
            }
        }

    }