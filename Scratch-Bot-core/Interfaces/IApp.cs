﻿using System.Threading;
using System.Threading.Tasks;

namespace Scratch_Bot_core
{
    public interface IApp
    {
        Task Run(string token);
    }
}
