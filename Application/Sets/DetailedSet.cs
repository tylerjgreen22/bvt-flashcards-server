using Application.Core;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Sets
{
    // Mediator class for getting a single set. Method returns are wrapped in a result object that faciliates error handling
    public class DetailedSet
    {
        // Creating a query that extends IRequest with type of Flashcard and contains a property Id
        public class Query : IRequest<Result<SetWithFlashcardsDto>>
        {
            public string Id { get; set; }
        }

        // Handler class that uses the query of type Flashcard to process the request to Mediator
        public class Handler : IRequestHandler<Query, Result<SetWithFlashcardsDto>>
        {
            // Injecting data context to access DB
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            // Handle method that uses the created query to obtain the requested flashcard
            public async Task<Result<SetWithFlashcardsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var set = await _context.Sets.Include(x => x.AppUser).Include(x => x.Flashcards).FirstOrDefaultAsync(x => x.Id == request.Id);
                var setToReturn = _mapper.Map<SetWithFlashcardsDto>(set);
                return Result<SetWithFlashcardsDto>.Success(setToReturn);
            }
        }
    }
}