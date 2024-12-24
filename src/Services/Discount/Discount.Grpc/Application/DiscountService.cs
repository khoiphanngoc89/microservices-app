namespace Discount.Grpc.Application;

public sealed class DiscountService
    (DiscountDbContext dbContext, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (coupon is null)
        {
            coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount" };
        }

        logger.LogInformation("Discount is retrieved for ProductName: {productName}, Amount: {amount}", coupon.ProductName, coupon.Amount);
        var result = coupon.Adapt<CouponModel>();

        return result;
    }

    public async override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
        }

        await dbContext.Coupons.AddAsync(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount is successful created. ProductName: {ProductName}", coupon.ProductName);

        var result = coupon.Adapt<CouponModel>();
        return result;
    }

    public async override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        // Property pattern used to match an expression
        // if the result of expression is not nill and if
        // nested pattern successfully matches
        // the corresponding property or field of that expression result
        if (coupon is { Id: <= 0 } || await dbContext.Coupons.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == coupon.Id) is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
        }

        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount is successful update. ProductName: {ProductName}", coupon.ProductName);

        var result = coupon.Adapt<CouponModel>();
        return result;
    }

    public async override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, string.Format("Discount with ProductName={0} is not found", request.ProductName)));
        }

        dbContext.Remove(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount is successful deleted. ProductName: {ProductName}", coupon.ProductName);

        return new DeleteDiscountResponse { Success = true };
    }
}
