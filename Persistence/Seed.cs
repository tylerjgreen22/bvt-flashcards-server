using Domain;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    // Seed class to seed the database using json files
    public class Seed
    {
        public static async Task SeedAsync(DataContext context, UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        Id = "265446eb-e5f2-47a5-b0fe-c8111855315a",
                        UserName = "quizlit",
                        Email = "quizlit@test.com"
                    },
                    new AppUser
                    {
                        Id = "a887081f-26e6-447c-8108-1f3a96a44695",
                        UserName = "tyler",
                        Email = "tyler@test.com"
                    },
                    new AppUser
                    {
                        Id = "dff4c26f-fc77-409f-847f-1543922fd87f",
                        UserName = "loren",
                        Email = "loren@test.com"
                    },
                    new AppUser
                    {
                        Id = "ca587720-ba61-4e67-abf0-ab082baccd16",
                        UserName = "ray",
                        Email = "ray@test.com"
                    },
                    new AppUser
                    {
                        Id = "066a45c4-c9e1-4e0f-9f01-73177b283ac2",
                        UserName = "jorge",
                        Email = "jorge@test.com"
                    },
                    new AppUser
                    {
                        Id = "f46e2736-ed6b-422f-96a9-b8a70e63e824",
                        UserName = "jesus",
                        Email = "jesus@test.com"
                    },
                    new AppUser
                    {
                        Id = "9e873a3f-418e-4297-8f53-f2ea03434ad3",
                        UserName = "daniel",
                        Email = "daniel@test.com"
                    }
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }

            if (!context.Sets.Any())
            {
                var sets = new List<Set>
                {
                    new Set
                    {
                        Id = "C05D7500-4E01-48FE-B607-EE629282F25B",
                        Title = "HTML and CSS",
                        Description = "Collection of flashcards about HTML and CSS",
                        AppUserId = "265446eb-e5f2-47a5-b0fe-c8111855315a"
                    },
                    new Set
                    {
                        Id = "D3BE9DBE-3921-4C6B-8D3F-C2DB4516E09A",
                        Title = "JavaScript",
                        Description = "Collection of flashcards about JavaScript",
                        AppUserId = "265446eb-e5f2-47a5-b0fe-c8111855315a"
                    },
                    new Set
                    {
                        Id = "8A139723-2D80-49D1-9FA6-7E7BA6CBDD3D",
                        Title = "React",
                        Description = "",
                        AppUserId = "265446eb-e5f2-47a5-b0fe-c8111855315a"
                    },
                    new Set
                    {
                        Id = "F7D24C8C-42E7-47C4-937A-3D4E9FED30E9",
                        Title = "Python Basics",
                        Description = "Introduction to Python programming language",
                        AppUserId = "dff4c26f-fc77-409f-847f-1543922fd87f"
                    },
                    new Set
                    {
                        Id = "A5B6E344-5D02-4C4F-9DDE-26E1B379ACDA",
                        Title = "Responsive Web Design",
                        Description = "Designing websites for different devices",
                        AppUserId = "ca587720-ba61-4e67-abf0-ab082baccd16"
                    },
                    new Set
                    {
                        Id = "6741A5C7-82D1-4E1E-9F10-FE93B8D9BDE6",
                        Title = "Node.js Essentials",
                        Description = "Fundamentals of server-side JavaScript with Node.js",
                        AppUserId = "066a45c4-c9e1-4e0f-9f01-73177b283ac2"
                    },
                    new Set
                    {
                        Id = "B36EF9B6-7BBB-44FC-A739-81B11A0EB320",
                        Title = "CSS Grid Layout",
                        Description = "Creating complex layouts with CSS Grid",
                        AppUserId = "f46e2736-ed6b-422f-96a9-b8a70e63e824"
                    },
                    new Set
                    {
                        Id = "B8EB20A0-5733-402C-BF92-0A6CBA2620A2",
                        Title = "Introduction to SQL",
                        Description = "Getting started with SQL databases",
                        AppUserId = "9e873a3f-418e-4297-8f53-f2ea03434ad3"
                    },
                    new Set
                    {
                        Id = "B3F3009E-6A31-4F23-A8C1-04A4C8CDB300",
                        Title = "Responsive Web Design",
                        Description = "Creating user-friendly websites for all devices",
                        AppUserId = "dff4c26f-fc77-409f-847f-1543922fd87f"
                    },
                    new Set
                    {
                        Id = "D9A77BFC-47C9-4A5C-91F2-2C840F83E1E7",
                        Title = "Web Accessibility",
                        Description = "Making web content accessible to everyone",
                        AppUserId = "ca587720-ba61-4e67-abf0-ab082baccd16"
                    },
                    new Set
                    {
                        Id = "DCFDDBB3-93AE-42DD-B6FA-25E0B2B392F3",
                        Title = "RESTful APIs",
                        Description = "Building APIs following REST architecture",
                        AppUserId = "066a45c4-c9e1-4e0f-9f01-73177b283ac2"
                    },
                    new Set
                    {
                        Id = "4A4EB580-C9D0-4B3D-937A-C1CE557F8D9B",
                        Title = "SASS Fundamentals",
                        Description = "Introduction to SASS preprocessor for CSS",
                        AppUserId = "f46e2736-ed6b-422f-96a9-b8a70e63e824"
                    },
                    new Set
                    {
                        Id = "765C7C53-1E53-4A61-A6AC-FCC52E67492C",
                        Title = "Database Design Principles",
                        Description = "Fundamentals of designing efficient databases",
                        AppUserId = "9e873a3f-418e-4297-8f53-f2ea03434ad3"
                    },
                    new Set
                    {
                        Id = "E48163A1-DBDF-4E9A-B7A1-FBEBFD687A1C",
                        Title = "Webpack Configuration",
                        Description = "Setting up Webpack for modern web development",
                        AppUserId = "dff4c26f-fc77-409f-847f-1543922fd87f"
                    },
                    new Set
                    {
                        Id = "1D9E4392-4D08-4F0B-9C2A-BA64DC2ECE35",
                        Title = "Responsive Design Patterns",
                        Description = "Common design patterns for responsive websites",
                        AppUserId = "ca587720-ba61-4e67-abf0-ab082baccd16"
                    },
                    new Set
                    {
                        Id = "D3A3780A-4397-47F6-BCA1-509DF82C3841",
                        Title = "RESTful API Design",
                        Description = "Best practices for designing RESTful APIs",
                        AppUserId = "066a45c4-c9e1-4e0f-9f01-73177b283ac2"
                    },
                    new Set
                    {
                        Id = "EF3C6739-46B5-43ED-9C6F-28EB6FD282E0",
                        Title = "CSS Flexbox",
                        Description = "Building flexible layouts with CSS Flexbox",
                        AppUserId = "f46e2736-ed6b-422f-96a9-b8a70e63e824"
                    },
                    new Set
                    {
                        Id = "FF16E543-6DD7-45C3-8A96-10A3E226E2E5",
                        Title = "Introduction to NoSQL Databases",
                        Description = "Exploring non-relational database options",
                        AppUserId = "9e873a3f-418e-4297-8f53-f2ea03434ad3"
                    },
                    new Set
                    {
                        Id = "6827648A-64D3-4F09-A6D9-F4321EEDCD32",
                        Title = "Front-End Performance Optimization",
                        Description = "Techniques for optimizing front-end performance",
                        AppUserId = "265446eb-e5f2-47a5-b0fe-c8111855315a"
                    },
                    new Set
                    {
                        Id = "E9E04762-23E2-46E7-82A3-3E511DF64B4E",
                        Title = "Web Animation with CSS",
                        Description = "Creating engaging animations using CSS",
                        AppUserId = "265446eb-e5f2-47a5-b0fe-c8111855315a"
                    }
                };


                await context.Sets.AddRangeAsync(sets);
            }

            if (!context.Flashcards.Any())
            {
                var flashcards = new List<Flashcard>
                {
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What does HTML stand for?",
                        Definition = "Hyper Text Markup Language",
                        PictureUrl = "test",
                        SetId = "C05D7500-4E01-48FE-B607-EE629282F25B"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What does CSS stand for?",
                        Definition = "Cascading Style Sheets",
                        PictureUrl = "",
                        SetId = "C05D7500-4E01-48FE-B607-EE629282F25B"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is the correct HTML element for the largest heading?",
                        Definition = "<h1>",
                        PictureUrl = "",
                        SetId = "C05D7500-4E01-48FE-B607-EE629282F25B"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What does the CSS property 'margin' control?",
                        Definition = "The 'margin' property controls the space outside an element's border.",
                        PictureUrl = "",
                        SetId = "C05D7500-4E01-48FE-B607-EE629282F25B"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is JavaScript?",
                        Definition = "JavaScript is a programming language",
                        PictureUrl = "",
                        SetId = "D3BE9DBE-3921-4C6B-8D3F-C2DB4516E09A"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is the output of the following code?\n\nconsole.log(2 + '2');",
                        Definition = "22",
                        PictureUrl = "",
                        SetId = "D3BE9DBE-3921-4C6B-8D3F-C2DB4516E09A"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is the correct way to create a JavaScript array?",
                        Definition = "var colors = [\"red\", \"green\", \"blue\"]",
                        PictureUrl = "test",
                        SetId = "D3BE9DBE-3921-4C6B-8D3F-C2DB4516E09A"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is the purpose of the 'this' keyword in JavaScript?",
                        Definition = "The 'this' keyword refers to the object that the function belongs to.",
                        PictureUrl = "",
                        SetId = "D3BE9DBE-3921-4C6B-8D3F-C2DB4516E09A"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is React?",
                        Definition = "React is a front-end JavaScript library",
                        PictureUrl = "",
                        SetId = "8A139723-2D80-49D1-9FA6-7E7BA6CBDD3D"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is JSX?",
                        Definition = "JSX is a syntax extension for JavaScript",
                        PictureUrl = "",
                        SetId = "8A139723-2D80-49D1-9FA6-7E7BA6CBDD3D"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is the purpose of state in React?",
                        Definition = "State is used to store and manage component data",
                        PictureUrl = "",
                        SetId = "8A139723-2D80-49D1-9FA6-7E7BA6CBDD3D"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "Which lifecycle method is used to update the component before rendering?",
                        Definition = "componentWillUpdate()",
                        PictureUrl = "",
                        SetId = "8A139723-2D80-49D1-9FA6-7E7BA6CBDD3D"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is Python's main design philosophy?",
                        Definition = "Python's main design philosophy emphasizes code readability and a clean syntax, which allows developers to express concepts in fewer lines of code.",
                        PictureUrl = "",
                        SetId = "F7D24C8C-42E7-47C4-937A-3D4E9FED30E9"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What are some basic data types in Python?",
                        Definition = "Python has several basic data types, including integers, floats, strings, and booleans.",
                        PictureUrl = "",
                        SetId = "F7D24C8C-42E7-47C4-937A-3D4E9FED30E9"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is a list in Python?",
                        Definition = "A list in Python is an ordered collection of elements that can contain various data types. Lists are mutable, meaning their elements can be changed after creation.",
                        PictureUrl = "",
                        SetId = "F7D24C8C-42E7-47C4-937A-3D4E9FED30E9"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is a tuple in Python?",
                        Definition = "A tuple in Python is an ordered collection of elements, similar to a list. However, tuples are immutable, meaning their elements cannot be changed after creation.",
                        PictureUrl = "",
                        SetId = "F7D24C8C-42E7-47C4-937A-3D4E9FED30E9"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "How do you define a function in Python?",
                        Definition = "You can define a function in Python using the 'def' keyword followed by the function name, parameters, and a colon. The function's code block is indented beneath the definition.",
                        PictureUrl = "",
                        SetId = "F7D24C8C-42E7-47C4-937A-3D4E9FED30E9"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is the difference between '==' and 'is' in Python?",
                        Definition = "In Python, '==' is used to compare the values of two objects, while 'is' is used to compare whether two objects are the same object in memory.",
                        PictureUrl = "",
                        SetId = "F7D24C8C-42E7-47C4-937A-3D4E9FED30E9"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "How do you iterate over a sequence in Python?",
                        Definition = "You can use a 'for' loop to iterate over a sequence, such as a list or a string, in Python. The loop variable takes on each value in the sequence during each iteration.",
                        PictureUrl = "",
                        SetId = "F7D24C8C-42E7-47C4-937A-3D4E9FED30E9"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is a dictionary in Python?",
                        Definition = "A dictionary in Python is an unordered collection of key-value pairs. Each key is unique and maps to a corresponding value. Dictionaries are often used for data that needs to be looked up quickly.",
                        PictureUrl = "",
                        SetId = "F7D24C8C-42E7-47C4-937A-3D4E9FED30E9"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is the 'if' statement used for in Python?",
                        Definition = "The 'if' statement in Python is used for conditional execution. It allows you to execute a block of code only if a certain condition is true.",
                        PictureUrl = "",
                        SetId = "F7D24C8C-42E7-47C4-937A-3D4E9FED30E9"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is a module in Python?",
                        Definition = "A module in Python is a file containing Python code that defines functions, classes, and variables. Modules allow you to organize your code and reuse functionality across different programs.",
                        PictureUrl = "",
                        SetId = "F7D24C8C-42E7-47C4-937A-3D4E9FED30E9"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is responsive web design?",
                        Definition = "Responsive web design is an approach to designing and coding websites that ensures they adapt and display properly on various devices and screen sizes, from desktops to mobile phones.",
                        PictureUrl = "",
                        SetId = "A5B6E344-5D02-4C4F-9DDE-26E1B379ACDA"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is a viewport?",
                        Definition = "A viewport is the visible area of a web page in a web browser. In responsive web design, viewports are adjusted to accommodate different device screen sizes and orientations.",
                        PictureUrl = "",
                        SetId = "A5B6E344-5D02-4C4F-9DDE-26E1B379ACDA"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is a media query?",
                        Definition = "A media query is a CSS technique that allows you to apply different styles based on the characteristics of the user's device, such as screen width, height, or device orientation.",
                        PictureUrl = "",
                        SetId = "A5B6E344-5D02-4C4F-9DDE-26E1B379ACDA"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is a fluid grid layout?",
                        Definition = "A fluid grid layout is a design grid that adjusts its columns and content proportions based on the screen size. It allows content to adapt smoothly to different devices.",
                        PictureUrl = "",
                        SetId = "A5B6E344-5D02-4C4F-9DDE-26E1B379ACDA"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is the 'mobile-first' approach?",
                        Definition = "The 'mobile-first' approach is a design strategy that involves designing the mobile version of a website first and then progressively enhancing it for larger screen sizes.",
                        PictureUrl = "",
                        SetId = "A5B6E344-5D02-4C4F-9DDE-26E1B379ACDA"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What are breakpoints in responsive design?",
                        Definition = "Breakpoints are specific screen widths at which the layout of a responsive website changes. Different styles or layouts can be applied at different breakpoints.",
                        PictureUrl = "",
                        SetId = "A5B6E344-5D02-4C4F-9DDE-26E1B379ACDA"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is a flexible image in responsive design?",
                        Definition = "A flexible image in responsive design is an image that scales proportionally to the size of the viewport. This prevents images from becoming too large or too small on different devices.",
                        PictureUrl = "",
                        SetId = "A5B6E344-5D02-4C4F-9DDE-26E1B379ACDA"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is CSS media query syntax?",
                        Definition = "CSS media queries use the '@media' rule followed by a set of conditions enclosed in parentheses. If the conditions match the user's device characteristics, the specified styles are applied.",
                        PictureUrl = "",
                        SetId = "A5B6E344-5D02-4C4F-9DDE-26E1B379ACDA"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is the purpose of a meta viewport tag?",
                        Definition = "The meta viewport tag in HTML provides instructions to the browser on how to control the dimensions and scaling of the viewport, helping to achieve responsive behavior.",
                        PictureUrl = "",
                        SetId = "A5B6E344-5D02-4C4F-9DDE-26E1B379ACDA"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is mobile optimization?",
                        Definition = "Mobile optimization involves tailoring the design, layout, and performance of a website specifically for mobile devices to ensure a seamless user experience.",
                        PictureUrl = "",
                        SetId = "A5B6E344-5D02-4C4F-9DDE-26E1B379ACDA"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is SQL?",
                        Definition = "SQL (Structured Query Language) is a domain-specific language used for managing and querying relational databases.",
                        PictureUrl = "",
                        SetId = "B8EB20A0-5733-402C-BF92-0A6CBA2620A2"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is web accessibility?",
                        Definition = "Web accessibility is the practice of making websites and web content usable for people with disabilities.",
                        PictureUrl = "",
                        SetId = "D9A77BFC-47C9-4A5C-91F2-2C840F83E1E7"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What are RESTful APIs?",
                        Definition = "RESTful APIs (Representational State Transfer APIs) are a set of principles for designing networked applications.",
                        PictureUrl = "",
                        SetId = "DCFDDBB3-93AE-42DD-B6FA-25E0B2B392F3"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is SASS?",
                        Definition = "SASS (Syntactically Awesome Style Sheets) is a CSS preprocessor that extends the functionality of CSS.",
                        PictureUrl = "",
                        SetId = "4A4EB580-C9D0-4B3D-937A-C1CE557F8D9B"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What are database design principles?",
                        Definition = "Database design principles involve structuring and organizing databases to ensure efficient storage and retrieval of data.",
                        PictureUrl = "",
                        SetId = "765C7C53-1E53-4A61-A6AC-FCC52E67492C"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is Webpack?",
                        Definition = "Webpack is a popular open-source JavaScript module bundler used to bundle and optimize assets for the web.",
                        PictureUrl = "",
                        SetId = "E48163A1-DBDF-4E9A-B7A1-FBEBFD687A1C"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What are responsive design patterns?",
                        Definition = "Responsive design patterns are reusable solutions for common design challenges in responsive web development.",
                        PictureUrl = "",
                        SetId = "1D9E4392-4D08-4F0B-9C2A-BA64DC2ECE35"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What are RESTful API design best practices?",
                        Definition = "RESTful API design best practices include using clear naming conventions, proper HTTP methods, and meaningful error codes.",
                        PictureUrl = "",
                        SetId = "D3A3780A-4397-47F6-BCA1-509DF82C3841"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is CSS Flexbox?",
                        Definition = "CSS Flexbox is a layout model that allows you to design complex layouts with a more efficient and predictable approach.",
                        PictureUrl = "",
                        SetId = "EF3C6739-46B5-43ED-9C6F-28EB6FD282E0"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What are NoSQL databases?",
                        Definition = "NoSQL databases are non-relational databases that provide flexible and scalable options for storing and retrieving data.",
                        PictureUrl = "",
                        SetId = "FF16E543-6DD7-45C3-8A96-10A3E226E2E5"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is front-end performance optimization?",
                        Definition = "Front-end performance optimization involves techniques to improve the speed and efficiency of a website's user interface.",
                        PictureUrl = "",
                        SetId = "6827648A-64D3-4F09-A6D9-F4321EEDCD32"
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Term = "What is web animation with CSS?",
                        Definition = "Web animation with CSS involves creating dynamic and engaging animations using CSS transitions and keyframes.",
                        PictureUrl = "",
                        SetId = "E9E04762-23E2-46E7-82A3-3E511DF64B4E"
                    }
                };


                await context.Flashcards.AddRangeAsync(flashcards);
            }

            // Saving changes if changes were made
            await context.SaveChangesAsync();
        }
    }
}