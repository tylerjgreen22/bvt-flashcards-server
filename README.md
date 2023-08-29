# bvt-flashcards-server
# End points

All endpoints start with the base url https://bvtflashcardsserver.fly.dev/api/

# [Get]

## /sets

Gets the sets, query parameters for sorting and filtering include: pageSize, pageNumber, search, orderBy, appUserId

Ex: https://bvtflashcardsserver.fly.dev/api/sets?orderBy=Recent&search=CSS

Gets all sets with CSS in title and ordered by recent, default pagination is 10 results per page

## /Sets/{setId}

Gets the given set with flashcards

## /account

Gets a users claims information, requires authorization header with JWT token

# [Post] - Requires Auth

## /sets

Creates a set, also creates the flashcards for that set. 

## /account/login 

Logins in a user, return JWT Token and user info

## /account/register

Registers a user, returns JWT Token and user info

## /account/delete

Deletes user account

## /pictures/add

Adds provided picture

# [Put] - Requires Auth

## /sets/{setId}

Updates a set based on the setId. Updates flashcards with set

## /account/username

Change username for account

## /account/password

Change password for account

# [Delete] - Requires Auth

## /sets/{setId}

Deletes a set based on the Set Id.
