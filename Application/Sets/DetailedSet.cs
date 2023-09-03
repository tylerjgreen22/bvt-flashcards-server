using Application.Core;
using Application.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Sets
{
    // Gets the detailed information of a single set
    public class DetailedSet
    {
        // Query request comes in with the id of the set to return
        public class Query : IRequest<Result<SetWithFlashcardsDto>>
        {
            public string Id { get; set; }
        }

        // Handler that handles the query request
        public class Handler : IRequestHandler<Query, Result<SetWithFlashcardsDto>>
        {
            // Injecting data context to access DB and auto mapper via constructor
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            // Handle method that handles the query request
            public async Task<Result<SetWithFlashcardsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Find the set to return detailed information about. If not found return null
                var set = await _context.Sets.Include(x => x.AppUser).Include(x => x.Flashcards).FirstOrDefaultAsync(x => x.Id == request.Id);
                if (set == null) return null;

                // Map the set found into a set with flashcards DTO and return the set
                var setToReturn = _mapper.Map<SetWithFlashcardsDto>(set);
                return Result<SetWithFlashcardsDto>.Success(setToReturn);
            }
        }
    }
}