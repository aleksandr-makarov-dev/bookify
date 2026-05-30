using Bookify.Modules.Scheduling.Application.Abstract;
using Bookify.Modules.Scheduling.Domain;
using ErrorOr;
using MediatR;

namespace Bookify.Modules.Scheduling.Application.BookedSlots;

public class CreateBookedSlotCommand : IRequest<ErrorOr<Guid>>
{
    public Guid EventTypeId { get; init; }
    public Guid OrderId { get; init; }
    public DateTime StartDateTime { get; init; }
    public DateTime EndDateTime { get; init; }
}

internal sealed class CreateBookedSlotCommandHandler(ISchedulingDataProvider dataProvider)
    : IRequestHandler<CreateBookedSlotCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(CreateBookedSlotCommand request, CancellationToken cancellationToken)
    {
        var bookedSlot = new BookedSlot
        {
            Id = Guid.NewGuid(),
            BookingId = request.OrderId,
            EventTypeId = request.EventTypeId,
            StartDateTime = request.StartDateTime,
            EndDateTime = request.EndDateTime
        };

        dataProvider.BookedSlots.Add(bookedSlot);

        await dataProvider.SaveChangesAsync(cancellationToken);

        return bookedSlot.Id;
    }
}