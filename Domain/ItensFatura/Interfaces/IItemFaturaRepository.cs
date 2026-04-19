using Domain.ItensFatura.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ItensFatura.Interfaces
{
    public interface IItemFaturaRepository
    {
        Task<ItemFatura> ObterPorIdAsync(Guid id);
    }
}
