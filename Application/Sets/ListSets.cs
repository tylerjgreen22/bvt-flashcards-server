using Application.Core;
using Application.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Sets
{
    // Mediator class for getting a list of sets. Method returns are wrapped in a result object that faciliates error handling
    public class ListSets
    {
        // Creating a query that extends IRequest with a type of a list of flashcards
        public class Query : IRequest<Result<List<SetDto>>> { }

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
                var sets = await _context.Sets.Include(x => x.AppUser).Include(x => x.Flashcards).ToListAsync();
                var setsDto = _mapper.Map<List<SetDto>>(sets);

                return Result<List<SetDto>>.Success(setsDto);
            }
        }
    }
}