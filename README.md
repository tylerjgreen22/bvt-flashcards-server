# bvt-flashcards-server
# End points

All endpoints start with the base url https://bvtflashcardsserver.fly.dev/api/

### [Get]

# /sets

Gets the sets, query parameters for sorting and filtering include: pageSize, pageNumber, search, orderBy, appUserId

Ex: https://bvtflashcardsserver.fly.dev/api/sets?orderBy=Recent&search=CSS

Gets all sets with CSS in title and ordered by recent

# /flashcards/{setId}

Gets the flashcards for a given set

# /account

Gets a users claims information, requires authorization header with JWT token

[Post]

# /sets

Creates a set, also creates the flashcards for that set. Requires Auth

# /account/login

Logins in a user, return JWT Token and user info

# /account/register

Registers a user, returns JWT Token and user info

[Put]

# /sets/{setId}

Updates a set based on the setId. Only updates the set information such as title and description. Requires Auth

# /flashcards/{setId}

Updates the flashcards based on the set they belong too. Requires Auth

[Delete]

# /sets/{setId}

Deletes a set based on the Set Id. Not currently functioning
