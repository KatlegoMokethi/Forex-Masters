using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForexMasters_site.Models.Entities;

namespace ForexMasters_site.Models.Data
{
    public interface IRepositoryWrapper
    {
        IVideoRepository Video { get; }
        ICategoryRepository Category { get; }
        IDocumentRepository Document { get; }
    }
}
