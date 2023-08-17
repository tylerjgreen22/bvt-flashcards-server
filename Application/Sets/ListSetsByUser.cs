using Application.Core;
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Application.DTOs;

namespace Application.Sets
{
    public class ListSetsByUser
    {
        // Creating a query that extends IRequest with a type of a list of flashcards
        public class Query : IRequest<Result<List<SetDto>>>
        {
            public string UserId { get; set; }
        }

        // Handler class that uses the query of type Flashcard list to process the request to Mediator
        public class Handler : IRequestHandler<Query, Result<List<SetDto>>>
        {
            // Injecting data context to access DB
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            // Handle method that uses the created query to obtain the requested flashcard
            public async Task<Result<List<SetDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var sets = await _context.Sets.Where(set => set.AppUserId == request.UserId).Include(x => x.AppUser).ToListAsync();
                var setsDto = _mapper.Map<List<SetDto>>(sets);

                return Result<List<SetDto>>.Success(setsDto);
            }
        }
    }
}
