using System;
using System.Collections.Generic;
using Cqs.Domain.Models;

namespace Cqs.Backend.Tests._TestUtils
{
    public static class Stubs
    {
        public static Todo[] CreateTodos()
        {
            return new List<Todo> 
            {
                new Todo{
                    Caption = "Buy milk",
                    Category = "Shopping",
                    CreatedUtc = DateTime.Now,
                    Description = "Make sure it's low lactose!",
                    Done = false,
                    DueUtc = null
                },
                new Todo{
                    Caption = "Buy oatmeal",
                    Category = "Shopping",
                    CreatedUtc = DateTime.Now,
                    Description = "No gluten here.",
                    Done = false,
                    DueUtc = null
                },
                                new Todo{
                    Caption = "Congratulate Rickard",
                    Category = "Reminder",
                    CreatedUtc = DateTime.Now,
                    Description = "",
                    Done = false,
                    DueUtc = null
                },
                                new Todo{
                    Caption = "Call mother",
                    Category = "Reminder",
                    CreatedUtc = DateTime.Now,
                    Description = "So she don't get upset",
                    Done = false,
                    DueUtc = null
                }
            }.ToArray();
        }
    }
}
