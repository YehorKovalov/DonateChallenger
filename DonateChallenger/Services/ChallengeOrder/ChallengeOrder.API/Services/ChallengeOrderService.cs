using ChallengeOrder.API.Data;
using ChallengeOrder.API.Models.DTOs;
using ChallengeOrder.API.Models.Responses;
using ChallengeOrder.API.Repositories.Abstractions;
using ChallengeOrder.API.Services.Abstractions;
using Infrastructure.Helpers;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;

namespace ChallengeOrder.API.Services;

public class ChallengeOrderService : BaseDataService<AppDbContext>,  IChallengeOrderService
{
    private readonly IChallengeOrderRepository _orderRepository;
    public ChallengeOrderService(
        IDbContextWrapper<AppDbContext> dbContext,
        ILogger<BaseDataService<AppDbContext>> logger,
        IChallengeOrderRepository orderRepository)
            : base(dbContext, logger)
    {
        _orderRepository = orderRepository;
    }

    public async Task<AddChallengeOrderResponse<Guid?>> AddChallengeOrderAsync(string paymentId, int challengesAmount, double resultDonationPrice)
    {
        return await ExecuteSafeAsync(async () =>
        {
            if (!AddChallengeOrderStateIsValid(paymentId, challengesAmount, resultDonationPrice))
            {
                var errorMessage = "Add Challenge Order State Is Valid";
                Logger.LogError($"{nameof(AddChallengeOrderAsync)} ---> {errorMessage}");
                return new AddChallengeOrderResponse<Guid?>
                {
                    ErrorMessage = errorMessage,
                    Succeeded = false,
                    OrderId = null
                };
            }

            var orderId = await _orderRepository.Add(paymentId, challengesAmount, resultDonationPrice);
            Logger.LogInformation($"{nameof(AddChallengeOrderAsync)} ---> {nameof(orderId)}: {orderId}");
            return new AddChallengeOrderResponse<Guid?>
            {
                OrderId = orderId,
                Succeeded = true
            };
        });
    }

    public async Task<UpdateChallengeOrderResponse<Guid>> UpdateChallengeOrderAsync(string challengeOrderId, string paymentId, int challengesAmount, double resultDonationPrice)
    {
        var result = await _orderRepository.Update(challengeOrderId, paymentId, challengesAmount, resultDonationPrice);
        return new UpdateChallengeOrderResponse<Guid>
        {
            Data = result
        };
    }

    public async Task<GetChallengeOrderByIdResponse<ChallengeOrderDto?>> GetChallengeOrderByIdAsync(string orderId)
    {
        var result = await _orderRepository.GetById(orderId);
        if (result == null)
        {
            return new GetChallengeOrderByIdResponse<ChallengeOrderDto?> { Data = null };
        }

        return new GetChallengeOrderByIdResponse<ChallengeOrderDto?>
        {
            Data = new ChallengeOrderDto
            {
                ChallengeOrderId = result.ChallengeOrderId,
                ChallengesAmount = result.ChallengesAmount,
                Date = result.Date,
                PaymentId = result.PaymentId,
                ResultDonationPrice = result.ResultDonationPrice
            }
        };
    }

    public async Task<GetPaginatedChallengeOrdersResponse<ChallengeOrderDto>> GetPaginatedChallengeOrdersAsync(int currentPage, int ordersPerPage)
    {
        var result = await _orderRepository.GetPaginatedOrders(currentPage, ordersPerPage);
        return new GetPaginatedChallengeOrdersResponse<ChallengeOrderDto>
        {
            Data = result.Challenges.Select(s => new ChallengeOrderDto
            {
                ChallengeOrderId = s.ChallengeOrderId,
                ChallengesAmount = s.ChallengesAmount,
                Date = s.Date,
                PaymentId = s.PaymentId,
                ResultDonationPrice = s.ResultDonationPrice
            }).ToList(),
            TotalCount = result.TotalCount,
            CurrentPage = currentPage,
            ChallengeOrdersPerPage = ordersPerPage,
            TotalPages = PageCounter.Count(result.TotalCount, ordersPerPage)
        };
    }

    private bool AddChallengeOrderStateIsValid(string paymentId, int challengesAmount, double resultDonationPrice)
    {
        const double logicalMinimumDonationPriceValue = 0.1;
        return !string.IsNullOrWhiteSpace(paymentId)
               && challengesAmount > 0
               && resultDonationPrice > logicalMinimumDonationPriceValue;
    }
}