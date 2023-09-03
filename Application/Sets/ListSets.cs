using Application.Core;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Sets
{
    // Lists the sets based on the provided paging params
    public class ListSets
    {
        // Query request comes in with the paging params
        public class Query : IRequest<Result<PagedList<SetDto>>>
        {
            public PagingParams Params { get; set; }
        }

        // Handler that handles the query request
        public class Handler : IRequestHandler<Query, Result<PagedList<SetDto>>>
        {
            // Injecting db context, auto mapper and user accessor via constructor
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper)
            {
                _userAccessor = userAccessor;
                _context = context;
                _mapper = mapper;

            }

            // Handle method that handles the query request
            public async Task<Result<PagedList<SetDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Create a query that includes the app user, flashcards and is ordered by title
                var setsQuery = _context.Sets.Include(x => x.AppUser).Include(x => x.Flashcards).OrderBy(s => s.Title.ToLower()).AsQueryable();

                // If the params includes by user, find the user that made the request and modify the query to only retrieve that users sets
                if (request.Params.ByUser)
                {
                    // Find the user making the request using the user accessor. If not found return null
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                    if (user == null) return null;

                    // Add the where criteria to the query to find the sets belonging to the user making the request
                    setsQuery = setsQuery.Where(s => s.AppUserId == user.Id);
                }

                // If the params include a search, add the contains criteria to find sets where the title matches the search
                if (!string.IsNullOrEmpty(request.Params.Search))
                {
                    setsQuery = setsQuery.Where(s => s.Title.ToLower().Contains(request.Params.Search.ToLower()));
                }

                // If the params include orderBy, add the order by criteria to the query
                if (!string.IsNullOrEmpty(request.Params.OrderBy))
                {
                    setsQuery = setsQuery.OrderByDescending(s => s.CreatedAt);
                }

                // Project the query to a set DTO using auto mapper to return a SetDto instead of a set
                var setsDtoQuery = setsQuery.ProjectTo<SetDto>(_mapper.ConfigurationProvider);

                // Create a paged list using the constructed query, as well as page number and size params from the request params if provided
                return Result<PagedList<SetDto>>.Success(await PagedList<SetDto>.CreateAsync(setsDtoQuery, request.Params.PageNumber, request.Params.PageSize));
            }
        }
    }
}