﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    interface INoteRepository
    {
        Task SaveAsync(Note note);

        Task DeleteAsync(Note note);

        Task<List<Note>> GetAllAsync();
    }
}
