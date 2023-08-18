using Application.Core;
using Application.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Sets
{
    // Mediator class for getting a list of sets. Method returns are wrapped in a result object that faciliates error handling
    public class ListSets
    {
        // Creating a query that extends IRequest with a type of a list of flashcards
        public class Query : IRequest<Result<PagedList<SetDto>>>
        {
            public PagingParams Params { get; set; }
        }

        // Handler class that uses the query of type Flashcard list to process the request to Mediator
        public class Handler : IRequestHandler<Query, Result<PagedList<SetDto>>>
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
            public async Task<Result<PagedList<SetDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var setsQuery = _context.Sets.Include(x => x.AppUser).OrderBy(s => s.Title.ToLower()).AsQueryable();
                if (!string.IsNullOrEmpty(request.Params.AppUserId))
                {
                    setsQuery = setsQuery.Where(s => s.AppUserId == request.Params.AppUserId);
                }
                if (!string.IsNullOrEmpty(request.Params.Search))
                {
                    setsQuery = setsQuery.Where(s => s.Title.ToLower().Contains(request.Params.Search.ToLower()));
                }
                if (!string.IsNullOrEmpty(request.Params.OrderBy))
                {
                    setsQuery = setsQuery.OrderByDescending(s => s.CreatedAt);
                }
                var setsDtoQuery = setsQuery.ProjectTo<SetDto>(_mapper.ConfigurationProvider);

                return Result<PagedList<SetDto>>.Success(await PagedList<SetDto>.CreateAsync(setsDtoQuery, request.Params.PageNumber, request.Params.PageSize));
            }
        }
    }
}