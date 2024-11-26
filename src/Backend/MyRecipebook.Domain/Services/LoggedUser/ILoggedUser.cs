﻿using MyRecipebook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipebook.Domain.Services.LoggedUser
{
    public interface ILoggedUser
    {
        public Task<User> User();    

    }
}