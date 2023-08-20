using Application.Core;
using Application.DTOs;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Flashcards
{
    // Mediator class for getting a list of flashcards by the set id. Method returns are wrapped in a result object that faciliates error handling
    public class ListFlashcards
    {
        // Creating a query that extends IRequest with a type of a list of flashcards
        public class Query : IRequest<Result<List<FlashcardDto>>>
        {
            public string SetId { get; set; }
        }

        // Handler class that uses the query of type Flashcard list to process the request to Mediator
        public class Handler : IRequestHandler<Query, Result<List<FlashcardDto>>>
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
            public async Task<Result<List<FlashcardDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var flashcards = await _context.Flashcards.Where(flashcard => flashcard.SetId == request.SetId).ToListAsync();
                if (!flashcards.Any()) return null;
                var flashcardsDto = _mapper.Map<List<FlashcardDto>>(flashcards);

                return Result<List<FlashcardDto>>.Success(flashcardsDto);
            }
        }
    }
}