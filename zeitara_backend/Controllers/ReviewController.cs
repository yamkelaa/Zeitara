using Application.DTO.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ReviewController(IReviewLogic _reviewLogic) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddReview([FromBody] ReviewRequestDto request)
    {
        if (request == null || request.UserId <= 0 || request.ProductId <= 0 || request.Rating is < 1 or > 5)
            return BadRequest("Invalid review data.");

        var result = await _reviewLogic.AddReviewAsync(request);
        return Ok(result);
    }

    [HttpGet("{userId}/reviews")]
    public async Task<IActionResult> GetUserProductReviews(int userId, [FromQuery] int pageSize = 10, [FromQuery] int lastLoadedId = 0)
    {
        if (userId <= 0)
            return BadRequest("Invalid user ID.");

        var reviews = await _reviewLogic.GetProductReviewsWithStatusesAsync(userId, pageSize, lastLoadedId);

        return Ok(reviews);
    }



    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> DeleteReview(int reviewId, [FromQuery] int userId)
    {
        if (reviewId <= 0 || userId <= 0)
            return BadRequest("Invalid review or user ID.");

        var result = await _reviewLogic.DeleteReviewAsync(reviewId, userId);
        return Ok(result);
    }

}

